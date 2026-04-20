using GoodHamburguer.Model.Enums;

namespace GoodHamburguer.Model;

public class Order : Base
{
    public SandwichType? Sandwich { get; set; }
    public SideType? Fries { get; set; }
    public SideType? Soda { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    
}

