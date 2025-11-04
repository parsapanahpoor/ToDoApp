using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Account;

namespace ToDoApp.Infra;

public class AppplicationDbContext : DbContext
{
    #region Ctor

    public AppplicationDbContext(DbContextOptions<AppplicationDbContext> options)
           : base(options) { }

    #endregion

    #region Entity

    public DbSet<UserEntity> Users { get; set; }

    #endregion

    #region OnConfiguring

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
