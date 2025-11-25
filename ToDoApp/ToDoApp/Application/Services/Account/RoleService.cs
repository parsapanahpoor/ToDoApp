using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoApp.Domain.Model.Account;
using ToDoApp.Infra;

namespace ToDoApp.Application.Services.Account;

public class RoleService(ApplicationDbContext context)
{
    public async Task<FilterRolesDto> FilterRoles(
        FilterRolesDto filter)
    {
        var query = context.Roles
            .Where(s => !s.IsDelete)
            .OrderByDescending(p=> p.CreateDate)
            .AsQueryable();

        #region Filter

        if (!string.IsNullOrEmpty(filter.RoleTitle))
            query = query.Where(s => s.Title.Contains(filter.RoleTitle));

        #endregion

        await filter.Paging(query);

        return filter;
    }

    public async Task<bool> CreateRole(CreateRoleDto model)
    {
        if (string.IsNullOrEmpty(model.Title))
            return false;

        if (await context.Roles.AnyAsync(p => p.Title == model.Title))
            return false;

        var newRole = new Domain.Entities.Account.Role()
        {
            Title = model.Title
        };

        await context.Roles.AddAsync(newRole);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<EditRoleDto> FillEditRoleDto(ulong id)
    {
        var role = await context.Roles.FindAsync(id);
        if (role == null)
            return null;

        return new EditRoleDto()
        {
            Id = id,
            Title = role.Title
        };
    }

    public async Task<bool> EditRole(EditRoleDto edit)
    {
        var role = await context.Roles.FindAsync(edit.Id);
        if (role == null)
            return false;

        if (await context.Roles.AnyAsync(p => p.Title == edit.Title && p.Id != edit.Id))
            return false;

        role.Title = edit.Title;

        context.Roles.Update(role);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteRole(ulong roleId)
    {
        var role = await context.Roles.FindAsync(roleId);
        if (role == null)
            return false;

        role.IsDelete = true;

        context.Roles.Update(role);

        //Delete User Role 
        var userRoles = await context.UserRoles
            .Where(p => !p.IsDelete && p.RoleId == role.Id)
            .ToListAsync();

        if (userRoles != null || userRoles.Any())
        {
            foreach (var item in userRoles)
            {
                context.UserRoles.Remove(item);
            }
        }

        await context.SaveChangesAsync();

        return true;
    }
}