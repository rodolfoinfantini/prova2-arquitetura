using Efc2.Models;

namespace Efc2.Patterns.Strategy;

public class FreeShipping : IShippingStrategy
{
    public decimal Calculate(Order order)
    {
        return 0m;
    }

    public string ShippingTypeName() => "Free";
}
