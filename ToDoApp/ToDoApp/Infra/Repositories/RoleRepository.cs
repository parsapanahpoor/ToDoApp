using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infra;

namespace ToDoApp.Infra.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Role> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.Title == title && !r.IsDelete, cancellationToken);
    }

    public async Task<bool> IsTitleExistsAsync(string title, ulong? excludeRoleId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(r => r.Title == title && !r.IsDelete);

        if (excludeRoleId.HasValue)
            query = query.Where(r => r.Id != excludeRoleId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<List<Role>> GetRolesByIdsAsync(List<ulong> roleIds, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => roleIds.Contains(r.Id) && !r.IsDelete)
            .ToListAsync(cancellationToken);
    }
}
