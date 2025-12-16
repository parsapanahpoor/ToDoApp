using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infra;

namespace ToDoApp.Infra.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<UserRole>> GetUserRolesByUserIdAsync(ulong userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ur => ur.UserId == userId && !ur.IsDelete)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ulong>> GetRoleIdsByUserIdAsync(ulong userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ur => ur.UserId == userId && !ur.IsDelete)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveUserRolesAsync(ulong userId, CancellationToken cancellationToken = default)
    {
        var userRoles = await GetUserRolesByUserIdAsync(userId, cancellationToken);
        
        if (userRoles.Any())
        {
            RemoveRange(userRoles);
        }
    }

    public async Task<bool> HasUserAnyRoleAsync(ulong userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(ur => ur.UserId == userId && !ur.IsDelete, cancellationToken);
    }
}
