using System;

namespace OrderService.API.Models
{
    public class Address : IAuditable
    {
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
