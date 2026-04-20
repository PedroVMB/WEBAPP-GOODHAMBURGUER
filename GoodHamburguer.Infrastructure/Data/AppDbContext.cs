using GoodHamburguer.Model;
using GoodHamburguer.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "X-Burger", Price = 5.00m, ItemType = 1, IsDisabled = false },
            new MenuItem { Id = 2, Name = "X-Egg", Price = 4.50m, ItemType = 1, IsDisabled = false },
            new MenuItem { Id = 3, Name = "X-Bacon", Price = 7.00m, ItemType = 1, IsDisabled = false },
            new MenuItem { Id = 4, Name = "French Fries", Price = 2.00m, ItemType = 2, IsDisabled = false },
            new MenuItem { Id = 5, Name = "Soda", Price = 2.50m, ItemType = 2, IsDisabled = false }
        );
    }
}

