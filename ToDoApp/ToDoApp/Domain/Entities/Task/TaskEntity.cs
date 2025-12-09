namespace ToDoApp.Domain.Entities.Task;

public class TaskEntity : BaseEntities<ulong>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime TaskDateTime { get; set; } // تاریخ و زمان انجام Task (میلادی)
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public bool IsCompleted { get; set; } = false;
    
    // Foreign Keys
    public ulong UserId { get; set; }
    public ulong? CategoryId { get; set; }
    
    // Navigation Properties
    public Domain.Entities.Account.UserEntity User { get; set; }
    public Category? Category { get; set; }
}

public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

