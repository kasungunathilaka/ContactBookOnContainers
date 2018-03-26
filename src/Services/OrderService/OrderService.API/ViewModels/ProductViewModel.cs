using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.ViewModels
{
    public class ProductViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
    }
}
