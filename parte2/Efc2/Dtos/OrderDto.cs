using Efc2.Models;

namespace Efc2.Dtos;

public class CreateOrderDto
{
    public string? Email { get; init; }
    public string? Cellphone { get; init; }
    public string? ShippingType { get; init; }
    public string? PaymentMethod { get; init; }
    public List<Product> Products { get; init; } = [];
}

public class OrderDto
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public required string Cellphone { get; init; }
    public required string Status { get; init; }
    public required List<Product> Products { get; init; }
    public required string PaymentMethod { get; init; }
    public required string ShippingType { get; init; }
    public required decimal SubTotal { get; init; }
    public required decimal ShippingCost { get; init; }
    public required decimal Total { get; init; }
}
