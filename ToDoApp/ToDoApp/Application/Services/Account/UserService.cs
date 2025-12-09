using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Model.Account;
using ToDoApp.Infra;

namespace ToDoApp.Application.Services.Account;

public class UserService(ApplicationDbContext context)
{
    #region Filter Users

    public async Task<FilterUsersDto> FilterUsers(FilterUsersDto filter)
    {
        var query = context.Users
            .Where(s => !s.IsDelete)
            .OrderByDescending(p => p.CreateDate)
            .AsQueryable();

        #region Filter

        if (!string.IsNullOrEmpty(filter.UserName))
            query = query.Where(s => s.UserName.Contains(filter.UserName));

        if (!string.IsNullOrEmpty(filter.Email))
            query = query.Where(s => s.Email.Contains(filter.Email));

        if (!string.IsNullOrEmpty(filter.PhoneNumber))
            query = query.Where(s => s.PhoneNumber.Contains(filter.PhoneNumber));

        #endregion

        await filter.Paging(query);

        return filter;
    }

    #endregion

    #region Create User

    public async Task<bool> CreateUser(CreateUserDto model)
    {
        // Check if username already exists
        if (await context.Users.AnyAsync(p => p.UserName == model.UserName && !p.IsDelete))
            return false;

        // Check if email already exists
        if (await context.Users.AnyAsync(p => p.Email == model.Email && !p.IsDelete))
            return false;

        // Hash password (simple version - in production use proper password hashing like BCrypt or Identity)
        var hashedPassword = HashPassword(model.Password);

        var newUser = new UserEntity
        {
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Password = hashedPassword,
            UserAvatar = "default-avatar.png"
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        // Add user roles
        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
            foreach (var roleId in model.SelectedRoles)
            {
                var userRole = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = roleId
                };
                await context.UserRoles.AddAsync(userRole);
            }
            await context.SaveChangesAsync();
        }

        return true;
    }

    #endregion

    #region Edit User

    public async Task<EditUserDto> FillEditUserDto(ulong userId)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDelete);

        if (user == null)
            return null;

        var userRoles = await context.UserRoles
            .Where(ur => ur.UserId == userId && !ur.IsDelete)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        return new EditUserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            SelectedRoles = userRoles
        };
    }

    public async Task<bool> EditUser(EditUserDto model)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == model.Id && !u.IsDelete);

        if (user == null)
            return false;

        // Check if username exists for other users
        if (await context.Users.AnyAsync(p => p.UserName == model.UserName && p.Id != model.Id && !p.IsDelete))
            return false;

        // Check if email exists for other users
        if (await context.Users.AnyAsync(p => p.Email == model.Email && p.Id != model.Id && !p.IsDelete))
            return false;

        user.UserName = model.UserName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        // Update password only if provided
        if (!string.IsNullOrEmpty(model.Password))
        {
            user.Password = HashPassword(model.Password);
        }

        user.Update();
        context.Users.Update(user);

        // Update user roles
        var existingRoles = await context.UserRoles
            .Where(ur => ur.UserId == model.Id && !ur.IsDelete)
            .ToListAsync();

        // Remove old roles
        context.UserRoles.RemoveRange(existingRoles);

        // Add new roles
        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
            foreach (var roleId in model.SelectedRoles)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId
                };
                await context.UserRoles.AddAsync(userRole);
            }
        }

        await context.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Delete User

    public async Task<bool> DeleteUser(ulong userId)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDelete);

        if (user == null)
            return false;

        user.IsDelete = true;
        user.Update();
        context.Users.Update(user);

        // Soft delete user roles
        var userRoles = await context.UserRoles
            .Where(ur => ur.UserId == userId && !ur.IsDelete)
            .ToListAsync();

        foreach (var userRole in userRoles)
        {
            userRole.IsDelete = true;
            context.UserRoles.Update(userRole);
        }

        await context.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Get User Roles

    public async Task<UserRolesDto> GetUserRoles(ulong userId)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDelete);

        if (user == null)
            return null;

        var allRoles = await context.Roles
            .Where(r => !r.IsDelete)
            .ToListAsync();

        var userRoleIds = await context.UserRoles
            .Where(ur => ur.UserId == userId && !ur.IsDelete)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        var userRoles = new UserRolesDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Roles = allRoles.Select(r => new UserRoleItemDto
            {
                RoleId = r.Id,
                RoleTitle = r.Title,
                IsSelected = userRoleIds.Contains(r.Id)
            }).ToList()
        };

        return userRoles;
    }

    #endregion

    #region Get All Roles

    public async Task<List<Role>> GetAllRoles()
    {
        return await context.Roles
            .Where(r => !r.IsDelete)
            .OrderBy(r => r.Title)
            .ToListAsync();
    }

    #endregion

    #region Helper Methods

    private string HashPassword(string password)
    {
        // Simple SHA256 hashing - in production use BCrypt or ASP.NET Core Identity
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }

    #endregion
}