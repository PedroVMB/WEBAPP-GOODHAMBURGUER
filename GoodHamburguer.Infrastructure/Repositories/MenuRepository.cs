using GoodHamburguer.Infrastructure.Data;
using GoodHamburguer.Model;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly AppDbContext _context;

    public MenuRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        return await _context.MenuItems
            .Where(m => !m.IsDisabled)
            .ToListAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        return await _context.MenuItems
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDisabled);
    }
}

