using System;

namespace OrderService.API.Models
{
    public class OrderItem: IAuditable
    {
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
