using Microsoft.EntityFrameworkCore;

namespace Arcadia.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Define your DbSets here
    // public DbSet<YourEntity> YourEntities { get; set; }
}
