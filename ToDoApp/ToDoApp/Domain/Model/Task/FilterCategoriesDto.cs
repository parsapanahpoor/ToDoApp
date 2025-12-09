using ToDoApp.Domain.Entities.Task;
using ToDoApp.Domain.Model.Common;

namespace ToDoApp.Domain.Model.Task;

public class FilterCategoriesDto : BasePaging<Category>
{
    public string? Title { get; set; }
}

