using Microsoft.AspNetCore.Mvc;
using Efc2.Models;
using Efc2.Patterns.Observer;
using Efc2.Dtos;
using Efc2.Patterns.Builder;
using Efc2.Database;

namespace Efc2.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController(
    /* Pattern: Singleton */ IDatabase<Order, int> ordersDatabase,
    ILogger<OrdersController> logger,
    ILoggerFactory loggerFactory
    ) : ControllerBase
{
    private NotFoundObjectResult NotFound(int id) => base.NotFound($"Order {id} not found.");

    [HttpPost]
    public ActionResult<OrderDto> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        try
        {
            // Pattern: Builder
            var order = new OrderBuilder()
                .Id(ordersDatabase.GenerateId())
                .Logger(loggerFactory.CreateLogger<Order>())
                .From(createOrderDto)
                .Build();

            // Pattern: Observer
            order.Attach(new EmailNotification(loggerFactory.CreateLogger<EmailNotification>()));
            order.Attach(new WhatsappNotification(loggerFactory.CreateLogger<WhatsappNotification>()));

            ordersDatabase.Save(order);
            order.Notify();

            return Created($"/{order.Id}", order.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on CreateOrder");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public ActionResult<OrderDto> GetOrder(int id)
    {
        var order = ordersDatabase.GetById(id);
        if (order is null) return NotFound(id);

        return Ok(order.ToDto());
    }

    [HttpPatch("{id:int}/pay")]
    public ActionResult<OrderDto> PayOrder(int id)
    {
        var order = ordersDatabase.GetById(id);
        if (order is null) return NotFound(id);

        try
        {
            // Pattern: State
            order.Pay();
            return Ok(order.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on PayOrder");
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id:int}/ship")]
    public ActionResult<OrderDto> ShipOrder(int id)
    {
        var order = ordersDatabase.GetById(id);
        if (order is null) return NotFound(id);

        try
        {
            // Pattern: State
            order.Ship();
            return Ok(order.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Ship");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult<OrderDto> CancelOrder(int id)
    {
        var order = ordersDatabase.GetById(id);
        if (order is null) return NotFound(id);

        try
        {
            // Pattern: State
            order.Cancel();
            return Ok(order.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on CancelOrder");
            return BadRequest(ex.Message);
        }
    }
}
