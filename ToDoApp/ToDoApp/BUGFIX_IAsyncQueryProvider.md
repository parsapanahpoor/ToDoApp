# ?? Bug Fix: IAsyncQueryProvider Error

## ãÔ˜á:
```
InvalidOperationException: The provider for the source 'IQueryable' doesn't implement 'IAsyncQueryProvider'. 
Only providers that implement 'IAsyncQueryProvider' can be used for Entity Framework asynchronous operations.
```

## ÚáÊ:
ÏÑ ServiceåÇ ÇÒ `FindAsync()` ?Ç `GetAllAsync()` ÇÓÊİÇÏå ã?˜ÑÏ?ã ˜å `IEnumerable<T>` ÈÑã?ÑÏÇäÏ¡ ÓÓ Ñæ? Âä `AsQueryable()` ã?ÒÏ?ã. Ç?ä `IQueryable` Ï?Ñ Èå EF Core ãÊÕá äÈæÏ æ äã?ÊæÇäÓÊ Úãá?ÇÊ async ÇäÌÇã ÏåÏ.

## ÑÇå Íá:
ãÊÏ `GetQueryable()` Èå `IGenericRepository<T>` ÇÖÇİå ÔÏ ˜å ãÓÊŞ?ãÇğ `DbSet<T>` ÑÇ Èå ÕæÑÊ `IQueryable<T>` ÈÑã?ÑÏÇäÏ.

### ÊÛ??ÑÇÊ:

#### 1. IGenericRepository.cs
```csharp
public interface IGenericRepository<T> where T : class
{
    // ... existing methods
    IQueryable<T> GetQueryable(); // NEW
}
```

#### 2. GenericRepository.cs
```csharp
public virtual IQueryable<T> GetQueryable()
{
    return _dbSet.AsQueryable();
}
```

#### 3. ServiceåÇ (ŞÈá):
```csharp
// ? ÇÔÊÈÇå
var query = (await _userRepository.FindAsync(u => !u.IsDelete, cancellationToken))
    .OrderByDescending(p => p.CreateDate)
    .AsQueryable();

await filter.Paging(query); // Error!
```

#### 4. ServiceåÇ (ÈÚÏ):
```csharp
// ? ÏÑÓÊ
var query = _userRepository.GetQueryable()
    .OrderByDescending(p => p.CreateDate);

await filter.Paging(query); // Works!
```

## ãÒÇ?Ç:
- ? Úãá?ÇÊ async Ñæ? IQueryable ˜ÇÑ ã?˜äÏ
- ? Query FilteråÇ? EF Core Èå ÕæÑÊ ÎæÏ˜ÇÑ ÇÚãÇá ã?ÔæäÏ (`!IsDelete`)
- ? Lazy Execution - Query ÊÇ ÒãÇä `ToListAsync()` ÇÌÑÇ äã?ÔæÏ
- ? ÈåÊÑ ÈÑÇ? Paging - İŞØ Ñ˜æÑÏåÇ? ÕİÍå İÚá? ÇÒ DB ã?Â?äÏ

## İÇ?áåÇ? ÊÛ??Ñ ?ÇİÊå:
1. `Domain/Interfaces/IGenericRepository.cs`
2. `Infra/Repositories/GenericRepository.cs`
3. `Application/Services/Account/UserServiceNew.cs`
4. `Application/Services/Account/RoleServiceNew.cs`
5. `Application/Services/Task/CategoryServiceNew.cs`

## ÊÓÊ:
```bash
dotnet build
# Build successful ?
```

---

**ÊÇÑ?Î:** 2025-01-16
**Status:** ? Fixed
