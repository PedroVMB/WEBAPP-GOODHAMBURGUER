using GoodHamburguer.Application.DTOs;

namespace GoodHamburguer.Application.Services;

public interface IMenuService
{
    Task<IEnumerable<MenuItemResponse>> GetMenuAsync();
}

