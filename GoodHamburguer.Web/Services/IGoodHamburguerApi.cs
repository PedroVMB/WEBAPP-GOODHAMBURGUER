using GoodHamburguer.Application.DTOs;
using Refit;

namespace GoodHamburguer.Web.Services;

public interface IGoodHamburguerApi
{
    [Get("/api/menu")]
    Task<IEnumerable<MenuItemResponse>> GetMenuAsync();

    [Get("/api/orders")]
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();

    [Get("/api/orders/{id}")]
    Task<OrderResponse> GetOrderByIdAsync(int id);

    [Post("/api/orders")]
    Task<ApiResponse<OrderResponse>> CreateOrderAsync([Body] CreateOrderRequest request);

    [Put("/api/orders/{id}")]
    Task<ApiResponse<OrderResponse>> UpdateOrderAsync(int id, [Body] UpdateOrderRequest request);

    [Delete("/api/orders/{id}")]
    Task<IApiResponse> DeleteOrderAsync(int id);
}

