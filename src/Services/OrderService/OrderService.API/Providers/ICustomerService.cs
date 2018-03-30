using OrderService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Providers
{
    public interface ICustomerService
    {
        Task CreateCustomer(CustomerViewModel addedCustomer);
        Task<List<string>> GetAllCustomerNames();
        Task<CustomerViewModel> SearchCustomerByName(string name);
    }
}
