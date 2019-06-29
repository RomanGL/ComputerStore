using System.Linq;

namespace ComputerStore.Models
{
    public sealed class FakeProductRepository : IProductRepository
    {
        public IQueryable<Product> Products => new[]
        {
            new Product { Name = "Intel Core i5-6500", Price = 250 },
            new Product { Name = "AMD Ryzen 7 1800X", Price = 199 },
            new Product { Name = "Intel Celeron G1610", Price = 69 },
        }.AsQueryable();
    }
}
