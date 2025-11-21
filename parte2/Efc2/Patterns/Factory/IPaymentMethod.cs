namespace Efc2.Patterns.Factory;

public interface IPaymentMethod
{
    string PaymentMethodName();
    string Process(decimal amount);
}
