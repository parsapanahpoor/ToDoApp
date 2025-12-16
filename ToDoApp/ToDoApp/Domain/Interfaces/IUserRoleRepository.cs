using ToDoApp.Domain.Entities.Account;

namespace ToDoApp.Domain.Interfaces;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<List<UserRole>> GetUserRolesByUserIdAsync(ulong userId, CancellationToken cancellationToken = default);
    Task<List<ulong>> GetRoleIdsByUserIdAsync(ulong userId, CancellationToken cancellationToken = default);
    Task RemoveUserRolesAsync(ulong userId, CancellationToken cancellationToken = default);
    Task<bool> HasUserAnyRoleAsync(ulong userId, CancellationToken cancellationToken = default);
}
