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
        public ICollection<Product> Product { get; set; }
    }
}
