﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ComputerStore.Models
{
    public sealed class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Product> Products => _context.Products;

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                _context.Products.Attach(product).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }
    }
}
