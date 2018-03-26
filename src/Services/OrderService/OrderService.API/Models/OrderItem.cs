using System;

namespace OrderService.API.Models
{
    public class OrderItem : IAuditable
    {
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
