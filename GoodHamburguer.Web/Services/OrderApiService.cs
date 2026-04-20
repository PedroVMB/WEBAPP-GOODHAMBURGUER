using System.Text.Json;
using GoodHamburguer.Application.DTOs;

namespace GoodHamburguer.Web.Services;

public class OrderApiService : IOrderApiService
{
    private readonly IGoodHamburguerApi _api;

    public OrderApiService(IGoodHamburguerApi api)
    {
        _api = api;
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
    {
        return await _api.GetAllOrdersAsync();
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(int id)
    {
        return await _api.GetOrderByIdAsync(id);
    }

    public async Task<(OrderResponse? Result, ErrorResponse? Error)> CreateOrderAsync(CreateOrderRequest request)
    {
        var response = await _api.CreateOrderAsync(request);
        if (response.IsSuccessStatusCode)
            return (response.Content, null);

        var error = await ParseError(response.Error);
        return (null, error);
    }

    public async Task<(OrderResponse? Result, ErrorResponse? Error)> UpdateOrderAsync(int id, UpdateOrderRequest request)
    {
        var response = await _api.UpdateOrderAsync(id, request);
        if (response.IsSuccessStatusCode)
            return (response.Content, null);

        var error = await ParseError(response.Error);
        return (null, error);
    }

    public async Task<ErrorResponse?> DeleteOrderAsync(int id)
    {
        var response = await _api.DeleteOrderAsync(id);
        if (response.IsSuccessStatusCode)
            return null;

        return await ParseError(response.Error);
    }

    public async Task<IEnumerable<MenuItemResponse>> GetMenuAsync()
    {
        return await _api.GetMenuAsync();
    }

    private static async Task<ErrorResponse> ParseError(Refit.ApiException? ex)
    {
        if (ex is null)
            return new ErrorResponse { Message = "Erro desconhecido.", Errors = [] };

        try
        {
            var content = ex.Content;
            if (string.IsNullOrWhiteSpace(content))
                return new ErrorResponse { Message = ex.ReasonPhrase ?? "Erro.", Errors = [] };

            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            if (root.TryGetProperty("errors", out var errorsElement))
            {
                if (errorsElement.ValueKind == JsonValueKind.Array)
                {
                    var list = errorsElement.EnumerateArray()
                        .Select(e => e.GetString() ?? string.Empty)
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList();

                    var msg = root.TryGetProperty("message", out var m) ? m.GetString() ?? "Erro." : "Erro.";
                    return new ErrorResponse { Message = msg, Errors = list };
                }

                if (errorsElement.ValueKind == JsonValueKind.Object)
                {
                    var list = errorsElement.EnumerateObject()
                        .SelectMany(p => p.Value.EnumerateArray().Select(v => v.GetString() ?? string.Empty))
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList();

                    var title = root.TryGetProperty("title", out var t) ? t.GetString() ?? "Erro de validação." : "Erro de validação.";
                    return new ErrorResponse { Message = title, Errors = list };
                }
            }

            var fallbackMsg = root.TryGetProperty("message", out var fm) ? fm.GetString() ?? "Erro." : "Erro.";
            return new ErrorResponse { Message = fallbackMsg, Errors = [] };
        }
        catch
        {
            return new ErrorResponse { Message = "Erro ao processar resposta da API.", Errors = [] };
        }
    }
}
