using Efc2.Models;

namespace Efc2.Patterns.Observer;

public interface IObserver
{
    void Update(Order order);
}
