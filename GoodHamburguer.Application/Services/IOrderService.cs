using GoodHamburguer.Application.DTOs;

namespace GoodHamburguer.Application.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
    Task<OrderResponse?> GetOrderByIdAsync(int id);
    Task<(OrderResponse? Result, ErrorResponse? Error)> CreateOrderAsync(CreateOrderRequest request);
    Task<(OrderResponse? Result, ErrorResponse? Error)> UpdateOrderAsync(int id, UpdateOrderRequest request);
    Task<bool> DeleteOrderAsync(int id);
}

