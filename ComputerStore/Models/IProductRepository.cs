﻿using System.Linq;

namespace ComputerStore.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
    }
}