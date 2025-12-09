using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Model.Common;

namespace ToDoApp.Domain.Model.Account;

public class FilterUsersDto : BasePaging<UserEntity>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

