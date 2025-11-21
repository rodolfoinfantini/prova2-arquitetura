namespace Efc2.Patterns.Factory;

public class CreditCardPayment : IPaymentMethod
{
    public string PaymentMethodName() => "credit";

    public string Process(decimal amount)
    {
        return $"Paid {amount:C} using Credit Card.";
    }
}
