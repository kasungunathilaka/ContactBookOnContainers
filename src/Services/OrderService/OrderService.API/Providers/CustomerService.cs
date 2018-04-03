using Microsoft.EntityFrameworkCore;
using OrderService.API.Infrastructure;
using OrderService.API.Models;
using OrderService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Providers
{
    public class CustomerService: ICustomerService
    {
        private OrderContext _dbConext;
        private Guid _customerGuid, _contactDetailsGuid;

        public CustomerService(OrderContext dbContext)
        {
            _dbConext = dbContext;
        }

        public async Task CreateCustomer(CustomerViewModel addedCustomer)
        {
            _customerGuid = Guid.NewGuid();
            _contactDetailsGuid = Guid.NewGuid();

            List<Address> addresses = new List<Address>();
            if (addedCustomer.Addresses != null)
            {
                foreach (var addressVM in addedCustomer.Addresses)
                {
                    Address address = new Address
                    {
                        AddressId = Guid.NewGuid(),
                        Street = addressVM.Street,
                        City = addressVM.City,
                        Province = addressVM.Province,
                        ZipCode = addressVM.ZipCode,
                        CustomerId = _customerGuid
                    };
                    addresses.Add(address);
                }
            }

            ContactDetails contactDetails = new ContactDetails
            {
                ContactDetailsId = _contactDetailsGuid,
                CustomerId = _customerGuid,
                Email = addedCustomer.Email,
                FacebookId = addedCustomer.FacebookId,
                MobilePhone = addedCustomer.MobilePhone,
                HomePhone = addedCustomer.HomePhone
            };

            Customer customer = new Customer
            {
                CustomerId = _customerGuid,
                FirstName = addedCustomer.FirstName,
                LastName = addedCustomer.LastName,
                Gender = addedCustomer.Gender
            };

            await _dbConext.Customers.AddAsync(customer);
            await _dbConext.ContactDetails.AddAsync(contactDetails);
            await _dbConext.Addresses.AddRangeAsync(addresses);
            _dbConext.SaveChanges();

        }

        public async Task DeleteCustomer(string customerId)
        {
            if (customerId != null)
            {
                Customer customer = await _dbConext.Customers.FirstOrDefaultAsync(c => c.CustomerId.ToString().Equals(customerId));
                ContactDetails contactDetails = await _dbConext.ContactDetails.FirstOrDefaultAsync(d => d.CustomerId.ToString().Equals(customerId));
                List<Address> addresses = await _dbConext.Addresses.Where(a => a.CustomerId.ToString().Equals(customerId)).ToListAsync();

                if (contactDetails != null)
                {
                    _dbConext.ContactDetails.Remove(contactDetails);
                }
                if (addresses != null)
                {
                    _dbConext.Addresses.RemoveRange(addresses);
                }
                _dbConext.Customers.Remove(customer);
                _dbConext.SaveChanges();
            }
        }

        public async Task<List<string>> GetAllCustomerNames()
        {
            List<string> contactNames = new List<string>();
            List<Customer> customers = await _dbConext.Customers.ToListAsync();

            if (customers.Count > 0)
            {
                foreach (var customer in customers)
                {
                    if (customer != null)
                    {
                        string name = customer.FirstName + " " + customer.LastName;
                        contactNames.Add(name);
                    }
                }
                return contactNames;
            }
            return null;
        }

        public async Task<List<CustomerViewModel>> GetAllCustomers()
        {
            List<CustomerViewModel> customersVM = new List<CustomerViewModel>();
            List<Customer> customers = await _dbConext.Customers.OrderBy(c => c.FirstName).ToListAsync();

            if (customers != null)
            {
                foreach (var customer in customers)
                {
                    ContactDetails contactDetails = await _dbConext.ContactDetails.FirstOrDefaultAsync(d => d.CustomerId.Equals(customer.CustomerId));
                    List<Address> addresses = await _dbConext.Addresses.Where(a => a.CustomerId.Equals(customer.CustomerId)).ToListAsync();

                    List<AddressViewModel> addressesVM = new List<AddressViewModel>();
                    if (addresses != null)
                    {
                        foreach (var adderss in addresses)
                        {
                            AddressViewModel addressVM = new AddressViewModel
                            {
                                AddressId = adderss.AddressId,
                                Street = adderss.Street,
                                City = adderss.City,
                                Province = adderss.Province,
                                ZipCode = adderss.ZipCode
                            };
                            addressesVM.Add(addressVM);
                        }
                    }

                    CustomerViewModel customerDetails = new CustomerViewModel
                    {
                        CustomerId = customer.CustomerId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Gender = customer.Gender,
                        Addresses = addressesVM != null ? addressesVM : null,
                        ContactDetailsId = contactDetails != null ? contactDetails.ContactDetailsId : Guid.Parse(null),
                        Email = contactDetails != null ? contactDetails.Email : null,
                        FacebookId = contactDetails != null ? contactDetails.FacebookId : null,
                        HomePhone = contactDetails != null ? contactDetails.HomePhone : null,
                        MobilePhone = contactDetails != null ? contactDetails.MobilePhone : null
                    };
                    customersVM.Add(customerDetails);
                }
                return customersVM;
            }
            return null;         
        }

        public async Task<CustomerViewModel> SearchCustomerByName(string name)
        {
            List<Customer> customers = new List<Customer>();

            string[] words = name.Split(' ');
            foreach (var word in words)
            {
                List<Customer> resultfirst = await _dbConext.Customers.Where(c => c.FirstName.Contains(word)).ToListAsync();
                if (resultfirst.Count > 0)
                {
                    customers.AddRange(resultfirst);
                }
                List<Customer> resultLast = await _dbConext.Customers.Where(c => c.LastName.Contains(word)).ToListAsync();
                if (resultLast.Count > 0)
                {
                    customers.AddRange(resultLast);
                }
            }

            if (customers.Count > 0)
            {
                Customer customer = customers.Distinct().FirstOrDefault();
                if (customer != null)
                {
                    ContactDetails contactDetails = await _dbConext.ContactDetails.FirstOrDefaultAsync(d => d.CustomerId.Equals(customer.CustomerId));
                    List<Address> addresses = await _dbConext.Addresses.Where(a => a.CustomerId.Equals(customer.CustomerId)).ToListAsync();

                    List<AddressViewModel> addressesVM = new List<AddressViewModel>();
                    if (addresses != null)
                    {
                        foreach (var adderss in addresses)
                        {
                            AddressViewModel addressVM = new AddressViewModel
                            {
                                AddressId = adderss.AddressId,
                                Street = adderss.Street,
                                City = adderss.City,
                                Province = adderss.Province,
                                ZipCode = adderss.ZipCode
                            };
                            addressesVM.Add(addressVM);
                        }
                    }

                    CustomerViewModel customerDetails = new CustomerViewModel
                    {
                        CustomerId = customer.CustomerId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Gender = customer.Gender,
                        Addresses = addressesVM != null ? addressesVM : null,
                        ContactDetailsId = contactDetails != null ? contactDetails.ContactDetailsId : Guid.Parse(null),
                        Email = contactDetails != null ? contactDetails.Email : null,
                        FacebookId = contactDetails != null ? contactDetails.FacebookId : null,
                        HomePhone = contactDetails != null ? contactDetails.HomePhone : null,
                        MobilePhone = contactDetails != null ? contactDetails.MobilePhone : null
                    };
                    return customerDetails;
                }
            }
            return null;
        }
    }
}
