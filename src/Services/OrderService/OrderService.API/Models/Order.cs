using System;
using System.Collections.Generic;

namespace OrderService.API.Models
{
    public class Order : IAuditable
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCompleted { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
