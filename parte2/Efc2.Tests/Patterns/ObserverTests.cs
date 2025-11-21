using Efc2.Models;
using Efc2.Patterns.Observer;
using Efc2.Patterns.State;
using Moq;
using Xunit;

namespace Efc2.Tests.Patterns
{
    public class ObserverTests
    {
        [Fact]
        public void Notify_ShouldUpdateAllObservers_WhenStateChanges()
        {
            // Arrange
            var order = new Order();
            var mockObserver1 = new Mock<IObserver>();
            var mockObserver2 = new Mock<IObserver>();

            order.Attach(mockObserver1.Object);
            order.Attach(mockObserver2.Object);

            // Act
            // Trigger a state change (e.g., Cancel)
            order.Cancel();

            // Assert
            // Verify that Update was called on both observers
            mockObserver1.Verify(o => o.Update(order), Times.Once);
            mockObserver2.Verify(o => o.Update(order), Times.Once);
        }

        [Fact]
        public void Detach_ShouldStopNotifications()
        {
            // Arrange
            var order = new Order();
            var mockObserver = new Mock<IObserver>();

            order.Attach(mockObserver.Object);
            order.Detach(mockObserver.Object);

            // Act
            order.Cancel();

            // Assert
            mockObserver.Verify(o => o.Update(It.IsAny<Order>()), Times.Never);
        }
    }
}
