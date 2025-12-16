using ToDoApp.Domain.Common;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Application.Interfaces;

public interface IUserService
{
    Task<FilterUsersDto> FilterUsers(FilterUsersDto filter, CancellationToken cancellationToken = default);
    Task<Result<ulong>> CreateUser(CreateUserDto model, CancellationToken cancellationToken = default);
    Task<Result<EditUserDto>> GetUserForEdit(ulong userId, CancellationToken cancellationToken = default);
    Task<Result> EditUser(EditUserDto model, CancellationToken cancellationToken = default);
    Task<Result> DeleteUser(ulong userId, CancellationToken cancellationToken = default);
    Task<UserRolesDto> GetUserRoles(ulong userId, CancellationToken cancellationToken = default);
    Task<List<Domain.Entities.Account.Role>> GetAllRoles(CancellationToken cancellationToken = default);
}
