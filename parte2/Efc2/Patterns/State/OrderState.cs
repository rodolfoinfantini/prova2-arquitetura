using Efc2.Models;

namespace Efc2.Patterns.State;

public abstract class OrderState(Order order)
{
    protected abstract string StateName { get; }

    protected readonly Order _context = order;

    public abstract void Pay();
    public abstract void Ship();
    public abstract void Cancel();

    public override string ToString()
    {
        return StateName;
    }
}
