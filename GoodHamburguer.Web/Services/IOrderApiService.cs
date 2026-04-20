using GoodHamburguer.Application.DTOs;

namespace GoodHamburguer.Web.Services;

public interface IOrderApiService
{
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
    Task<OrderResponse?> GetOrderByIdAsync(int id);
    Task<(OrderResponse? Result, ErrorResponse? Error)> CreateOrderAsync(CreateOrderRequest request);
    Task<(OrderResponse? Result, ErrorResponse? Error)> UpdateOrderAsync(int id, UpdateOrderRequest request);
    Task<ErrorResponse?> DeleteOrderAsync(int id);
    Task<IEnumerable<MenuItemResponse>> GetMenuAsync();
}

