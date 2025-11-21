using Efc2.Models;

namespace Efc2.Patterns.State;

public class ShippedState(Order order) : OrderState(order)
{
    protected override string StateName => "Shipped";

    public override void Pay()
    {
        _context.Logger?.LogError("Error: Order already shipped.");
        throw new InvalidOperationException("Order already shipped.");
    }

    public override void Ship()
    {
        _context.Logger?.LogError("Error: Order already shipped.");
        throw new InvalidOperationException("Order already shipped.");
    }

    public override void Cancel()
    {
        _context.Logger?.LogError("Error: Cannot cancel a shipped order.");
        throw new InvalidOperationException("Cannot cancel a shipped order.");
    }
}
