using ComputerStore.Controllers;
using ComputerStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ComputerStore.Tests
{
    public sealed class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
            }.AsQueryable());

            var controller = new AdminController(mock.Object);

            // Act
            var result = GetViewModel<IEnumerable<Product>>(controller.Index()).ToArray();

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
            }.AsQueryable());

            var controller = new AdminController(mock.Object);

            // Act
            var p1 = GetViewModel<Product>(controller.Edit(1));
            var p2 = GetViewModel<Product>(controller.Edit(2));
            var p3 = GetViewModel<Product>(controller.Edit(3));

            // Assert
            Assert.Equal("P1", p1.Name);
            Assert.Equal("P2", p2.Name);
            Assert.Equal("P3", p3.Name);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
            }.AsQueryable());

            var controller = new AdminController(mock.Object);

            // Act
            var product = GetViewModel<Product>(controller.Edit(4));

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            var mock = new Mock<IProductRepository>();
            var tempData = new Mock<ITempDataDictionary>();

            var controller = new AdminController(mock.Object) { TempData = tempData.Object };
            var product = new Product { ProductId = 1, Name = "P1" };

            var result = controller.Edit(product);

            mock.Verify(m => m.SaveProduct(product), Times.Once);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            var mock = new Mock<IProductRepository>();
            var tempData = new Mock<ITempDataDictionary>();

            var controller = new AdminController(mock.Object) { TempData = tempData.Object };
            var product = new Product { ProductId = 1, Name = "P1" };

            controller.ModelState.AddModelError("error", "error");
            var result = controller.Edit(product);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            var mock = new Mock<IProductRepository>();
            var product = new Product { ProductId = 2, Name = "To delete" };

            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1" },
                product,
                new Product { ProductId = 3, Name = "P3" }
            }.AsQueryable());

            var controller = new AdminController(mock.Object);
            controller.Delete(product.ProductId);

            mock.Verify(m => m.DeleteProduct(product.ProductId), Times.Once);
        }

        private static T GetViewModel<T>(IActionResult result) where T : class
            => (result as ViewResult)?.ViewData.Model as T;
    }
}
