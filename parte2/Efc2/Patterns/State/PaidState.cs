using Efc2.Models;

namespace Efc2.Patterns.State;

public class PaidState(Order order) : OrderState(order)
{
    protected override string StateName => "Paid";

    public override void Pay()
    {
        _context.Logger?.LogError("Error: Order is already paid.");
        throw new InvalidOperationException("Order is already paid.");
    }

    public override void Ship()
    {
        _context.Logger?.LogInformation("Shipping order...");
        _context.TransitionTo(new ShippedState(_context));
    }

    public override void Cancel()
    {
        _context.Logger?.LogInformation("Cancelling paid order (refunding)...");
        _context.TransitionTo(new CancelledState(_context));
    }
}
