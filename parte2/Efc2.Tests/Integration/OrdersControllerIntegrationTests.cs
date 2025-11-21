using Efc2.Controllers;
using Efc2.Database;
using Efc2.Dtos;
using Efc2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Efc2.Tests.Integration
{
    public class OrdersControllerIntegrationTests
    {
        [Fact]
        public void CompleteOrderFlow_ShouldSucceed_WhenUsingRealDatabaseAndLogic()
        {
            // Arrange - Setup Real Dependencies (No Mocks)
            var database = new InMemoryOrderDatabase(NullLogger<InMemoryOrderDatabase>.Instance);
            var controller = new OrdersController(database, NullLogger<OrdersController>.Instance, NullLoggerFactory.Instance);

            var createDto = new CreateOrderDto
            {
                Email = "integration@test.com",
                Cellphone = "999999999",
                Products = [new Product("Laptop", 2500m), new Product("Mouse", 50m)],
                PaymentMethod = "credit",
                ShippingType = "sedex"
            };

            // Act 1: Create Order
            var createResult = controller.CreateOrder(createDto);

            // Assert 1: Verify Creation
            var createdActionResult = Assert.IsType<CreatedResult>(createResult.Result);
            var createdOrderDto = Assert.IsType<OrderDto>(createdActionResult.Value);

            Assert.NotNull(createdOrderDto);
            Assert.Equal(1, createdOrderDto.Id);
            Assert.Equal("New", createdOrderDto.Status);
            Assert.Equal(2550m, createdOrderDto.SubTotal);

            // Sedex Calculation: 20 (Fixed) + 5% of 2550 (127.5) = 147.5
            Assert.Equal(147.5m, createdOrderDto.ShippingCost);
            Assert.Equal(2697.5m, createdOrderDto.Total);

            int orderId = createdOrderDto.Id;

            // Act 2: Pay Order
            var payResult = controller.PayOrder(orderId);

            // Assert 2: Verify Payment State Transition
            var payActionResult = Assert.IsType<OkObjectResult>(payResult.Result);
            var paidOrderDto = Assert.IsType<OrderDto>(payActionResult.Value);
            Assert.Equal("Paid", paidOrderDto.Status);

            // Act 3: Ship Order
            var shipResult = controller.ShipOrder(orderId);

            // Assert 3: Verify Shipping State Transition
            var shipActionResult = Assert.IsType<OkObjectResult>(shipResult.Result);
            var shippedOrderDto = Assert.IsType<OrderDto>(shipActionResult.Value);
            Assert.Equal("Shipped", shippedOrderDto.Status);

            // Act 4: Verify Persistence via GetById
            var getResult = controller.GetOrder(orderId);
            var getActionResult = Assert.IsType<OkObjectResult>(getResult.Result);
            var retrievedOrderDto = Assert.IsType<OrderDto>(getActionResult.Value);

            Assert.Equal("Shipped", retrievedOrderDto.Status);
            Assert.Equal(createdOrderDto.Total, retrievedOrderDto.Total);
        }
    }
}


