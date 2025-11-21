namespace Efc2.Patterns.Factory;

public class PaymentFactory
{
    public static IPaymentMethod Create(string type)
    {
        return type.ToLower() switch
        {
            "credit" => new CreditCardPayment(),
            "pix" => new PixPayment(),
            _ => throw new ArgumentException("Invalid payment method (credit, pix)")
        };
    }
}
