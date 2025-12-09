namespace ToDoApp.Domain.Model.Account;

public class UserRolesDto
{
    public ulong UserId { get; set; }
    public string UserName { get; set; }
    public List<UserRoleItemDto> Roles { get; set; } = new();
}

public class UserRoleItemDto
{
    public ulong RoleId { get; set; }
    public string RoleTitle { get; set; }
    public bool IsSelected { get; set; }
}

