# ?? ToDoApp - Architecture Improvements Documentation

## ?? Œ·«’Â  €??—« 

«?‰ Å—ÊéÂ »« „⁄„«—? Ãœ?œ Ê »Â —?‰ ‘?ÊÂùÂ«? (Best Practices) »—‰«„Âù‰Ê?”? »«“‰Ê?”? ‘œÂ «” .

---

## ?  €??—«  «‰Ã«„ ‘œÂ

### 1?? **„⁄„«—? Repository Pattern**

#### InterfaceùÂ«:
- `IGenericRepository<T>` - Generic repository »—«? ⁄„·?«  CRUD Å«?Â
- `IUnitOfWork` - „œ?—?  Transaction Ê SaveChanges
- `IUserRepository`, `IRoleRepository`, `IUserRoleRepository`, `ITaskRepository`, `ICategoryRepository`

#### ImplementationùÂ«:
- `GenericRepository<T>` - Å?«œÂù”«“? Generic
- `UnitOfWork` - „œ?—?  Transaction
- RepositoryùÂ«? „Œ’Ê’ Â— Entity

**„“«?«:**
- ? Ãœ«”«“? Data Access «“ Business Logic
- ? ﬁ«»·?   ” ùÅ–?—? »«·«
- ? ò«Â‘  ò—«— òœ
- ? «„ò«‰  €??— ”«œÂ Database Provider

---

### 2?? **Service Layer »« Interface**

#### InterfaceùÂ«:
- `IUserService` - „œ?—?  ò«—»—«‰
- `IRoleService` - „œ?—?  ‰ﬁ‘ùÂ«
- `IAccountService` - À» ù‰«„ Ê Ê—Êœ
- `ITaskService` - „œ?—?   ”òùÂ«
- `ICategoryService` - „œ?—?  œ” Âù»‰œ?ùÂ«

#### ImplementationùÂ«:
- `UserServiceNew`
- `RoleServiceNew`
- `AccountServiceNew`
- `TaskServiceNew`
- `CategoryServiceNew`

**„“«?«:**
- ? Dependency Inversion Principle
- ? ﬁ«»·?  Mock ò—œ‰ »—«? Unit Test
- ? òœ  „?“ Ê ﬁ«»· ‰êÂœ«—?

---

### 3?? **Result Pattern »—«? „œ?—?  Œÿ«**

```csharp
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}
```

**„“«?«:**
- ? „œ?—?  Õ—›Âù«? Œÿ«Â«
- ? Å?«„ùÂ«? Ê«÷Õ »—«? ò«—»—
- ? ⁄œ„ «” ›«œÂ «“ Exception »—«? Flow Control

**„À«·:**
```csharp
var result = await _userService.CreateUser(model);
if (result.IsSuccess)
{
    // Success
    var userId = result.Data;
}
else
{
    // Error
    var errorMessage = result.Message;
    var errors = result.Errors;
}
```

---

### 4?? **«„‰?  —„“ ⁄»Ê— »« BCrypt**

#### ﬁ»·:
```csharp
// SHA256 »œÊ‰ Salt - ‰««„‰!
using var sha256 = SHA256.Create();
var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
```

#### »⁄œ:
```csharp
// BCrypt »« Salt - «„‰
public static string HashPassword(string password)
{
    return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
}

public static bool VerifyPassword(string password, string hashedPassword)
{
    return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}
```

**„“«?«:**
- ? «„‰?  »«·« »« Salt ŒÊœò«—
- ? „ﬁ«Ê„  œ— »—«»— Rainbow Table Attack
- ? ﬁ«»·  ‰Ÿ?„ »Êœ‰ ”Œ ? (Work Factor)

---

### 5?? **Logging »« Serilog**

```csharp
_logger.LogInformation("Creating user {UserName}", model.UserName);
_logger.LogWarning("Username {UserName} already exists", model.UserName);
_logger.LogError(ex, "Error creating user {UserId}", userId);
```

** ‰Ÿ?„« :**
- ?? Log Files œ— ÅÊ‘Â `Logs/`
- ?? Rolling Daily
- ?? ‰êÂœ«—? 30 —Ê“ ê–‘ Â
- ??? ‰„«?‘ œ— Console

---

### 6?? **Transaction Management**

```csharp
await _unitOfWork.BeginTransactionAsync(cancellationToken);
try
{
    // ⁄„·?«  1
    await _userRepository.AddAsync(newUser);
    await _unitOfWork.SaveChangesAsync();

    // ⁄„·?«  2
    await _userRoleRepository.AddRangeAsync(userRoles);
    await _unitOfWork.SaveChangesAsync();

    await _unitOfWork.CommitTransactionAsync();
}
catch
{
    await _unitOfWork.RollbackTransactionAsync();
    throw;
}
```

**„“«?«:**
- ? « „? »Êœ‰ ⁄„·?«  (Atomic Operations)
- ? Data Consistency
- ? «„ò«‰ Rollback œ— ’Ê—  Œÿ«

---

### 7?? **Query Filters »—«? Soft Delete**

```csharp
modelBuilder.Entity<UserEntity>().HasQueryFilter(p => !p.IsDelete);
modelBuilder.Entity<Role>().HasQueryFilter(p => !p.IsDelete);
modelBuilder.Entity<UserRole>().HasQueryFilter(p => !p.IsDelete);
modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsDelete);
modelBuilder.Entity<TaskEntity>().HasQueryFilter(p => !p.IsDelete);
```

**„“«?«:**
- ? ŒÊœò«— ›?· — ‘œ‰ —òÊ—œÂ«? Õ–› ‘œÂ
- ? ò«Â‘ Œÿ«? «‰”«‰?
- ? òœ  „?“ —

---

### 8?? **Database Indexes**

```csharp
modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email);
modelBuilder.Entity<UserEntity>().HasIndex(u => u.PhoneNumber);
modelBuilder.Entity<UserEntity>().HasIndex(u => u.UserName);
```

**„“«?«:**
- ? «›“«?‘ ”—⁄  QueryùÂ«
- ? »Â»Êœ Performance

---

### 9?? **Authorization ò«„·**

```csharp
public class CheckUserHasAnyRole : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(...)
    {
        var userId = context.HttpContext.User.GetUserId();
        if (userId == 0)
        {
            context.Result = new RedirectResult("/Account/Login");
            return;
        }

        var hasUserAnyRole = await _userRoleRepository.HasUserAnyRoleAsync(userId);
        if (!hasUserAnyRole)
        {
            context.Result = new RedirectResult("/");
            return;
        }

        await next();
    }
}
```

**„“«?«:**
- ? «„‰?  Admin Area
- ? Ã·Êê?—? «“ œ” —”? €?—„Ã«“

---

### ?? **AutoMapper**

```csharp
CreateMap<UserEntity, EditUserDto>();
CreateMap<CreateUserDto, UserEntity>();
```

**„“«?«:**
- ? ò«Â‘ Boilerplate Code
- ? Mapping ŒÊœò«—
- ? òœ  „?“ —

---

### 1??1?? **«” ›«œÂ ?òÅ«—çÂ «“ UTC**

#### ﬁ»·:
```csharp
public DateTime CreateDate { get; set; } = DateTime.Now; // Local Time
public void Update() => UpdateDate = DateTime.UtcNow; // UTC
```

#### »⁄œ:
```csharp
public DateTime CreateDate { get; set; } = DateTime.UtcNow; // UTC
public void Update() => UpdateDate = DateTime.UtcNow; // UTC
```

**„“«?«:**
- ? ”«“ê«—? »« TimeZoneùÂ«? „Œ ·›
- ? ⁄œ„  œ«Œ· »« Daylight Saving Time

---

### 1??2?? **CancellationToken Support**

```csharp
public async Task<Result> CreateUser(CreateUserDto model, CancellationToken cancellationToken = default)
{
    await _userRepository.AddAsync(newUser, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
}
```

**„“«?«:**
- ? ﬁ«»·?  Cancel ò—œ‰ ⁄„·?«  ÿÊ·«‰?
- ? »Â»Êœ Performance œ— ASP.NET Core

---

## ?? „ﬁ«?”Â ﬁ»· Ê »⁄œ

| Ê?éê? | ﬁ»· | »⁄œ |
|-------|-----|-----|
| **«„‰?  —„“ ⁄»Ê—** | ? SHA256 »œÊ‰ Salt | ? BCrypt »« Salt |
| **Authorization Admin** | ? €?—›⁄«· | ? ›⁄«· Ê ò«„· |
| **„⁄„«—?** | ? Service „” ﬁ?„ »« DbContext | ? Repository Pattern + UoW |
| **Error Handling** | ? Boolean Return | ? Result Pattern |
| **Logging** | ? ‰œ«—œ | ? Serilog |
| **Transaction** | ? ‰œ«—œ | ? œ«—œ |
| **Query Filter** | ?? ‰?„Âùò«„· | ? ò«„· »—«? Â„Â |
| **Indexes** | ? ‰œ«—œ | ? œ«—œ |
| **Interface** | ? ‰œ«—œ | ?  „«„ ServiceùÂ« |
| **Unit Test** | ? ”Œ  | ? ¬”«‰ |

---

## ?? ‰ÕÊÂ «” ›«œÂ

### 1. «” ›«œÂ «“ ServiceùÂ« œ— Controller:

```csharp
public class UserController : AdminBaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> CreateUser(CreateUserDto model, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUser(model, cancellationToken);
        
        if (result.IsSuccess)
        {
            TempData[SuccessMessage] = result.Message;
            return RedirectToAction(nameof(FilterUsers));
        }

        TempData[WarningMessage] = result.Message;
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error);
        }
        
        return View(model);
    }
}
```

---

## ?? Åò?ÃùÂ«? Ãœ?œ

```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="Serilog.AspNetCore" Version="10.0.0" />
```

---

## ??? ”«Œ «— ÅÊ‘ÂùÂ«

```
ToDoApp/
??? Application/
?   ??? Extensions/
?   ?   ??? ClaimsExtensions.cs
?   ?   ??? ControllerExtensions.cs
?   ??? Helpers/
?   ?   ??? DateHelper.cs
?   ?   ??? PasswordHelper.cs
?   ??? Interfaces/
?   ?   ??? IUserService.cs
?   ?   ??? IRoleService.cs
?   ?   ??? IAccountService.cs
?   ?   ??? ITaskService.cs
?   ?   ??? ICategoryService.cs
?   ??? Mappings/
?   ?   ??? UserProfile.cs
?   ?   ??? RoleProfile.cs
?   ??? Middleware/
?   ?   ??? LanguageMiddleware.cs
?   ??? Services/
?       ??? Account/
?       ?   ??? UserServiceNew.cs
?       ?   ??? RoleServiceNew.cs
?       ?   ??? AccountServiceNew.cs
?       ??? Task/
?           ??? TaskServiceNew.cs
?           ??? CategoryServiceNew.cs
??? Domain/
?   ??? Common/
?   ?   ??? Result.cs
?   ??? Entities/
?   ??? Interfaces/
?   ?   ??? IGenericRepository.cs
?   ?   ??? IUnitOfWork.cs
?   ?   ??? IUserRepository.cs
?   ?   ??? IRoleRepository.cs
?   ?   ??? IUserRoleRepository.cs
?   ?   ??? ITaskRepository.cs
?   ?   ??? ICategoryRepository.cs
?   ??? Model/
??? Infra/
?   ??? Repositories/
?   ?   ??? GenericRepository.cs
?   ?   ??? UnitOfWork.cs
?   ?   ??? UserRepository.cs
?   ?   ??? RoleRepository.cs
?   ?   ??? UserRoleRepository.cs
?   ?   ??? TaskRepository.cs
?   ?   ??? CategoryRepository.cs
?   ??? ApplicationDbContext.cs
??? Logs/ (Auto-generated)
```

---

## ??  ‰Ÿ?„«  «÷«›?

### appsettings.json:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    }
  }
}
```

---

## ?? „À«· Unit Test

```csharp
[Fact]
public async Task CreateUser_WithValidData_ShouldReturnSuccess()
{
    // Arrange
    var mockUserRepo = new Mock<IUserRepository>();
    var mockUoW = new Mock<IUnitOfWork>();
    var mockLogger = new Mock<ILogger<UserServiceNew>>();
    
    var service = new UserServiceNew(mockUserRepo.Object, ..., mockUoW.Object, mockLogger.Object);
    
    var dto = new CreateUserDto { ... };

    // Act
    var result = await service.CreateUser(dto);

    // Assert
    Assert.True(result.IsSuccess);
}
```

---

## ?? „‰«»⁄ „›?œ

- [Repository Pattern](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)
- [Unit of Work](https://www.c-sharpcorner.com/article/unit-of-work-in-repository-pattern/)
- [Result Pattern](https://enterprisecraftsmanship.com/posts/error-handling-exception-or-result/)
- [BCrypt](https://github.com/BcryptNet/bcrypt.net)
- [Serilog](https://serilog.net/)
- [AutoMapper](https://docs.automapper.org/)

---

## ??û??  ?„  Ê”⁄Â

«?‰ »Â»ÊœÂ«  Ê”ÿ GitHub Copilot »« Â„ò«—? ‘„« «‰Ã«„ ‘œ! ??

** «—?Œ:** 2025-01-16

---

## ?? Å‘ ?»«‰?

»—«? ”Ê«·«  Ê „‘ò·« ° ·ÿ›« ?ò Issue œ— GitHub «?Ã«œ ò‰?œ.

---

**„Ê›ﬁ »«‘?œ! ??**
