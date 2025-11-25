using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Account;

namespace ToDoApp.Infra;

public class ApplicationDbContext : DbContext
{
    #region Ctor

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options) { }

    #endregion

    #region Entity

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

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

        modelBuilder.Entity<Role>().HasQueryFilter(p=> !p.IsDelete);

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
