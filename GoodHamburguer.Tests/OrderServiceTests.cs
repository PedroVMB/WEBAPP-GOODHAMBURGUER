using GoodHamburguer.Application.DTOs;
using GoodHamburguer.Application.Services;
using GoodHamburguer.Infrastructure.Repositories;
using GoodHamburguer.Model;
using GoodHamburguer.Model.Enums;
using Moq;
using Xunit;

namespace GoodHamburguer.Tests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<IMenuRepository> _menuRepoMock = new();

    private static List<MenuItem> BuildMenuItems() =>
    [
        new MenuItem { Id = 1, Name = "X-Burger", Price = 5.00m, ItemType = 1, IsDisabled = false },
        new MenuItem { Id = 2, Name = "X-Egg", Price = 4.50m, ItemType = 1, IsDisabled = false },
        new MenuItem { Id = 3, Name = "X-Bacon", Price = 7.00m, ItemType = 1, IsDisabled = false },
        new MenuItem { Id = 4, Name = "French Fries", Price = 2.00m, ItemType = 2, IsDisabled = false },
        new MenuItem { Id = 5, Name = "Soda", Price = 2.50m, ItemType = 2, IsDisabled = false }
    ];

    private OrderService CreateService() => new(_orderRepoMock.Object, _menuRepoMock.Object);

    [Fact]
    public async Task CreateOrder_WithSandwichOnly_ReturnsOrderWithNoDiscount()
    {
        _menuRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(BuildMenuItems());
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => { o.Id = 1; return o; });

        var service = CreateService();
        var (result, error) = await service.CreateOrderAsync(new CreateOrderRequest { Sandwich = SandwichType.XBurger });

        Assert.Null(error);
        Assert.NotNull(result);
        Assert.Equal(0m, result.DiscountPercent);
        Assert.Equal(5.00m, result.Total);
    }

    [Fact]
    public async Task CreateOrder_WithSandwichAndFries_Applies10PercentDiscount()
    {
        _menuRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(BuildMenuItems());
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => { o.Id = 1; return o; });

        var service = CreateService();
        var (result, error) = await service.CreateOrderAsync(
            new CreateOrderRequest { Sandwich = SandwichType.XBurger, IncludeFries = true });

        Assert.Null(error);
        Assert.Equal(10m, result!.DiscountPercent);
        Assert.Equal(7.00m, result.Subtotal);
        Assert.Equal(6.30m, result.Total);
    }

    [Fact]
    public async Task CreateOrder_WithSandwichAndSoda_Applies15PercentDiscount()
    {
        _menuRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(BuildMenuItems());
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => { o.Id = 1; return o; });

        var service = CreateService();
        var (result, error) = await service.CreateOrderAsync(
            new CreateOrderRequest { Sandwich = SandwichType.XBurger, IncludeSoda = true });

        Assert.Null(error);
        Assert.Equal(15m, result!.DiscountPercent);
        Assert.Equal(7.50m, result.Subtotal);
        Assert.Equal(6.375m, result.Total);
    }

    [Fact]
    public async Task CreateOrder_WithAllItems_Applies20PercentDiscount()
    {
        _menuRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(BuildMenuItems());
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => { o.Id = 1; return o; });

        var service = CreateService();
        var (result, error) = await service.CreateOrderAsync(
            new CreateOrderRequest { Sandwich = SandwichType.XBurger, IncludeFries = true, IncludeSoda = true });

        Assert.Null(error);
        Assert.Equal(20m, result!.DiscountPercent);
        Assert.Equal(9.50m, result.Subtotal);
        Assert.Equal(7.60m, result.Total);
    }

    [Fact]
    public async Task CreateOrder_WithNoSandwich_ReturnsError()
    {
        var service = CreateService();
        var (result, error) = await service.CreateOrderAsync(new CreateOrderRequest { Sandwich = null });

        Assert.Null(result);
        Assert.NotNull(error);
        Assert.Contains(error.Errors, e => e.Contains("Sandwich is required"));
    }

    [Fact]
    public async Task UpdateOrder_NotFound_ReturnsError()
    {
        _orderRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Order?)null);

        var service = CreateService();
        var (result, error) = await service.UpdateOrderAsync(99, new UpdateOrderRequest { Sandwich = SandwichType.XBurger });

        Assert.Null(result);
        Assert.NotNull(error);
        Assert.Equal("Order not found", error.Message);
    }

    [Fact]
    public async Task DeleteOrder_NotFound_ReturnsFalse()
    {
        _orderRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Order?)null);

        var service = CreateService();
        var result = await service.DeleteOrderAsync(99);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteOrder_Found_ReturnsTrue()
    {
        var order = new Order { Id = 1, Sandwich = SandwichType.XBurger };
        _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
        _orderRepoMock.Setup(r => r.DeleteAsync(order)).Returns(Task.CompletedTask);

        var service = CreateService();
        var result = await service.DeleteOrderAsync(1);

        Assert.True(result);
    }
}

