using ComputerStore.Components;
using ComputerStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ComputerStore.Tests
{
    public sealed class NavigationMenuViewComponentTest
    {
        [Fact]
        public void Can_Select_Categories()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1", Category = "Apples" },
                new Product { ProductId = 2, Name = "P2", Category = "Plums" },
                new Product { ProductId = 3, Name = "P3", Category = "Apples" },
                new Product { ProductId = 4, Name = "P4", Category = "Oranges" },
            }.AsQueryable());

            var target = new NavigationMenuViewComponent(mock.Object);

            // Act
            var componentResult = target.Invoke() as ViewViewComponentResult;
            var categories = ((IEnumerable<string>)componentResult.ViewData.Model).ToArray();

            // Assert
            Assert.True(categories.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            // Arrange
            string categoryToSelect = "Apples";
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1", Category = "Apples" },
                new Product { ProductId = 2, Name = "P2", Category = "Oranges" },
            }.AsQueryable());

            var target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            // Action
            var componentResult = target.Invoke() as ViewViewComponentResult;
            string result = componentResult.ViewData["SelectedCategory"].ToString();

            // Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
