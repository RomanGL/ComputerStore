using System;
using System.Linq;
using ComputerStore.Models;
using ComputerStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public int PageSize { get; set; } = 4;

        public ViewResult List(string category, int page = 1)
        {
            IQueryable<Product> products;
            bool isCategoryEmpty = string.IsNullOrEmpty(category);

            if (isCategoryEmpty)
            {
                products = _repository.Products
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            }
            else
            {
                products = _repository.Products
                    .Where(p => p.Category == null || p.Category == category)
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            }

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = isCategoryEmpty ? 
                    _repository.Products.Count() :
                    _repository.Products.Where(e => e.Category == category).Count()
            };

            return View(new ProductsListViewModel
            {
                Products = products,
                PagingInfo = pagingInfo,
                CurrentCategory = category
            });
        }
    }
}