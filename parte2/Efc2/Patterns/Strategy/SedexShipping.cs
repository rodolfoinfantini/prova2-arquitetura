using Efc2.Models;

namespace Efc2.Patterns.Strategy;

public class SedexShipping : IShippingStrategy
{
    public decimal Calculate(Order order)
    {
        // Base 20 + 5% of value
        return 20m + (order.SubTotal * 0.05m);
    }

    public string ShippingTypeName() => "Sedex";
}
