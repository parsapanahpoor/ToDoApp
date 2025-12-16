using Microsoft.Extensions.Logging;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Common;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Application.Services.Account;

public class RoleServiceNew : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RoleServiceNew> _logger;

    public RoleServiceNew(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        ILogger<RoleServiceNew> logger)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #region Filter Roles

    public async Task<FilterRolesDto> FilterRoles(FilterRolesDto filter, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Filtering roles with parameters: {@Filter}", filter);

            var query = _roleRepository.GetQueryable()
                .OrderByDescending(p => p.CreateDate)
                .AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.RoleTitle))
                query = query.Where(s => s.Title.Contains(filter.RoleTitle));

            #endregion

            await filter.Paging(query);

            _logger.LogInformation("Successfully filtered {Count} roles", filter.Entities?.Count ?? 0);

            return filter;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering roles");
            throw;
        }
    }

    #endregion

    #region Create Role

    public async Task<Result<ulong>> CreateRole(CreateRoleDto model, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new role with title: {Title}", model.Title);

            // Validation: Check if title already exists
            if (await _roleRepository.IsTitleExistsAsync(model.Title, cancellationToken: cancellationToken))
            {
                _logger.LogWarning("Role title {Title} already exists", model.Title);
                return Result<ulong>.Failure("«?Ã«œ ‰ﬁ‘ »« Œÿ« „Ê«ÃÂ ‘œ", "⁄‰Ê«‰ ‰ﬁ‘  ò—«—? «” ");
            }

            var newRole = new Role
            {
                Title = model.Title,
                CreateDate = DateTime.UtcNow
            };

            await _roleRepository.AddAsync(newRole, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully created role {Title} with ID {RoleId}", model.Title, newRole.Id);

            return Result<ulong>.Success(newRole.Id, "‰ﬁ‘ »« „Ê›ﬁ?  «?Ã«œ ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role {Title}", model.Title);
            return Result<ulong>.Failure("Œÿ« œ— «?Ã«œ ‰ﬁ‘", ex.Message);
        }
    }

    #endregion

    #region Edit Role

    public async Task<Result<EditRoleDto>> GetRoleForEdit(ulong roleId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting role {RoleId} for edit", roleId);

            var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);

            if (role == null || role.IsDelete)
            {
                _logger.LogWarning("Role {RoleId} not found", roleId);
                return Result<EditRoleDto>.Failure("‰ﬁ‘ ?«›  ‰‘œ");
            }

            var editDto = new EditRoleDto
            {
                Id = role.Id,
                Title = role.Title
            };

            _logger.LogInformation("Successfully retrieved role {RoleId} for edit", roleId);

            return Result<EditRoleDto>.Success(editDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role {RoleId} for edit", roleId);
            return Result<EditRoleDto>.Failure("Œÿ« œ— œ—?«›  «ÿ·«⁄«  ‰ﬁ‘", ex.Message);
        }
    }

    public async Task<Result> EditRole(EditRoleDto model, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Editing role {RoleId}", model.Id);

            var role = await _roleRepository.GetByIdAsync(model.Id, cancellationToken);

            if (role == null || role.IsDelete)
            {
                _logger.LogWarning("Role {RoleId} not found", model.Id);
                return Result.Failure("‰ﬁ‘ ?«›  ‰‘œ");
            }

            // Validation: Check if title exists for other roles
            if (await _roleRepository.IsTitleExistsAsync(model.Title, model.Id, cancellationToken))
            {
                _logger.LogWarning("Role title {Title} already exists for another role", model.Title);
                return Result.Failure("Ê?—«?‘ ‰ﬁ‘ »« Œÿ« „Ê«ÃÂ ‘œ", "⁄‰Ê«‰ ‰ﬁ‘  ò—«—? «” ");
            }

            role.Title = model.Title;
            role.Update();

            _roleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully edited role {RoleId}", model.Id);

            return Result.Success("‰ﬁ‘ »« „Ê›ﬁ?  Ê?—«?‘ ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing role {RoleId}", model.Id);
            return Result.Failure("Œÿ« œ— Ê?—«?‘ ‰ﬁ‘", ex.Message);
        }
    }

    #endregion

    #region Delete Role

    public async Task<Result> DeleteRole(ulong roleId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting role {RoleId}", roleId);

            var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);

            if (role == null || role.IsDelete)
            {
                _logger.LogWarning("Role {RoleId} not found", roleId);
                return Result.Failure("‰ﬁ‘ ?«›  ‰‘œ");
            }

            role.IsDelete = true;
            role.Update();

            _roleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted role {RoleId}", roleId);

            return Result.Success("‰ﬁ‘ »« „Ê›ﬁ?  Õ–› ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting role {RoleId}", roleId);
            return Result.Failure("Œÿ« œ— Õ–› ‰ﬁ‘", ex.Message);
        }
    }

    #endregion
}
