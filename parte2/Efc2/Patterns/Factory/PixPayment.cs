namespace Efc2.Patterns.Factory;

public class PixPayment : IPaymentMethod
{
    public string PaymentMethodName() => "pix";

    public string Process(decimal amount)
    {
        return $"Paid {amount:C} using PIX QrCode.";
    }
}
