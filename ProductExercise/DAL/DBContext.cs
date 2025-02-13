using Microsoft.EntityFrameworkCore;
using ProductExercise;

namespace DAL;

public class DBContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    public DBContext()
        : base()
    {
        
    }

    public DBContext(DbContextOptions options)
        : base(options)
    {
        
    }
}