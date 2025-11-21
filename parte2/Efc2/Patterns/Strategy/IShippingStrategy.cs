using Efc2.Models;

namespace Efc2.Patterns.Strategy;

public interface IShippingStrategy
{
    string ShippingTypeName();
    decimal Calculate(Order order);
}
