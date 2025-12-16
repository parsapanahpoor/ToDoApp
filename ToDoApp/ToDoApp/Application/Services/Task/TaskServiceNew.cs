using Microsoft.Extensions.Logging;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Common;
using ToDoApp.Domain.Entities.Task;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Domain.Model.Task;

namespace ToDoApp.Application.Services.Task;

public class TaskServiceNew : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TaskServiceNew> _logger;

    public TaskServiceNew(
        ITaskRepository taskRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<TaskServiceNew> logger)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #region Get Weekly Tasks

    public async Task<WeeklyCalendarDto> GetWeeklyTasks(ulong userId, DateTime weekStartDate, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting weekly tasks for user {UserId} starting from {WeekStartDate}", userId, weekStartDate);

            var weekEndDate = weekStartDate.AddDays(6).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var tasks = await _taskRepository.GetUserTasksByDateRangeAsync(userId, weekStartDate.Date, weekEndDate, cancellationToken);

            var taskDtos = tasks.Select(t => new TaskCalendarItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                TaskDateTime = t.TaskDateTime,
                Priority = t.Priority,
                IsCompleted = t.IsCompleted,
                CategoryId = t.CategoryId,
                CategoryTitle = t.Category?.Title,
                CategoryColor = t.Category?.Color
            }).ToList();

            _logger.LogInformation("Retrieved {Count} tasks for user {UserId}", taskDtos.Count, userId);

            return new WeeklyCalendarDto
            {
                WeekStartDate = weekStartDate.Date,
                WeekEndDate = weekEndDate,
                Tasks = taskDtos
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting weekly tasks for user {UserId}", userId);
            throw;
        }
    }

    #endregion

    #region Create Task

    public async Task<Result<ulong>> CreateTask(CreateTaskDto model, ulong userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating task for user {UserId} with title: {Title}", userId, model.Title);

            // Validation: Check if title is provided
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                _logger.LogWarning("Task title is empty for user {UserId}", userId);
                return Result<ulong>.Failure("«?Ã«œ  ”ò »« Œÿ« „Ê«ÃÂ ‘œ", "⁄‰Ê«‰  ”ò «·“«„? «” ");
            }

            // Validation: Check if TaskDateTime is not in the past
            if (model.TaskDateTime < DateTime.UtcNow.AddHours(-24))
            {
                _logger.LogWarning("Task date is in the past for user {UserId}", userId);
                return Result<ulong>.Failure("«?Ã«œ  ”ò »« Œÿ« „Ê«ÃÂ ‘œ", " «—?Œ  ”ò ‰„?ù Ê«‰œ œ— ê–‘ Â »«‘œ");
            }

            // Validation: Check if category exists (if provided)
            if (model.CategoryId.HasValue)
            {
                var categoryExists = await _categoryRepository.GetByIdAsync(model.CategoryId.Value, cancellationToken);
                if (categoryExists == null)
                {
                    _logger.LogWarning("Category {CategoryId} not found for user {UserId}", model.CategoryId, userId);
                    return Result<ulong>.Failure("«?Ã«œ  ”ò »« Œÿ« „Ê«ÃÂ ‘œ", "œ” Âù»‰œ? «‰ Œ«» ‘œÂ „⁄ »— ‰?” ");
                }
            }

            var newTask = new TaskEntity
            {
                Title = model.Title,
                Description = model.Description,
                TaskDateTime = model.TaskDateTime,
                Priority = model.Priority,
                CategoryId = model.CategoryId,
                UserId = userId,
                IsCompleted = false,
                CreateDate = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(newTask, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully created task {TaskId} for user {UserId}", newTask.Id, userId);

            return Result<ulong>.Success(newTask.Id, " ”ò »« „Ê›ﬁ?  «?Ã«œ ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task for user {UserId}", userId);
            return Result<ulong>.Failure("Œÿ« œ— «?Ã«œ  ”ò", ex.Message);
        }
    }

    #endregion

    #region Edit Task

    public async Task<Result<EditTaskDto>> GetTaskForEdit(ulong taskId, ulong userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting task {TaskId} for edit by user {UserId}", taskId, userId);

            var task = await _taskRepository.FirstOrDefaultAsync(
                t => t.Id == taskId && t.UserId == userId && !t.IsDelete, 
                cancellationToken);

            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}", taskId, userId);
                return Result<EditTaskDto>.Failure(" ”ò ?«›  ‰‘œ");
            }

            var editDto = new EditTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                TaskDateTime = task.TaskDateTime,
                Priority = task.Priority,
                CategoryId = task.CategoryId,
                IsCompleted = task.IsCompleted
            };

            _logger.LogInformation("Successfully retrieved task {TaskId} for edit", taskId);

            return Result<EditTaskDto>.Success(editDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task {TaskId} for edit", taskId);
            return Result<EditTaskDto>.Failure("Œÿ« œ— œ—?«›  «ÿ·«⁄«   ”ò", ex.Message);
        }
    }

    public async Task<Result> EditTask(EditTaskDto model, ulong userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Editing task {TaskId} by user {UserId}", model.Id, userId);

            var task = await _taskRepository.FirstOrDefaultAsync(
                t => t.Id == model.Id && t.UserId == userId && !t.IsDelete, 
                cancellationToken);

            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}", model.Id, userId);
                return Result.Failure(" ”ò ?«›  ‰‘œ");
            }

            // Validation: Check if title is provided
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                _logger.LogWarning("Task title is empty for task {TaskId}", model.Id);
                return Result.Failure("Ê?—«?‘  ”ò »« Œÿ« „Ê«ÃÂ ‘œ", "⁄‰Ê«‰  ”ò «·“«„? «” ");
            }

            // Validation: Check if category exists (if provided)
            if (model.CategoryId.HasValue)
            {
                var categoryExists = await _categoryRepository.GetByIdAsync(model.CategoryId.Value, cancellationToken);
                if (categoryExists == null)
                {
                    _logger.LogWarning("Category {CategoryId} not found for task {TaskId}", model.CategoryId, model.Id);
                    return Result.Failure("Ê?—«?‘  ”ò »« Œÿ« „Ê«ÃÂ ‘œ", "œ” Âù»‰œ? «‰ Œ«» ‘œÂ „⁄ »— ‰?” ");
                }
            }

            task.Title = model.Title;
            task.Description = model.Description;
            task.TaskDateTime = model.TaskDateTime;
            task.Priority = model.Priority;
            task.CategoryId = model.CategoryId;
            task.IsCompleted = model.IsCompleted;
            task.Update();

            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully edited task {TaskId}", model.Id);

            return Result.Success(" ”ò »« „Ê›ﬁ?  Ê?—«?‘ ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing task {TaskId}", model.Id);
            return Result.Failure("Œÿ« œ— Ê?—«?‘  ”ò", ex.Message);
        }
    }

    #endregion

    #region Delete Task

    public async Task<Result> DeleteTask(ulong taskId, ulong userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting task {TaskId} by user {UserId}", taskId, userId);

            var task = await _taskRepository.FirstOrDefaultAsync(
                t => t.Id == taskId && t.UserId == userId && !t.IsDelete, 
                cancellationToken);

            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}", taskId, userId);
                return Result.Failure(" ”ò ?«›  ‰‘œ");
            }

            task.IsDelete = true;
            task.Update();

            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted task {TaskId}", taskId);

            return Result.Success(" ”ò »« „Ê›ﬁ?  Õ–› ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {TaskId}", taskId);
            return Result.Failure("Œÿ« œ— Õ–›  ”ò", ex.Message);
        }
    }

    #endregion

    #region Toggle Task Completion

    public async Task<Result> ToggleTaskCompletion(ulong taskId, ulong userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Toggling completion for task {TaskId} by user {UserId}", taskId, userId);

            var task = await _taskRepository.FirstOrDefaultAsync(
                t => t.Id == taskId && t.UserId == userId && !t.IsDelete, 
                cancellationToken);

            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}", taskId, userId);
                return Result.Failure(" ”ò ?«›  ‰‘œ");
            }

            task.IsCompleted = !task.IsCompleted;
            task.Update();

            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully toggled completion for task {TaskId} to {IsCompleted}", taskId, task.IsCompleted);

            return Result.Success(task.IsCompleted ? " ”ò »Â ⁄‰Ê«‰ «‰Ã«„ ‘œÂ ⁄·«„ ùê–«—? ‘œ" : " ”ò »Â ⁄‰Ê«‰ «‰Ã«„ ‰‘œÂ ⁄·«„ ùê–«—? ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling completion for task {TaskId}", taskId);
            return Result.Failure("Œÿ« œ—  €??— Ê÷⁄?   ”ò", ex.Message);
        }
    }

    #endregion

    #region Get All Categories

    public async Task<List<Domain.Entities.Task.Category>> GetAllCategories(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all categories");

            var categories = await _categoryRepository.GetAllAsync(cancellationToken);

            return categories.OrderBy(c => c.Title).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all categories");
            throw;
        }
    }

    #endregion
}
