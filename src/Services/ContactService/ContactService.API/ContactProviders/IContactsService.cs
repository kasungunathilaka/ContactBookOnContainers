using ContactService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.API.ContactProviders
{
    public interface IContactsService
    {
        Task<List<ContactViewModel>> GetAllContacts();
        Task<ContactViewModel> GetContactById(string id);
        Task<List<ContactViewModel>> SearchContactByName(string name);
        Task<List<ContactViewModel>> SearchContactByNumber(string number);
        Task<List<string>> GetAllContactNames();
        Task AddContact(ContactViewModel addedContact);
        Task DeleteContact(string id);
        Task EditContact(string id, ContactViewModel editedContact);
    }
}
