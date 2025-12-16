using ToDoApp.Domain.Entities.Account;

namespace ToDoApp.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<UserEntity>
{
    Task<UserEntity> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<UserEntity> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserEntity> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneNumberExistsAsync(string phoneNumber, ulong? excludeUserId = null, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistsAsync(string email, ulong? excludeUserId = null, CancellationToken cancellationToken = default);
    Task<bool> IsUserNameExistsAsync(string userName, ulong? excludeUserId = null, CancellationToken cancellationToken = default);
    Task<List<UserEntity>> GetUsersWithRolesAsync(CancellationToken cancellationToken = default);
}
