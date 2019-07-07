using ComputerStore.Controllers;
using ComputerStore.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ComputerStore.Tests
{
    public sealed class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();

            var controller = new OrderController(mock.Object, cart);

            // Act
            var result = controller.Checkout(new Order()) as ViewResult;

            // Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();

            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            var controller = new OrderController(mock.Object, cart);
            controller.ModelState.AddModelError("error", "error message");

            // Act
            var result = controller.Checkout(new Order()) as ViewResult;

            // Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();

            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            var controller = new OrderController(mock.Object, cart);

            // Act
            var result = controller.Checkout(new Order()) as RedirectToActionResult;

            // Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }

        [Fact]
        public void Can_Mark_Shipped()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            var cartLine = new CartLine { Product = new Product(), Quantity = 1 };
            var order = new Order { OrderId = 1, Shipped = false, Lines = new[] { cartLine } };

            mock.Setup(m => m.Orders).Returns(new[] { order }.AsQueryable());

            var controller = new OrderController(mock.Object, new Cart());

            // Act
            var result = controller.MarkShipped(1) as RedirectToActionResult;

            // Assert
            mock.Verify(m => m.SaveOrder(order), Times.Once);
            Assert.True(order.Shipped);
            Assert.Equal("List", result.ActionName);
        }

        [Fact]
        public void Can_List_Only_Unshipped_Orders()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.Orders).Returns(new[]
            {
                new Order { OrderId = 1, Shipped = false },
                new Order { OrderId = 2, Shipped = true },
                new Order { OrderId = 3, Shipped = false },
                new Order { OrderId = 4, Shipped = false },
                new Order { OrderId = 5, Shipped = true },
            }.AsQueryable());

            var controller = new OrderController(mock.Object, new Cart());

            // Act
            var result = controller.List();
            var orders = ((IEnumerable<Order>)result.ViewData.Model).ToArray();

            // Assert
            Assert.Equal(3, orders.Length);
            Assert.Equal(1, orders[0].OrderId);
            Assert.Equal(3, orders[1].OrderId);
            Assert.Equal(4, orders[2].OrderId);
        }
    }
}
