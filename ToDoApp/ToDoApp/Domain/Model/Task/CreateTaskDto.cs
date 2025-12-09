using System.ComponentModel.DataAnnotations;
using ToDoApp.Domain.Entities.Task;

namespace ToDoApp.Domain.Model.Task;

public class CreateTaskDto
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string Title { get; set; }

    [Display(Name = "توضیحات")]
    [MaxLength(1000, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string? Description { get; set; }

    [Display(Name = "تاریخ و زمان")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public DateTime TaskDateTime { get; set; }

    [Display(Name = "اولویت")]
    [Required(ErrorMessage = "لطفا {0} را انتخاب کنید")]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    [Display(Name = "دسته‌بندی")]
    public ulong? CategoryId { get; set; }
}

public class EditTaskDto : CreateTaskDto
{
    public ulong Id { get; set; }
    public bool IsCompleted { get; set; }
}

