using System;

namespace OrderService.API.Models
{
    public class Order: IAuditable
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCompleted { get; set; }
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
