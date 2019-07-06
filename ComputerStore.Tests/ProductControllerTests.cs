using ComputerStore.Controllers;
using ComputerStore.Models;
using ComputerStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using Xunit;

namespace ComputerStore.Tests
{
    public sealed class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" },
            }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            var viewModel = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            var array = viewModel.Products.ToArray();
            Assert.True(array.Length == 2);
            Assert.Equal("P4", array[0].Name);
            Assert.Equal("P5", array[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_ViewModel()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" },
            }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            var viewModel = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            var pagingInfo = viewModel.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductId = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductId = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductId = 5, Name = "P5", Category = "Cat3" }
            }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            var viewModel = controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel;
            var result = viewModel.Products.ToArray();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[0].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            ProductsListViewModel GetModel(ViewResult result)
                => result.ViewData.Model as ProductsListViewModel;

            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductId = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductId = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductId = 5, Name = "P5", Category = "Cat3" }
            }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            int cat1Count = GetModel(controller.List("Cat1")).PagingInfo.TotalItems;
            int cat2Count = GetModel(controller.List("Cat2")).PagingInfo.TotalItems;
            int cat3Count = GetModel(controller.List("Cat3")).PagingInfo.TotalItems;
            int allCount = GetModel(controller.List(null)).PagingInfo.TotalItems;

            // Assert
            Assert.Equal(2, cat1Count);
            Assert.Equal(2, cat2Count);
            Assert.Equal(1, cat3Count);
            Assert.Equal(5, allCount);
        }
    }
}
