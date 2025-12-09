using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Domain.Model.Task;

public class CreateCategoryDto
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string Title { get; set; }

    [Display(Name = "توضیحات")]
    [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string? Description { get; set; }

    [Display(Name = "رنگ")]
    [MaxLength(20, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string? Color { get; set; } // Hex color code مثل #FF5733
}

public class EditCategoryDto : CreateCategoryDto
{
    public ulong Id { get; set; }
}

