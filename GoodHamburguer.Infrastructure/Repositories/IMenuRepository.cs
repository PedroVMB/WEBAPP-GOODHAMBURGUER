using GoodHamburguer.Model;

namespace GoodHamburguer.Infrastructure.Repositories;

public interface IMenuRepository
{
    Task<IEnumerable<MenuItem>> GetAllAsync();
    Task<MenuItem?> GetByIdAsync(int id);
}

