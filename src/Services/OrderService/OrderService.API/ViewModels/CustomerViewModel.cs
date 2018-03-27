using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.ViewModels
{
    public class CustomerViewModel
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public ICollection<AddressViewModel> Addresses { get; set; }
        public Guid ContactDetailsId { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string FacebookId { get; set; }
    }

    public class AddressViewModel
    {
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
    }
}
