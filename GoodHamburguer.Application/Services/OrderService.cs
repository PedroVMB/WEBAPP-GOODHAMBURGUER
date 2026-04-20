using GoodHamburguer.Application.DTOs;
using GoodHamburguer.Infrastructure.Repositories;
using GoodHamburguer.Model;
using GoodHamburguer.Model.Enums;

namespace GoodHamburguer.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuRepository _menuRepository;

    public OrderService(IOrderRepository orderRepository, IMenuRepository menuRepository)
    {
        _orderRepository = orderRepository;
        _menuRepository = menuRepository;
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order is null ? null : MapToResponse(order);
    }

    public async Task<(OrderResponse? Result, ErrorResponse? Error)> CreateOrderAsync(CreateOrderRequest request)
    {
        if (request.Sandwich is null)
            return (null, new ErrorResponse { Message = "Validação falhou", Errors = ["O sanduíche é obrigatório."] });

        var menuItems = (await _menuRepository.GetAllAsync()).ToList();
        var (order, error) = BuildOrder(request.Sandwich.Value, request.IncludeFries, request.IncludeSoda, menuItems);

        if (error is not null)
            return (null, error);

        order!.CreatedAt = DateTime.UtcNow;
        var created = await _orderRepository.CreateAsync(order);
        return (MapToResponse(created), null);
    }

    public async Task<(OrderResponse? Result, ErrorResponse? Error)> UpdateOrderAsync(int id, UpdateOrderRequest request)
    {
        var existing = await _orderRepository.GetByIdAsync(id);
        if (existing is null)
            return (null, new ErrorResponse { Message = "Pedido não encontrado", Errors = [$"Nenhum pedido encontrado com o id {id}."] });

        if (request.Sandwich is null)
            return (null, new ErrorResponse { Message = "Validação falhou", Errors = ["O sanduíche é obrigatório."] });

        var menuItems = (await _menuRepository.GetAllAsync()).ToList();
        var (order, error) = BuildOrder(request.Sandwich.Value, request.IncludeFries, request.IncludeSoda, menuItems);

        if (error is not null)
            return (null, error);

        existing.Sandwich = order!.Sandwich;
        existing.Fries = order.Fries;
        existing.Soda = order.Soda;
        existing.Subtotal = order.Subtotal;
        existing.DiscountPercent = order.DiscountPercent;
        existing.DiscountAmount = order.DiscountAmount;
        existing.Total = order.Total;
        existing.UpdatedAt = DateTime.UtcNow;

        var updated = await _orderRepository.UpdateAsync(existing);
        return (MapToResponse(updated), null);
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return false;

        await _orderRepository.DeleteAsync(order);
        return true;
    }

    private static (Order? Order, ErrorResponse? Error) BuildOrder(
        SandwichType sandwich, bool includeFries, bool includeSoda, List<MenuItem> menuItems)
    {
        var sandwichItem = sandwich switch
        {
            SandwichType.XBurger => menuItems.FirstOrDefault(m => m.Id == 1),
            SandwichType.XEgg => menuItems.FirstOrDefault(m => m.Id == 2),
            SandwichType.XBacon => menuItems.FirstOrDefault(m => m.Id == 3),
            _ => null
        };

        if (sandwichItem is null)
            return (null, new ErrorResponse { Message = "Validação falhou", Errors = ["Tipo de sanduíche inválido."] });

        var subtotal = sandwichItem.Price;

        SideType? fries = null;
        if (includeFries)
        {
            var friesItem = menuItems.FirstOrDefault(m => m.Id == 4);
            if (friesItem is not null)
            {
                subtotal += friesItem.Price;
                fries = SideType.FrenchFries;
            }
        }

        SideType? soda = null;
        if (includeSoda)
        {
            var sodaItem = menuItems.FirstOrDefault(m => m.Id == 5);
            if (sodaItem is not null)
            {
                subtotal += sodaItem.Price;
                soda = SideType.Soda;
            }
        }

        var discountPercent = DiscountCalculator.Calculate(true, includeFries, includeSoda);
        var discountAmount = subtotal * discountPercent / 100;
        var total = subtotal - discountAmount;

        var order = new Order
        {
            Sandwich = sandwich,
            Fries = fries,
            Soda = soda,
            Subtotal = subtotal,
            DiscountPercent = discountPercent,
            DiscountAmount = discountAmount,
            Total = total
        };

        return (order, null);
    }

    private static OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            Sandwich = order.Sandwich switch
            {
                SandwichType.XBurger => "X-Burger",
                SandwichType.XEgg => "X-Egg",
                SandwichType.XBacon => "X-Bacon",
                _ => string.Empty
            },
            Fries = order.Fries == SideType.FrenchFries ? "French Fries" : null,
            Soda = order.Soda == SideType.Soda ? "Soda" : null,
            Subtotal = order.Subtotal,
            DiscountPercent = order.DiscountPercent,
            DiscountAmount = order.DiscountAmount,
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}
