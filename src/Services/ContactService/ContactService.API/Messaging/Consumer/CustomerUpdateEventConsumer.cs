using ContactService.API.ContactProviders;
using ContactService.API.ViewModels;
using Events;
using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.API.Messaging.Consumer
{
    public class CustomerUpdateEventConsumer : IConsumer<CustomerUpdateEvent>
    {
        private readonly IContactsService _contactService;

        public CustomerUpdateEventConsumer(IContactsService contactsService)
        {
            _contactService = contactsService;
        }

        public async Task Consume(ConsumeContext<CustomerUpdateEvent> context)
        {
            var customer = JsonConvert.DeserializeObject<CustomerViewModel>(context.Message.Customer);

            if (customer != null)
            {
                ContactViewModel contactViewModel = new ContactViewModel()
                {
                    CustomerId = customer.CustomerId.ToString(),
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Gender = customer.Gender,
                    Email = customer.Email,
                    MobilePhone = customer.MobilePhone,
                    HomePhone = customer.HomePhone,
                    FacebookId = customer.FacebookId,
                    ContactDetailsId = customer.ContactDetailsId.ToString(),
                    Street = customer.Addresses.ElementAt(0) != null ? customer.Addresses.ElementAtOrDefault(0).Street : null,
                    City = customer.Addresses.ElementAt(0) != null ? customer.Addresses.ElementAtOrDefault(0).City : null,
                    Province = customer.Addresses.ElementAt(0) != null ? customer.Addresses.ElementAtOrDefault(0).Province : null,
                    ZipCode = customer.Addresses.ElementAt(0) != null ? customer.Addresses.ElementAtOrDefault(0).ZipCode : null
                };

                await _contactService.AddContact(contactViewModel);
            }

        }

        //public Task Consume(ConsumeContext<CustomerUpdateEvent> context)
        //{
        //    Console.WriteLine(context.Message.Customer);
        //    return Task.CompletedTask;
        //}

    }
}
