using Microsoft.Extensions.Logging;
using ToDoApp.Application.Helpers;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Common;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Application.Services.Account;

public class AccountServiceNew : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountServiceNew> _logger;

    public AccountServiceNew(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<AccountServiceNew> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #region Register

    public async Task<Result<ulong>> Register(RegisterUserDto model, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new user with phone: {PhoneNumber}", model.PhoneNumber);

            // Validation: Check if phone number already exists
            if (await _userRepository.IsPhoneNumberExistsAsync(model.PhoneNumber, cancellationToken: cancellationToken))
            {
                _logger.LogWarning("Phone number {PhoneNumber} already exists", model.PhoneNumber);
                return Result<ulong>.Failure("À»  ‰«„ »« Œÿ« „Ê«ÃÂ ‘œ", "‘„«—Â  ·›‰ ﬁ»·« À»  ‘œÂ «” ");
            }

            // Hash password
            var hashedPassword = PasswordHelper.HashPassword(model.Password);

            var newUser = new UserEntity
            {
                CreateDate = DateTime.UtcNow,
                Email = null,
                IsDelete = false,
                Password = hashedPassword,
                PhoneNumber = model.PhoneNumber,
                UserName = model.PhoneNumber,
                UserAvatar = "default-avatar.png"
            };

            await _userRepository.AddAsync(newUser, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully registered user with phone: {PhoneNumber} and ID: {UserId}", 
                model.PhoneNumber, newUser.Id);

            return Result<ulong>.Success(newUser.Id, "À»  ‰«„ »« „Ê›ﬁ?  «‰Ã«„ ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with phone: {PhoneNumber}", model.PhoneNumber);
            return Result<ulong>.Failure("Œÿ« œ— À»  ‰«„", ex.Message);
        }
    }

    #endregion

    #region Login

    public async Task<Result<UserEntity>> Login(LoginUserDto model, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("User login attempt with phone: {PhoneNumber}", model.PhoneNumber);

            // Check if user exists
            var user = await _userRepository.GetByPhoneNumberAsync(model.PhoneNumber, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found with phone: {PhoneNumber}", model.PhoneNumber);
                return Result<UserEntity>.Failure("Ê—Êœ »« Œÿ« „Ê«ÃÂ ‘œ", "ò«—»—? »« «ÿ·«⁄«  Ê«—œ ‘œÂ ?«›  ‰‘œ");
            }

            // Verify password
            if (!PasswordHelper.VerifyPassword(model.Password, user.Password))
            {
                _logger.LogWarning("Login failed: Invalid password for user: {PhoneNumber}", model.PhoneNumber);
                return Result<UserEntity>.Failure("Ê—Êœ »« Œÿ« „Ê«ÃÂ ‘œ", "ò·„Â ⁄»Ê— «‘ »«Â «” ");
            }

            _logger.LogInformation("Successfully logged in user: {PhoneNumber}", model.PhoneNumber);

            return Result<UserEntity>.Success(user, "Ê—Êœ „Ê›ﬁ? ù¬„?“ »Êœ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for phone: {PhoneNumber}", model.PhoneNumber);
            return Result<UserEntity>.Failure("Œÿ« œ— Ê—Êœ", ex.Message);
        }
    }

    #endregion
}
