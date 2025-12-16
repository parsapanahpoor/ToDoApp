using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Entities.Task;

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

    public DbSet<Category> Categories { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }

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

        // Query Filters for Soft Delete
        modelBuilder.Entity<UserEntity>().HasQueryFilter(p=> !p.IsDelete);
        modelBuilder.Entity<Role>().HasQueryFilter(p=> !p.IsDelete);
        modelBuilder.Entity<UserRole>().HasQueryFilter(p=> !p.IsDelete);
        modelBuilder.Entity<Category>().HasQueryFilter(p=> !p.IsDelete);
        modelBuilder.Entity<TaskEntity>().HasQueryFilter(p=> !p.IsDelete);

        // Configure TaskEntity relationships
        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes for better performance
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email);
        
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.PhoneNumber);
        
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.UserName);

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
