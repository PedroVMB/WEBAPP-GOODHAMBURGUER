using GoodHamburguer.Model.Enums;

namespace GoodHamburguer.Application.DTOs;

public class UpdateOrderRequest
{
    public SandwichType? Sandwich { get; set; }
    public bool IncludeFries { get; set; }
    public bool IncludeSoda { get; set; }
}

