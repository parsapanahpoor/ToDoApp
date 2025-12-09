using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoApp.Domain.Entities.Task;
using ToDoApp.Domain.Model.Task;
using ToDoApp.Infra;

namespace ToDoApp.Application.Services.Task;

public class TaskService(ApplicationDbContext context)
{
    public async Task<WeeklyCalendarDto> GetWeeklyTasks(ulong userId, DateTime weekStartDate)
    {
        // محاسبه شروع و پایان هفته
        var weekEndDate = weekStartDate.AddDays(6).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        var tasks = await context.Tasks
            .Where(t => t.UserId == userId 
                && !t.IsDelete 
                && t.TaskDateTime >= weekStartDate.Date 
                && t.TaskDateTime <= weekEndDate)
            .Include(t => t.Category)
            .OrderBy(t => t.TaskDateTime)
            .ToListAsync();

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

        return new WeeklyCalendarDto
        {
            WeekStartDate = weekStartDate.Date,
            WeekEndDate = weekEndDate,
            Tasks = taskDtos
        };
    }

    public async Task<bool> CreateTask(CreateTaskDto model, ulong userId)
    {
        if (string.IsNullOrEmpty(model.Title))
            return false;

        var newTask = new TaskEntity
        {
            Title = model.Title,
            Description = model.Description,
            TaskDateTime = model.TaskDateTime,
            Priority = model.Priority,
            CategoryId = model.CategoryId,
            UserId = userId,
            IsCompleted = false
        };

        await context.Tasks.AddAsync(newTask);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<EditTaskDto> FillEditTaskDto(ulong taskId, ulong userId)
    {
        var task = await context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId && !t.IsDelete);

        if (task == null)
            return null;

        return new EditTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            TaskDateTime = task.TaskDateTime,
            Priority = task.Priority,
            CategoryId = task.CategoryId,
            IsCompleted = task.IsCompleted
        };
    }

    public async Task<bool> EditTask(EditTaskDto model, ulong userId)
    {
        var task = await context.Tasks
            .FirstOrDefaultAsync(t => t.Id == model.Id && t.UserId == userId && !t.IsDelete);

        if (task == null)
            return false;

        task.Title = model.Title;
        task.Description = model.Description;
        task.TaskDateTime = model.TaskDateTime;
        task.Priority = model.Priority;
        task.CategoryId = model.CategoryId;
        task.IsCompleted = model.IsCompleted;
        task.Update();

        context.Tasks.Update(task);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTask(ulong taskId, ulong userId)
    {
        var task = await context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId && !t.IsDelete);

        if (task == null)
            return false;

        task.IsDelete = true;
        task.Update();

        context.Tasks.Update(task);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ToggleTaskCompletion(ulong taskId, ulong userId)
    {
        var task = await context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId && !t.IsDelete);

        if (task == null)
            return false;

        task.IsCompleted = !task.IsCompleted;
        task.Update();

        context.Tasks.Update(task);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Domain.Entities.Task.Category>> GetAllCategories()
    {
        return await context.Categories
            .Where(c => !c.IsDelete)
            .OrderBy(c => c.Title)
            .ToListAsync();
    }
}

