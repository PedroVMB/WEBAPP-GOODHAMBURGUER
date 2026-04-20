using GoodHamburguer.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.API.Controllers;

[ApiController]
[Route("api/menu")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        var items = await _menuService.GetMenuAsync();
        return Ok(items);
    }
}

