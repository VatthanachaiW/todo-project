using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Models.Models;

namespace Todo.Models.Contexts
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Models.TodoTask>().ToTable("tb_todos");
      modelBuilder.Entity<Models.TodoTask>().HasKey(k => k.Id);
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.Id).ValueGeneratedOnAdd();
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.StartDate).HasColumnType("datetime2")
        .HasColumnName("start_date");
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.EndDate).HasColumnType("datetime2")
        .HasColumnName("end_date");
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.Description).HasColumnName("descriptions");
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.Status).HasColumnName("status");
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.IsActive).HasColumnName("is_active");
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.IsDelete).HasColumnName("is_delete");
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.CreatedBy).HasColumnName("created_by");
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.CreatedOn).HasColumnType("datetime2")
        .HasColumnName("created_on");
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.UpdatedBy).HasColumnName("updated_by").IsRequired(false);
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.UpdatedOn).HasColumnType("datetime2")
        .HasColumnName("updated_on").IsRequired(false);
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.DeletedBy).HasColumnName("deleted_by").IsRequired(false);
      modelBuilder.Entity<Models.TodoTask>().Property(p => p.DeletedOn).HasColumnType("datetime2")
        .HasColumnName("deleted_on").IsRequired(false);
    }
  }
}