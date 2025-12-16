using ToDoApp.Domain.Entities.Task;

namespace ToDoApp.Domain.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<bool> IsTitleExistsAsync(string title, ulong? excludeCategoryId = null, CancellationToken cancellationToken = default);
}
