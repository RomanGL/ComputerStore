using ComputerStore.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace ComputerStore.Models
{
    public sealed class SessionCart : Cart
    {
        private const string SessionCartKey = "Cart";

        public static Cart GetCart(IServiceProvider serviceProvider)
        {
            var session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            if (session == null)
            {
                throw new NullReferenceException("Can't get a current session.");
            }

            var cart = session.GetJson<SessionCart>(SessionCartKey) ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        private ISession Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson(SessionCartKey, this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson(SessionCartKey, this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove(SessionCartKey);
        }
    }
}
