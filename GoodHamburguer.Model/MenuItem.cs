namespace GoodHamburguer.Model;

public class MenuItem : Base
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int ItemType { get; set; }
}

