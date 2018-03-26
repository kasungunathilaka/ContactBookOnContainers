using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.ViewModels
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCompleted { get; set; }
        public Guid CustomerId { get; set; }
        public ICollection<OrderItemViewModel> OrderItems { get; set; }
    }

    public class OrderItemViewModel
    {
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
    }
}
