using ToDoApp.Domain.Common;
using ToDoApp.Domain.Model.Task;

namespace ToDoApp.Application.Interfaces;

public interface ITaskService
{
    Task<WeeklyCalendarDto> GetWeeklyTasks(ulong userId, DateTime weekStartDate, CancellationToken cancellationToken = default);
    Task<Result<ulong>> CreateTask(CreateTaskDto model, ulong userId, CancellationToken cancellationToken = default);
    Task<Result<EditTaskDto>> GetTaskForEdit(ulong taskId, ulong userId, CancellationToken cancellationToken = default);
    Task<Result> EditTask(EditTaskDto model, ulong userId, CancellationToken cancellationToken = default);
    Task<Result> DeleteTask(ulong taskId, ulong userId, CancellationToken cancellationToken = default);
    Task<Result> ToggleTaskCompletion(ulong taskId, ulong userId, CancellationToken cancellationToken = default);
    Task<List<Domain.Entities.Task.Category>> GetAllCategories(CancellationToken cancellationToken = default);
}
