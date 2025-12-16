using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Task;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infra;

namespace ToDoApp.Infra.Repositories;

public class TaskRepository : GenericRepository<TaskEntity>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<TaskEntity>> GetUserTasksByDateRangeAsync(ulong userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.UserId == userId && !t.IsDelete && t.TaskDateTime >= startDate && t.TaskDateTime <= endDate)
            .Include(t => t.Category)
            .OrderBy(t => t.TaskDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TaskEntity>> GetUserTasksByCategoryAsync(ulong userId, ulong categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.UserId == userId && t.CategoryId == categoryId && !t.IsDelete)
            .Include(t => t.Category)
            .OrderBy(t => t.TaskDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TaskEntity>> GetCompletedTasksAsync(ulong userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.UserId == userId && t.IsCompleted && !t.IsDelete)
            .Include(t => t.Category)
            .OrderByDescending(t => t.TaskDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TaskEntity>> GetPendingTasksAsync(ulong userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.UserId == userId && !t.IsCompleted && !t.IsDelete)
            .Include(t => t.Category)
            .OrderBy(t => t.TaskDateTime)
            .ToListAsync(cancellationToken);
    }
}
