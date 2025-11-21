using Efc2.Models;

namespace Efc2.Patterns.State;

public class NewState(Order order) : OrderState(order)
{
    protected override string StateName => "New";

    public override void Pay()
    {
        _context.Logger?.LogInformation("Processing payment...");
        var result = _context.PaymentMethod!.Process(_context.Total);
        _context.Logger?.LogInformation("{Result}", result);
        _context.TransitionTo(new PaidState(_context));
    }

    public override void Ship()
    {
        _context.Logger?.LogError("Error: Cannot ship an unpaid order.");
        throw new InvalidOperationException("Cannot ship an unpaid order.");
    }

    public override void Cancel()
    {
        _context.TransitionTo(new CancelledState(_context));
        _context.Logger?.LogInformation("Order cancelled.");
    }
}
