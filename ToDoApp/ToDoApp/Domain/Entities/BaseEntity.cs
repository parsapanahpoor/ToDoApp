namespace ToDoApp.Domain.Entities;

public abstract class BaseEntities<TKey> where TKey :  struct , IComparable<TKey>
{
    public TKey Id { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public bool IsDelete { get; set; }

    public DateTime UpdateDate { get; set; }

    public void Update() => UpdateDate = DateTime.UtcNow;
}