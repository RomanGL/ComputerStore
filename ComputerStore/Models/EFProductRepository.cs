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
    }
}
