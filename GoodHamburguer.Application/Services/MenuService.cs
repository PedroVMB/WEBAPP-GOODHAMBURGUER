using GoodHamburguer.Application.DTOs;
using GoodHamburguer.Infrastructure.Repositories;

namespace GoodHamburguer.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<IEnumerable<MenuItemResponse>> GetMenuAsync()
    {
        var items = await _menuRepository.GetAllAsync();
        return items.Select(m => new MenuItemResponse
        {
            Id = m.Id,
            Name = m.Name,
            Price = m.Price,
            Category = m.ItemType == 1 ? "Sandwich" : "Side"
        });
    }
}

