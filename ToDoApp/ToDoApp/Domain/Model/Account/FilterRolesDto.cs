using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Model.Common;

namespace ToDoApp.Domain.Model.Account;

public class FilterRolesDto : BasePaging<Role>
{
    public string RoleTitle { get; set; }
}
