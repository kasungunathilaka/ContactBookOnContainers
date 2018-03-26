using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Models
{
    public class ProductCategory : IAuditable
    {
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
