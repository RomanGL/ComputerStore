using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.Controllers
{
    public class AdminController : Controller
    {
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
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        [HttpGet]
        public ViewResult Create() => View(nameof(Edit), new Product());
    }
}