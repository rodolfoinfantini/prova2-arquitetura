using Efc2.Models;

namespace Efc2.Patterns.State;

public class CancelledState(Order order) : OrderState(order)
{
    protected override string StateName => "Cancelled";

    public override void Cancel()
    {
        _context.Logger?.LogError("Error: Order already cancelled.");
        throw new InvalidOperationException("Order already cancelled.");
    }

    public override void Pay()
    {
        _context.Logger?.LogError("Error: Cannot pay a cancelled order.");
        throw new InvalidOperationException("Cannot pay a cancelled order.");
    }

    public override void Ship()
    {
        _context.Logger?.LogError("Error: Cannot ship a cancelled order.");
        throw new InvalidOperationException("Cannot ship a cancelled order.");
    }
}
