using ToDoApp.Domain.Entities.Task;

namespace ToDoApp.Domain.Model.Task;

public class WeeklyCalendarDto
{
    public DateTime WeekStartDate { get; set; } // شروع هفته (میلادی)
    public DateTime WeekEndDate { get; set; } // پایان هفته (میلادی)
    public List<TaskCalendarItemDto> Tasks { get; set; } = new();
}

public class TaskCalendarItemDto
{
    public ulong Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime TaskDateTime { get; set; } // میلادی
    public TaskPriority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public ulong? CategoryId { get; set; }
    public string? CategoryTitle { get; set; }
    public string? CategoryColor { get; set; }
}

