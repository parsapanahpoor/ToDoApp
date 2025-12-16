using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoApp.Application.Helpers;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Common;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Application.Services.Account;

public class UserServiceNew : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserServiceNew> _logger;

    public UserServiceNew(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IUnitOfWork unitOfWork,
        ILogger<UserServiceNew> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #region Filter Users

    public async Task<FilterUsersDto> FilterUsers(FilterUsersDto filter, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Filtering users with parameters: {@Filter}", filter);

            var query = _userRepository.GetQueryable()
                .OrderByDescending(p => p.CreateDate)
                .AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.UserName))
                query = query.Where(s => s.UserName.Contains(filter.UserName));

            if (!string.IsNullOrEmpty(filter.Email))
                query = query.Where(s => s.Email.Contains(filter.Email));

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
                query = query.Where(s => s.PhoneNumber.Contains(filter.PhoneNumber));

            #endregion

            await filter.Paging(query);

            _logger.LogInformation("Successfully filtered {Count} users", filter.Entities?.Count ?? 0);

            return filter;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering users");
            throw;
        }
    }

    #endregion

    #region Create User

    public async Task<Result<ulong>> CreateUser(CreateUserDto model, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user with username: {UserName}", model.UserName);

            // Validation: Check if username already exists
            if (await _userRepository.IsUserNameExistsAsync(model.UserName, cancellationToken: cancellationToken))
            {
                _logger.LogWarning("Username {UserName} already exists", model.UserName);
                return Result<ulong>.Failure("«?Ã«œ ò«—»— »« Œÿ« „Ê«ÃÂ ‘œ", "‰«„ ò«—»—?  ò—«—? «” ");
            }

            // Validation: Check if email already exists
            if (await _userRepository.IsEmailExistsAsync(model.Email, cancellationToken: cancellationToken))
            {
                _logger.LogWarning("Email {Email} already exists", model.Email);
                return Result<ulong>.Failure("«?Ã«œ ò«—»— »« Œÿ« „Ê«ÃÂ ‘œ", "«?„?·  ò—«—? «” ");
            }

            // Validation: Check if phone number already exists
            if (await _userRepository.IsPhoneNumberExistsAsync(model.PhoneNumber, cancellationToken: cancellationToken))
            {
                _logger.LogWarning("PhoneNumber {PhoneNumber} already exists", model.PhoneNumber);
                return Result<ulong>.Failure("«?Ã«œ ò«—»— »« Œÿ« „Ê«ÃÂ ‘œ", "‘„«—Â  ·›‰  ò—«—? «” ");
            }

            // Validation: Check if roles exist
            if (model.SelectedRoles != null && model.SelectedRoles.Any())
            {
                var existingRoles = await _roleRepository.GetRolesByIdsAsync(model.SelectedRoles, cancellationToken);
                if (existingRoles.Count != model.SelectedRoles.Count)
                {
                    _logger.LogWarning("Some roles do not exist for user {UserName}", model.UserName);
                    return Result<ulong>.Failure("«?Ã«œ ò«—»— »« Œÿ« „Ê«ÃÂ ‘œ", "»—Œ? «“ ‰ﬁ‘ùÂ« „⁄ »— ‰?” ‰œ");
                }
            }

            // Begin Transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Hash password
                var hashedPassword = PasswordHelper.HashPassword(model.Password);

                var newUser = new UserEntity
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = hashedPassword,
                    UserAvatar = "default-avatar.png",
                    CreateDate = DateTime.UtcNow
                };

                await _userRepository.AddAsync(newUser, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Add user roles
                if (model.SelectedRoles != null && model.SelectedRoles.Any())
                {
                    var userRoles = model.SelectedRoles.Select(roleId => new UserRole
                    {
                        UserId = newUser.Id,
                        RoleId = roleId,
                        CreateDate = DateTime.UtcNow
                    }).ToList();

                    await _userRoleRepository.AddRangeAsync(userRoles, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                // Commit Transaction
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Successfully created user {UserName} with ID {UserId}", model.UserName, newUser.Id);

                return Result<ulong>.Success(newUser.Id, "ò«—»— »« „Ê›ﬁ?  «?Ã«œ ‘œ");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user {UserName}", model.UserName);
            return Result<ulong>.Failure("Œÿ« œ— «?Ã«œ ò«—»—", ex.Message);
        }
    }

    #endregion

    #region Edit User

    public async Task<Result<EditUserDto>> GetUserForEdit(ulong userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting user {UserId} for edit", userId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null || user.IsDelete)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                return Result<EditUserDto>.Failure("ò«—»— ?«›  ‰‘œ");
            }

            var userRoleIds = await _userRoleRepository.GetRoleIdsByUserIdAsync(userId, cancellationToken);

            var editDto = new EditUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                SelectedRoles = userRoleIds
            };

            _logger.LogInformation("Successfully retrieved user {UserId} for edit", userId);

            return Result<EditUserDto>.Success(editDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user {UserId} for edit", userId);
            return Result<EditUserDto>.Failure("Œÿ« œ— œ—?«›  «ÿ·«⁄«  ò«—»—", ex.Message);
        }
    }

    public async Task<Result> EditUser(EditUserDto model, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Editing user {UserId}", model.Id);

            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);

            if (user == null || user.IsDelete)
            {
                _logger.LogWarning("User {UserId} not found", model.Id);
                return Result.Failure("ò«—»— ?«›  ‰‘œ");
            }

            // Validation: Check if username exists for other users
            if (await _userRepository.IsUserNameExistsAsync(model.UserName, model.Id, cancellationToken))
            {
                _logger.LogWarning("Username {UserName} already exists for another user", model.UserName);
                return Result.Failure("Ê?—«?‘ ò«—»— »« Œÿ« „Ê«ÃÂ ‘œ", "‰«„ ò«—»—?  ò—«—? «” ");
            }

            // Validation: Check if email exists for other users
            if (await _userRepository.IsEmailExistsAsync(model.Email, model.Id, cancellationToken))
            {
                _logger.LogWarning("Email {Email} already exists for another user", model.Email);
                return Result.Failure("Ê?—«?‘ ò«—»— »« Œÿ« „Ê«ÃÂ ‘œ", "«?„?·  ò—«—? «” ");
            }

            // Validation: Check if phone number exists for other users
            if (await _userRepository.IsPhoneNumberExistsAsync(model.PhoneNumber, model.Id, cancellationToken))
            {
                _logger.LogWarning("PhoneNumber {PhoneNumber} already exists for another user", model.PhoneNumber);
                return Result.Failure("Ê?—«?‘ ò«—»— »« Œÿ« „Ê«ÃÂ ‘œ", "‘„«—Â  ·›‰  ò—«—? «” ");
            }

            // Validation: Check if roles exist
            if (model.SelectedRoles != null && model.SelectedRoles.Any())
            {
                var existingRoles = await _roleRepository.GetRolesByIdsAsync(model.SelectedRoles, cancellationToken);
                if (existingRoles.Count != model.SelectedRoles.Count)
                {
                    _logger.LogWarning("Some roles do not exist for user {UserId}", model.Id);
                    return Result.Failure("Ê?—«?‘ ò«—»— »« Œÿ« „Ê«ÃÂ ‘œ", "»—Œ? «“ ‰ﬁ‘ùÂ« „⁄ »— ‰?” ‰œ");
                }
            }

            // Begin Transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                // Update password only if provided
                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = PasswordHelper.HashPassword(model.Password);
                }

                user.Update();
                _userRepository.Update(user);

                // Update user roles
                await _userRoleRepository.RemoveUserRolesAsync(model.Id, cancellationToken);

                // Add new roles
                if (model.SelectedRoles != null && model.SelectedRoles.Any())
                {
                    var userRoles = model.SelectedRoles.Select(roleId => new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId,
                        CreateDate = DateTime.UtcNow
                    }).ToList();

                    await _userRoleRepository.AddRangeAsync(userRoles, cancellationToken);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Commit Transaction
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Successfully edited user {UserId}", model.Id);

                return Result.Success("ò«—»— »« „Ê›ﬁ?  Ê?—«?‘ ‘œ");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing user {UserId}", model.Id);
            return Result.Failure("Œÿ« œ— Ê?—«?‘ ò«—»—", ex.Message);
        }
    }

    #endregion

    #region Delete User

    public async Task<Result> DeleteUser(ulong userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user {UserId}", userId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null || user.IsDelete)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                return Result.Failure("ò«—»— ?«›  ‰‘œ");
            }

            // Begin Transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                user.IsDelete = true;
                user.Update();
                _userRepository.Update(user);

                // Soft delete user roles
                var userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(userId, cancellationToken);

                foreach (var userRole in userRoles)
                {
                    userRole.IsDelete = true;
                    userRole.Update();
                }

                if (userRoles.Any())
                {
                    _userRoleRepository.UpdateRange(userRoles);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Commit Transaction
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Successfully deleted user {UserId}", userId);

                return Result.Success("ò«—»— »« „Ê›ﬁ?  Õ–› ‘œ");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", userId);
            return Result.Failure("Œÿ« œ— Õ–› ò«—»—", ex.Message);
        }
    }

    #endregion

    #region Get User Roles

    public async Task<UserRolesDto> GetUserRoles(ulong userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting roles for user {UserId}", userId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null || user.IsDelete)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                return null;
            }

            var allRoles = await _roleRepository.GetAllAsync(cancellationToken);
            var userRoleIds = await _userRoleRepository.GetRoleIdsByUserIdAsync(userId, cancellationToken);

            var userRoles = new UserRolesDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(r => new UserRoleItemDto
                {
                    RoleId = r.Id,
                    RoleTitle = r.Title,
                    IsSelected = userRoleIds.Contains(r.Id)
                }).ToList()
            };

            _logger.LogInformation("Successfully retrieved roles for user {UserId}", userId);

            return userRoles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles for user {UserId}", userId);
            throw;
        }
    }

    #endregion

    #region Get All Roles

    public async Task<List<Domain.Entities.Account.Role>> GetAllRoles(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all roles");

            var roles = await _roleRepository.GetAllAsync(cancellationToken);

            return roles.OrderBy(r => r.Title).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all roles");
            throw;
        }
    }

    #endregion
}
