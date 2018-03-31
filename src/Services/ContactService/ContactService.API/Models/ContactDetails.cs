using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.API.Models
{
    public class ContactDetails: IAuditable
    {
        public Guid Id { get; set; }
        public Address Address { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string FacebookId { get; set; }
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
    }
}
