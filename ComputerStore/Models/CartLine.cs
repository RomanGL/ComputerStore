﻿namespace ComputerStore.Models
{
    public sealed class CartLine
    {
        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
