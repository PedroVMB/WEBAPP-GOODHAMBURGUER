using GoodHamburguer.Infrastructure.Data;
using GoodHamburguer.Infrastructure.Repositories;
using GoodHamburguer.Model;
using GoodHamburguer.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GoodHamburguer.Tests;

public class OrderRepositoryTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_PersistsOrder()
    {
        await using var ctx = CreateContext();
        var repo = new OrderRepository(ctx);

        var order = new Order { Sandwich = SandwichType.XBurger, Subtotal = 5m, Total = 5m, CreatedAt = DateTime.UtcNow };
        var created = await repo.CreateAsync(order);

        Assert.True(created.Id > 0);
        Assert.Equal(SandwichType.XBurger, created.Sandwich);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectOrder()
    {
        await using var ctx = CreateContext();
        var repo = new OrderRepository(ctx);

        var order = new Order { Sandwich = SandwichType.XEgg, Subtotal = 4.50m, Total = 4.50m, CreatedAt = DateTime.UtcNow };
        var created = await repo.CreateAsync(order);

        var found = await repo.GetByIdAsync(created.Id);

        Assert.NotNull(found);
        Assert.Equal(SandwichType.XEgg, found.Sandwich);
    }

    [Fact]
    public async Task GetAllAsync_ExcludesSoftDeletedOrders()
    {
        await using var ctx = CreateContext();
        var repo = new OrderRepository(ctx);

        var order1 = new Order { Sandwich = SandwichType.XBurger, CreatedAt = DateTime.UtcNow };
        var order2 = new Order { Sandwich = SandwichType.XBacon, CreatedAt = DateTime.UtcNow };
        await repo.CreateAsync(order1);
        await repo.CreateAsync(order2);
        await repo.DeleteAsync(order1);

        var all = (await repo.GetAllAsync()).ToList();

        Assert.Single(all);
        Assert.Equal(SandwichType.XBacon, all[0].Sandwich);
    }

    [Fact]
    public async Task DeleteAsync_SetsIsDisabledTrue()
    {
        await using var ctx = CreateContext();
        var repo = new OrderRepository(ctx);

        var order = new Order { Sandwich = SandwichType.XBurger, CreatedAt = DateTime.UtcNow };
        var created = await repo.CreateAsync(order);
        await repo.DeleteAsync(created);

        var found = await ctx.Orders.FindAsync(created.Id);
        Assert.NotNull(found);
        Assert.True(found.IsDisabled);
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        await using var ctx = CreateContext();
        var repo = new OrderRepository(ctx);

        var order = new Order { Sandwich = SandwichType.XBurger, Subtotal = 5m, Total = 5m, CreatedAt = DateTime.UtcNow };
        var created = await repo.CreateAsync(order);

        created.Sandwich = SandwichType.XBacon;
        created.Subtotal = 7m;
        created.Total = 7m;
        created.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(created);

        var updated = await repo.GetByIdAsync(created.Id);
        Assert.NotNull(updated);
        Assert.Equal(SandwichType.XBacon, updated.Sandwich);
        Assert.Equal(7m, updated.Total);
    }
}

