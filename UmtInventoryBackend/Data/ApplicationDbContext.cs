using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UmtInventoryBackend.Entities;

namespace UmtInventoryBackend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public DbSet<User> Users { get; set; }
    
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Item> Items { get; set; }
    
    public DbSet<StockRequest> StockRequests { get; set; }
 
    public DbSet<FinancialRequest> FinancialRequests { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Find all entity types that implement the IEntity interface
        var entityTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof(IEntity)));

        // Configure the Id property of each entity to be auto-generated
        foreach (var entityType in entityTypes)
            modelBuilder.Entity(entityType)
                .Property("Id")
                .ValueGeneratedOnAdd();
    }
}