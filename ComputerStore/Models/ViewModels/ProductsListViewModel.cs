using System.Collections.Generic;

namespace ComputerStore.Models.ViewModels
{
    public sealed class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
