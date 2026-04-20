namespace GoodHamburguer.Application.DTOs;

public class OrderResponse
{
    public int Id { get; set; }
    public string Sandwich { get; set; } = string.Empty;
    public string? Fries { get; set; }
    public string? Soda { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

