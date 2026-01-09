using Microsoft.EntityFrameworkCore;
using Employees_WebAPI.Model;

namespace Employees_WebAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
}
