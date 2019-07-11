using System;
using System.Linq;
using ComputerStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private const string TempDataMessageKey = "message";
        private readonly IProductRepository _productRepository;

        public AdminController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public ViewResult Index() => View(_productRepository.Products.AsEnumerable());

        [HttpGet]
        public ViewResult Edit(int productId) 
            => View(_productRepository.Products.FirstOrDefault(p => p.ProductId == productId));

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.SaveProduct(product);
                TempData[TempDataMessageKey] = $"{product.Name} has been saved";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        [HttpGet]
        public ViewResult Create() => View(nameof(Edit), new Product());

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            var deletedProduct = _productRepository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData[TempDataMessageKey] = $"{deletedProduct.Name} was deleted";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}