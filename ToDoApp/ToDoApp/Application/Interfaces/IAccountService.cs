using ToDoApp.Domain.Common;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Application.Interfaces;

public interface IAccountService
{
    Task<Result<ulong>> Register(RegisterUserDto model, CancellationToken cancellationToken = default);
    Task<Result<Domain.Entities.Account.UserEntity>> Login(LoginUserDto model, CancellationToken cancellationToken = default);
}
