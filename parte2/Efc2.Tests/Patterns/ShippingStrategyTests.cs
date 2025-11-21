using Efc2.Models;
using Efc2.Patterns.Strategy;
using Xunit;

namespace Efc2.Tests.Patterns
{
    public class ShippingStrategyTests
    {
        private Order CreateOrderWithAmount(decimal amount)
        {
            var order = new Order();
            order.AddProduct(new Product("Test Product", amount));
            return order;
        }

        [Fact]
        public void FreeShipping_ShouldReturnZero()
        {
            // Arrange
            var strategy = new FreeShipping();
            var order = CreateOrderWithAmount(100m);

            // Act
            var cost = strategy.Calculate(order);

            // Assert
            Assert.Equal(0m, cost);
        }

        [Fact]
        public void NormalShipping_ShouldReturnFixedPrice()
        {
            // Arrange
            var strategy = new NormalShipping();
            var order = CreateOrderWithAmount(100m);

            // Act
            var cost = strategy.Calculate(order);

            // Assert
            Assert.Equal(10m, cost); // Fixed 10
        }

        [Fact]
        public void SedexShipping_ShouldReturnBasePlusFivePercent()
        {
            // Arrange
            var strategy = new SedexShipping();
            var order = CreateOrderWithAmount(100m);

            // Act
            var cost = strategy.Calculate(order);

            // Assert
            // Calculation: 20 (Base) + (100 * 0.05) = 25
            Assert.Equal(25m, cost); 
        }
    }
}

