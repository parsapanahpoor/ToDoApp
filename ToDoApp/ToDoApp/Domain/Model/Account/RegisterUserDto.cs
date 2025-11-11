using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Domain.Model.Account;

public class RegisterUserDto
{
    [Required(ErrorMessage ="وارد کردن این فیلد اجباری است")]
    [MaxLength(12 , ErrorMessage ="طول پاراماتر ارسالی نباید بیشتر از 12 کاراکتر باشد.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "وارد کردن این فیلد اجباری است")]
    public string Password { get; set; }
}
