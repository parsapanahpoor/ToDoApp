using ToDoApp.Domain.Common;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Application.Interfaces;

public interface IRoleService
{
    Task<FilterRolesDto> FilterRoles(FilterRolesDto filter, CancellationToken cancellationToken = default);
    Task<Result<ulong>> CreateRole(CreateRoleDto model, CancellationToken cancellationToken = default);
    Task<Result<EditRoleDto>> GetRoleForEdit(ulong roleId, CancellationToken cancellationToken = default);
    Task<Result> EditRole(EditRoleDto model, CancellationToken cancellationToken = default);
    Task<Result> DeleteRole(ulong roleId, CancellationToken cancellationToken = default);
}
