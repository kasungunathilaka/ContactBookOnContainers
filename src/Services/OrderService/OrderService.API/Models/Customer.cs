using System;
using System.Collections.Generic;

namespace OrderService.API.Models
{
    public class Customer: IAuditable
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ContactDetails ContactDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
