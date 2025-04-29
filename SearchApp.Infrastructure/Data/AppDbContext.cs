using Microsoft.EntityFrameworkCore;
using SearchApp.Domain;

namespace SearchApp.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
    }
}
