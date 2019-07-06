using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ComputerStore.Models
{
    public sealed class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public EFOrderRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Order> Orders => _context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderId == 0)
            {
                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }
    }
}
