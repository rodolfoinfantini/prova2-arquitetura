using Efc2.Models;
using Efc2.Patterns.Builder;
using Efc2.Patterns.Factory;
using Efc2.Patterns.Strategy;
using Xunit;

namespace Efc2.Tests.Patterns
{
    public class OrderBuilderTests
    {
        [Fact]
        public void Build_ShouldCreateOrderWithCorrectProperties()
        {
            // Arrange
            var builder = new OrderBuilder();
            var products = new List<Product> { new("Item 1", 50), new("Item 2", 50) };

            // Act
            var order = builder
                .Id(1)
                .Email("test@example.com")
                .Cellphone("123456789")
                .Products(products)
                .ShippingType("normal")
                .PaymentMethod("pix")
                .Build();

            // Assert
            Assert.Equal(1, order.Id);
            Assert.Equal("test@example.com", order.Email);
            Assert.Equal("123456789", order.Cellphone);
            Assert.Equal(100m, order.SubTotal);
            Assert.IsType<NormalShipping>(order.ShippingStrategy);
            Assert.IsType<PixPayment>(order.PaymentMethod);
            
            // Shipping Cost for Normal is 10% of 100 = 10
            Assert.Equal(10m, order.ShippingCost);
            Assert.Equal(110m, order.Total);
        }

        [Fact]
        public void Build_ShouldThrowException_WhenShippingTypeIsInvalid()
        {
            // Arrange
            var builder = new OrderBuilder();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.ShippingType("teleport"));
        }

        [Fact]
        public void Build_ShouldThrowException_WhenPaymentMethodIsInvalid()
        {
            // Arrange
            var builder = new OrderBuilder();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => builder.PaymentMethod("gold"));
        }
    }
}
