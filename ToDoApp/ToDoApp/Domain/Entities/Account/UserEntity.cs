namespace ToDoApp.Domain.Entities.Account;

public class User : BaseEntities<ulong>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string UserAvatar { get; set; }
}
