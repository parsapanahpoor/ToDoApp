namespace ToDoApp.Domain.Entities.Task;

public class Category : BaseEntities<ulong>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; } // برای نمایش رنگ در تقویم
}

