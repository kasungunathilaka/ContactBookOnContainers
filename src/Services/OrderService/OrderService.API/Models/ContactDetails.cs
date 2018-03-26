using System;

namespace OrderService.API.Models
{
    public class ContactDetails : IAuditable
    {
        public Guid ContactDetailsId { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string FacebookId { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
