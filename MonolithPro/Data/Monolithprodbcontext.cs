using Microsoft.EntityFrameworkCore;
using MonolithPro.Modules.Users;
using MonolithPro.Modules.Orders;
using MonolithPro.Modules.Inventory;

namespace MonolithPro.Data;

public class MonolithProDbContext : DbContext
{
    public MonolithProDbContext(DbContextOptions<MonolithProDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configuración de Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Stock).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
        });

        // Configuración de Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.ProductId).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // Seed inicial - Users
        modelBuilder.Entity<User>().HasData(
            new User(1, "Juan Pérez", "juan@example.com"),
            new User(2, "María García", "maria@example.com"),
            new User(3, "Carlos López", "carlos@example.com")
        );

        // Seed inicial - Products
        modelBuilder.Entity<Product>().HasData(
            new Product(1, "Laptop HP", 50, 899.99m),
            new Product(2, "Mouse Logitech", 150, 29.99m),
            new Product(3, "Teclado Mecánico", 80, 79.99m),
            new Product(4, "Monitor Samsung 24\"", 30, 199.99m),
            new Product(5, "Webcam HD", 100, 49.99m)
        );
    }
}