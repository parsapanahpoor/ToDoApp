namespace ToDoApp.Domain.Entities.Account;

public class UserRole : BaseEntities<ulong>
{
    public ulong UserId { get; set; }
    public ulong RoleId { get; set; }
}
