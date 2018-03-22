using ContactService.API.ContactProviders;
using ContactService.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class ContactsController : Controller
    {
        private IContactsService _service;
        private ILogger<ContactsController> _logger;
        public ContactsController(IContactsService service, ILogger<ContactsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                List<ContactViewModel> contacts = new List<ContactViewModel>();
                contacts = await _service.GetAllContacts();

                if (contacts.Count > 0)
                {
                    return Ok(contacts);
                }
                return NotFound("Could not found any contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting contacts: {ex.Message}");
            }
            return BadRequest("Could not found any contacts.");
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetAllContactNames()
        {
            try
            {
                List<string> contactNames = new List<string>();
                contactNames = await _service.GetAllContactNames();

                if (contactNames.Count > 0)
                {
                    return Ok(contactNames);
                }
                return NotFound("Could not found any contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting contacts: {ex.Message}");
            }
            return BadRequest("Could not found any contacts.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(string id)
        {
            try
            {
                ContactViewModel contact = new ContactViewModel();
                contact = await _service.GetContactById(id);

                if (contact != null)
                {
                    return Ok(contact);
                }
                return NotFound("Could not found any contact.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting contact: {ex.Message}");
            }
            return BadRequest("Could not found any contact.");
        }

        [HttpGet("search/{tag}")]
        public async Task<IActionResult> SearchContact(string tag)
        {
            try
            {
                List<ContactViewModel> contacts = new List<ContactViewModel>();
                List<ContactViewModel> searchByName = await _service.SearchContactByName(tag);
                List<ContactViewModel> searchByNumber = await _service.SearchContactByNumber(tag);

                if (searchByName != null)
                {
                    contacts.AddRange(searchByName);
                }
                if (searchByNumber != null)
                {
                    contacts.AddRange(searchByNumber);
                }

                if (contacts != null)
                {
                    return Ok(contacts);
                }
                return NotFound("Could not found any contact.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting contacts: {ex.Message}");
            }
            return BadRequest("Could not found any contacts.");
        }


        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody]ContactViewModel addedContact)
        {
            try
            {
                if (addedContact != null)
                {
                    await _service.AddContact(addedContact);
                    return Ok("New contact Added.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while adding a new contact: {ex.Message}");
            }
            return BadRequest("Failed to add new contact.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditContact(string id, [FromBody]ContactViewModel editedContact)
        {
            try
            {
                await _service.EditContact(id, editedContact);
                //GetContactById(id);
                return Ok("Contact Edited.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while editing the contact: {ex.Message}");
            }
            return BadRequest("Failed to edit the contact.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {
            try
            {
                await _service.DeleteContact(id);
                return Ok("Contact Deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while deleting the contact: {ex.Message}");
            }
            return BadRequest("Failed to delete the contact.");
        }
    }
}
