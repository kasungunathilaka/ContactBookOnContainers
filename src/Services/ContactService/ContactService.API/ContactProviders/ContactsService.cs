using Microsoft.EntityFrameworkCore;
using ContactService.API.Infrastructure;
using ContactService.API.Models;
using ContactService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.API.ContactProviders
{
    public class ContactsService : IContactsService
    {
        private ContactBookContext _dbConext;
        private Guid _customerGuid, _contactDetailsGuid;

        public ContactsService(ContactBookContext dbConext)
        {
            _dbConext = dbConext;
        }

        public async Task<List<ContactViewModel>> GetAllContacts()
        {
            List<ContactViewModel> contactDetails = new List<ContactViewModel>();
            List<Customer> customers = await _dbConext.Customers.OrderBy(c => c.FirstName).ToListAsync();

            if (customers.Count > 0 && customers != null)
            {
                foreach (var customer in customers)
                {
                    if (customer != null)
                    {
                        ContactDetails details = await _dbConext.ContactDetails.FirstOrDefaultAsync(d => d.CustomerId.Equals(customer.Id));
                        Address address = await _dbConext.Address.FirstOrDefaultAsync(a => a.ContactDetailsId.Equals(details.Id));

                        var contact = new ContactViewModel
                        {
                            CustomerId = customer.Id.ToString(),
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Gender = customer.Gender,
                            Email = details.Email,
                            MobilePhone = details.MobilePhone,
                            HomePhone = details.HomePhone,
                            FacebookId = details.FacebookId,
                            ContactDetailsId = details.Id.ToString(),
                            Street = address.Street,
                            City = address.City,
                            Province = address.Province,
                            ZipCode = address.ZipCode
                        };
                        contactDetails.Add(contact);
                    }
                }
                return contactDetails;
            }
            return null;
        }

        public async Task<List<string>> GetAllContactNames()
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

        public async Task<ContactViewModel> GetContactById(string id)
        {
            Customer customer = await _dbConext.Customers.FirstOrDefaultAsync(c => c.Id.ToString().Equals(id));

            if (customer != null)
            {
                ContactDetails details = await _dbConext.ContactDetails.FirstOrDefaultAsync(d => d.CustomerId.Equals(customer.Id));
                Address address = await _dbConext.Address.FirstOrDefaultAsync(a => a.ContactDetailsId.Equals(details.Id));

                ContactViewModel contact = new ContactViewModel
                {
                    CustomerId = customer.Id.ToString(),
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Gender = customer.Gender,
                    Email = details.Email,
                    MobilePhone = details.MobilePhone,
                    HomePhone = details.HomePhone,
                    FacebookId = details.FacebookId,
                    ContactDetailsId = details.Id.ToString(),
                    Street = address.Street,
                    City = address.City,
                    Province = address.Province,
                    ZipCode = address.ZipCode
                };
                return contact;
            }
            return null;
        }

        public async Task AddContact(ContactViewModel addedContact)
        {
            _customerGuid = Guid.NewGuid();
            _contactDetailsGuid = Guid.NewGuid();

            Customer customer = new Customer
            {
                Id = _customerGuid,
                FirstName = addedContact.FirstName,
                LastName = addedContact.LastName,
                Gender = addedContact.Gender
            };

            ContactDetails details = new ContactDetails
            {
                Id = _contactDetailsGuid,
                CustomerId = _customerGuid,
                Email = addedContact.Email,
                MobilePhone = addedContact.MobilePhone,
                HomePhone = addedContact.HomePhone,
                FacebookId = addedContact.FacebookId
            };

            Address address = new Address
            {
                ContactDetailsId = _contactDetailsGuid,
                City = addedContact.City,
                Province = addedContact.Province,
                Street = addedContact.Street,
                ZipCode = addedContact.ZipCode
            };

            await _dbConext.Customers.AddAsync(customer);
            await _dbConext.ContactDetails.AddAsync(details);
            await _dbConext.Address.AddAsync(address);
            _dbConext.SaveChanges();
        }

        public async Task DeleteContact(string id)
        {
            ContactViewModel contact = await GetContactById(id);
            Customer customer = _dbConext.Customers.FirstOrDefault(c => c.Id.ToString().Equals(contact.CustomerId));
            ContactDetails details = _dbConext.ContactDetails.FirstOrDefault(d => d.Id.ToString().Equals(contact.ContactDetailsId));
            Address address = _dbConext.Address.FirstOrDefault(a => a.ContactDetailsId.ToString().Equals(contact.ContactDetailsId));

            if (customer != null && details != null)
            {
                _dbConext.Customers.Remove(customer);
                _dbConext.ContactDetails.Remove(details);
                _dbConext.Address.Remove(address);
                _dbConext.SaveChanges();
            }
        }

        public async Task EditContact(string id, ContactViewModel editedContact)
        {
            ContactViewModel contact = await GetContactById(id);
            Customer customer = _dbConext.Customers.FirstOrDefault(c => c.Id.ToString().Equals(contact.CustomerId));
            ContactDetails details = _dbConext.ContactDetails.FirstOrDefault(d => d.Id.ToString().Equals(contact.ContactDetailsId));
            Address address = _dbConext.Address.FirstOrDefault(a => a.ContactDetailsId.ToString().Equals(contact.ContactDetailsId));

            if (customer != null && details != null)
            {
                customer.FirstName = editedContact.FirstName;
                customer.LastName = editedContact.LastName;
                customer.Gender = editedContact.Gender;

                details.Email = editedContact.Email;
                details.MobilePhone = editedContact.MobilePhone;
                details.HomePhone = editedContact.HomePhone;
                details.FacebookId = editedContact.FacebookId;

                address.Street = editedContact.Street;
                address.City = editedContact.City;
                address.Province = editedContact.Province;
                address.ZipCode = editedContact.ZipCode;

                _dbConext.Customers.Update(customer);
                _dbConext.ContactDetails.Update(details);
                _dbConext.Address.Update(address);
                _dbConext.SaveChanges();
            }
        }

        public async Task<List<ContactViewModel>> SearchContactByName(string name)
        {
            List<ContactViewModel> contactDetails = new List<ContactViewModel>();
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
                List<Customer> uniqueCustomers = customers.Distinct().OrderBy(c => c.FirstName).ToList();
                foreach (var customer in uniqueCustomers)
                {
                    if (customer != null)
                    {
                        ContactDetails details = await _dbConext.ContactDetails.FirstOrDefaultAsync(d => d.CustomerId.Equals(customer.Id));
                        Address address = await _dbConext.Address.FirstOrDefaultAsync(a => a.ContactDetailsId.Equals(details.Id));

                        var contact = new ContactViewModel
                        {
                            CustomerId = customer.Id.ToString(),
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Gender = customer.Gender,
                            Email = details.Email,
                            MobilePhone = details.MobilePhone,
                            HomePhone = details.HomePhone,
                            FacebookId = details.FacebookId,
                            ContactDetailsId = details.Id.ToString(),
                            Street = address.Street,
                            City = address.City,
                            Province = address.Province,
                            ZipCode = address.ZipCode
                        };
                        contactDetails.Add(contact);
                    }
                }
                return contactDetails;
            }
            return null;
        }

        public async Task<List<ContactViewModel>> SearchContactByNumber(string number)
        {
            List<ContactViewModel> contactDetails = new List<ContactViewModel>();
            List<ContactDetails> details = new List<ContactDetails>();

            string[] words = number.Split(' ');
            foreach (var word in words)
            {
                List<ContactDetails> resultMobile = await _dbConext.ContactDetails.Where(c => c.MobilePhone.Contains(word)).ToListAsync();
                if (resultMobile.Count > 0)
                {
                    details.AddRange(resultMobile);
                }
                List<ContactDetails> resultHome = await _dbConext.ContactDetails.Where(c => c.HomePhone.Contains(word)).ToListAsync();
                if (resultHome.Count > 0)
                {
                    details.AddRange(resultHome);
                }
            }

            if (details.Count > 0)
            {
                List<ContactDetails> uniqueDetails = details.Distinct().ToList();
                foreach (var detail in uniqueDetails)
                {
                    if (detail != null)
                    {
                        ContactViewModel contact = await GetContactById(detail.CustomerId.ToString());
                        contactDetails.Add(contact);
                    }
                }
                return contactDetails;
            }
            return null;
        }
    }
}
