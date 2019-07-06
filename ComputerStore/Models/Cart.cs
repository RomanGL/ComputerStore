using System.Collections.Generic;
using System.Linq;

namespace ComputerStore.Models
{
    public class Cart
    {
        private readonly List<CartLine> _lines = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            var line = _lines
                .Where(p => p.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (line == null)
            {
                _lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) => _lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
        public virtual decimal ComputeTotalValue() => _lines.Sum(l => l.Product.Price * l.Quantity);
        public virtual void Clear() => _lines.Clear();
        public virtual IEnumerable<CartLine> Lines => _lines;
    }
}
