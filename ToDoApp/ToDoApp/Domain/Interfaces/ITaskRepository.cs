using ToDoApp.Domain.Entities.Task;

namespace ToDoApp.Domain.Interfaces;

public interface ITaskRepository : IGenericRepository<TaskEntity>
{
    Task<List<TaskEntity>> GetUserTasksByDateRangeAsync(ulong userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<List<TaskEntity>> GetUserTasksByCategoryAsync(ulong userId, ulong categoryId, CancellationToken cancellationToken = default);
    Task<List<TaskEntity>> GetCompletedTasksAsync(ulong userId, CancellationToken cancellationToken = default);
    Task<List<TaskEntity>> GetPendingTasksAsync(ulong userId, CancellationToken cancellationToken = default);
}
