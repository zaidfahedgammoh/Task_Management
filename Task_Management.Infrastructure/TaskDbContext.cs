using Microsoft.EntityFrameworkCore;
using Task_Management.Domain;


namespace Task_Management.Infrastructure
{
    public class TaskDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.role)
                .HasConversion<string>();
        }
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
