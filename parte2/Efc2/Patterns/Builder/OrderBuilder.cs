using Efc2.Dtos;
using Efc2.Models;
using Efc2.Patterns.Factory;
using Efc2.Patterns.Strategy;

namespace Efc2.Patterns.Builder;

public class OrderBuilder
{
    private readonly Order _order = new();

    public OrderBuilder Id(int id)
    {
        _order.Id = id;
        return this;
    }

    public OrderBuilder Logger(ILogger<Order> logger)
    {
        _order.Logger = logger;
        return this;
    }

    public OrderBuilder Email(string? email)
    {
        ArgumentNullException.ThrowIfNull(email);

        _order.Email = email;
        return this;
    }

    public OrderBuilder Cellphone(string? cellphone)
    {
        ArgumentNullException.ThrowIfNull(cellphone);

        _order.Cellphone = cellphone;
        return this;
    }

    public OrderBuilder Products(IEnumerable<Product> products)
    {
        _order.AddProducts(products);
        return this;
    }

    public OrderBuilder ShippingType(string? type)
    {
        ArgumentNullException.ThrowIfNull(type);

        // Pattern: Strategy
        IShippingStrategy? strategy = type.Trim().ToLower() switch
        {
            "sedex" => new SedexShipping(),
            "normal" => new NormalShipping(),
            "free" => new FreeShipping(),
            _ => null
        };

        if (strategy is null) throw new ArgumentOutOfRangeException(type, "Invalid shipping type (sedex, normal, free)");

        _order.ShippingStrategy = strategy;
        _order.ShippingCost = strategy.Calculate(_order);
        return this;
    }

    public OrderBuilder PaymentMethod(string? paymentMethod)
    {
        ArgumentNullException.ThrowIfNull(paymentMethod);

        // Pattern: Factory
        var paymentMethodService = PaymentFactory.Create(paymentMethod);
        _order.PaymentMethod = paymentMethodService;
        return this;
    }

    public OrderBuilder From(CreateOrderDto createOrderDto)
    {
        Email(createOrderDto.Email);
        Cellphone(createOrderDto.Cellphone);
        Products(createOrderDto.Products);
        ShippingType(createOrderDto.ShippingType);
        PaymentMethod(createOrderDto.PaymentMethod);
        return this;
    }

    public Order Build() => _order;
}
