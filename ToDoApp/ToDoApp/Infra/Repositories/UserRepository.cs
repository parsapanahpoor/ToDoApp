using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infra;

namespace ToDoApp.Infra.Repositories;

public class UserRepository : GenericRepository<UserEntity>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UserEntity> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && !u.IsDelete, cancellationToken);
    }

    public async Task<UserEntity> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDelete, cancellationToken);
    }

    public async Task<UserEntity> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.UserName == userName && !u.IsDelete, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberExistsAsync(string phoneNumber, ulong? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(u => u.PhoneNumber == phoneNumber && !u.IsDelete);

        if (excludeUserId.HasValue)
            query = query.Where(u => u.Id != excludeUserId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> IsEmailExistsAsync(string email, ulong? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(u => u.Email == email && !u.IsDelete);

        if (excludeUserId.HasValue)
            query = query.Where(u => u.Id != excludeUserId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> IsUserNameExistsAsync(string userName, ulong? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(u => u.UserName == userName && !u.IsDelete);

        if (excludeUserId.HasValue)
            query = query.Where(u => u.Id != excludeUserId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<List<UserEntity>> GetUsersWithRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => !u.IsDelete)
            .ToListAsync(cancellationToken);
    }
}
