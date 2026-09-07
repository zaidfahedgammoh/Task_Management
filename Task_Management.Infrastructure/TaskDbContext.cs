using Microsoft.EntityFrameworkCore;
using Task_Management.Domain;


namespace Task_Management_Infrastructure
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>(); 
    }
}
