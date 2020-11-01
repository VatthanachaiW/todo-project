using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Todo.API.Connections
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        protected readonly DbContextOptions<ApplicationDbContext> ContextOptions;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) :
            base(options)
        {
            ContextOptions = options;
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Todo>().ToTable("tb_todos");
            modelBuilder.Entity<Models.Todo>().HasKey(k => k.Id);
            modelBuilder.Entity<Models.Todo>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Models.Todo>().Property(p => p.StartDate).HasColumnType("datetime2")
                .HasColumnName("start_date");
            modelBuilder.Entity<Models.Todo>().Property(p => p.EndDate).HasColumnType("datetime2")
                .HasColumnName("end_date");
            modelBuilder.Entity<Models.Todo>().Property(p => p.Description).HasColumnName("descriptions");
            modelBuilder.Entity<Models.Todo>().Property(p => p.Status).HasColumnName("status");
            modelBuilder.Entity<Models.Todo>().Property(p => p.IsActive).HasColumnName("is_active");
            modelBuilder.Entity<Models.Todo>().Property(p => p.IsDelete).HasColumnName("is_delete");
            modelBuilder.Entity<Models.Todo>().Property(p => p.CreatedBy).HasColumnName("created_by");
            modelBuilder.Entity<Models.Todo>().Property(p => p.CreatedOn).HasColumnType("datetime2")
                .HasColumnName("created_on");
            modelBuilder.Entity<Models.Todo>().Property(p => p.UpdatedBy).HasColumnName("updated_by").IsRequired(false);
            modelBuilder.Entity<Models.Todo>().Property(p => p.UpdatedOn).HasColumnType("datetime2")
                .HasColumnName("updated_on").IsRequired(false);
            modelBuilder.Entity<Models.Todo>().Property(p => p.DeletedBy).HasColumnName("deleted_by").IsRequired(false);
            modelBuilder.Entity<Models.Todo>().Property(p => p.DeletedOn).HasColumnType("datetime2")
                .HasColumnName("deleted_on").IsRequired(false);
        }
    }
}