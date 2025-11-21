using Efc2.Models;

namespace Efc2.Patterns.Observer;

public class WhatsappNotification(ILogger logger) : IObserver
{
    public void Update(Order order)
    {
        logger.LogInformation("Whatsapp sent to {Cellphone}: Order {Id} status changed to {Status}.", order.Cellphone, order.Id, order.Status);
    }
}
