using Efc2.Patterns.State;
using Efc2.Patterns.Observer;
using Efc2.Dtos;
using Efc2.Patterns.Factory;
using Efc2.Patterns.Strategy;

namespace Efc2.Models;

public class Order : ISubject
{
    public int? Id { get; set; }
    public string? Email { get; set; }
    public string? Cellphone { get; set; }

    public ILogger<Order>? Logger { get; set; }

    public decimal ShippingCost { get; set; }

    public string Status { get; private set; } = "New";
    public IPaymentMethod? PaymentMethod { get; set; }
    public IShippingStrategy? ShippingStrategy { get; set; }

    private readonly List<Product> _products = [];

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public void AddProducts(IEnumerable<Product> products)
    {
        _products.AddRange(products);
    }

    // State Pattern
    private OrderState? _state;

    // Observer Pattern
    private readonly List<IObserver> _observers = [];

    public Order()
    {
        // Initial state
        TransitionTo(new NewState(this));
    }

    public void TransitionTo(OrderState state)
    {
        _state = state;
        Status = _state.ToString();
        Notify();
    }

    public void Pay() => _state!.Pay();
    public void Ship() => _state!.Ship();
    public void Cancel() => _state!.Cancel();

    // Calculated property
    public decimal SubTotal => _products.Sum(p => p.Price);
    public decimal Total => SubTotal + ShippingCost;

    // ISubject Implementation
    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }

    public OrderDto ToDto() => new()
    {
        Id = Id!.Value,
        Email = Email ?? string.Empty,
        Cellphone = Cellphone ?? string.Empty,
        Products = [.. _products.Select(p => p with { })], // Clone
        Status = Status,
        PaymentMethod = PaymentMethod?.PaymentMethodName() ?? string.Empty,
        ShippingType = ShippingStrategy?.ShippingTypeName() ?? string.Empty,
        SubTotal = SubTotal,
        ShippingCost = ShippingCost,
        Total = Total
    };
}
