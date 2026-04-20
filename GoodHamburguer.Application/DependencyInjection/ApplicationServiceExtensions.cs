using GoodHamburguer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Application.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IMenuService, MenuService>();
        return services;
    }
}

