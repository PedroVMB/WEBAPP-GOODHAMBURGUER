namespace GoodHamburguer.Model;

public class Base
{
    public int Id { get; set; }
    public bool IsDisabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}