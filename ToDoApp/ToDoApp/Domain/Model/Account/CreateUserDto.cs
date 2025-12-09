using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Domain.Model.Account;

public class CreateUserDto
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string UserName { get; set; }

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
    public string Email { get; set; }

    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(20, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string PhoneNumber { get; set; }

    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string Password { get; set; }

    [Display(Name = "نقش‌های کاربر")]
    public List<ulong> SelectedRoles { get; set; } = new();
}

