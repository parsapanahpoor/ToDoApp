using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Task;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infra;

namespace ToDoApp.Infra.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Category> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Title == title && !c.IsDelete, cancellationToken);
    }

    public async Task<bool> IsTitleExistsAsync(string title, ulong? excludeCategoryId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(c => c.Title == title && !c.IsDelete);

        if (excludeCategoryId.HasValue)
            query = query.Where(c => c.Id != excludeCategoryId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}
