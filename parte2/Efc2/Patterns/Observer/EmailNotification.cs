using Efc2.Models;

namespace Efc2.Patterns.Observer;

public class EmailNotification(ILogger logger) : IObserver
{
    public void Update(Order order)
    {
        logger.LogInformation("Email sent to {Email}: Order {Id} status changed to {Status}.", order.Email, order.Id, order.Status);
    }
}
