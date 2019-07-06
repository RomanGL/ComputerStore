using ComputerStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ComputerStore.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartSummaryViewComponent(Cart cart)
        {
            _cart = cart ?? throw new ArgumentNullException(nameof(cart));
        }

        public IViewComponentResult Invoke() => View(_cart);
    }
}
