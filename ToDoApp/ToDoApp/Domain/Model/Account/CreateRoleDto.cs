namespace ToDoApp.Domain.Model.Account;

public class CreateRoleDto
{
    public string Title { get; set; }
}

public class EditRoleDto : 
    CreateRoleDto
{
    public ulong Id { get; set; }
}
