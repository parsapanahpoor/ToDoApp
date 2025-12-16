using ToDoApp.Domain.Entities.Account;

namespace ToDoApp.Domain.Interfaces;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<bool> IsTitleExistsAsync(string title, ulong? excludeRoleId = null, CancellationToken cancellationToken = default);
    Task<List<Role>> GetRolesByIdsAsync(List<ulong> roleIds, CancellationToken cancellationToken = default);
}
