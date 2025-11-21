using Efc2.Models;

namespace Efc2.Patterns.Strategy;

public class NormalShipping : IShippingStrategy
{
    private const decimal FixedShippingPrice = 10m;

    public decimal Calculate(Order order)
    {
        return FixedShippingPrice;
    }

    public string ShippingTypeName() => "Normal";
}
