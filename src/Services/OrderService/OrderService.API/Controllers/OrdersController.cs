using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderService.API.Providers;
using OrderService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        private IOrdersService _service;
        private ILogger<OrdersController> _logger;
        public OrdersController(IOrdersService service, ILogger<OrdersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                List<OrderViewModel> orders = new List<OrderViewModel>();
                orders = await _service.GetAllOrders();

                if (orders != null)
                {
                    return Ok(orders);
                }
                return NotFound("Could not found any orders.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting orders: {ex.Message}");
            }
            return BadRequest("Could not found any orders.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            try
            {
                OrderViewModel order = new OrderViewModel();
                order = await _service.GetOrderById(id);

                if (order != null)
                {
                    return Ok(order);
                }
                return NotFound("Could not found any order.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting order: {ex.Message}");
            }
            return BadRequest("Could not found any order.");
        }

        [HttpPost("createCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerViewModel addedCustomer)
        {
            try
            {
                if (addedCustomer != null)
                {
                    await _service.CreateCustomer(addedCustomer);
                    return Ok("New customer Added.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating a new customer: {ex.Message}");
            }
            return BadRequest("Failed to create new customer.");
        }

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromBody]ProductViewModel addedProduct)
        {
            try
            {
                if (addedProduct != null)
                {
                    await _service.CreateProduct(addedProduct);
                    return Ok("New product Added.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating a new product: {ex.Message}");
            }
            return BadRequest("Failed to create new product.");
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder(string customerId, [FromBody]OrderViewModel addedOrder)
        {
            try
            {
                if (addedOrder != null)
                {
                    await _service.CreateOrder(customerId, addedOrder);
                    return Ok("New order Added.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating a new order: {ex.Message}");
            }
            return BadRequest("Failed to create new order.");
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetAllCustomerNames()
        {
            try
            {
                List<string> contactNames = new List<string>();
                contactNames = await _service.GetAllCustomerNames();

                if (contactNames != null)
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

        [HttpGet("productNames")]
        public async Task<IActionResult> GetAllProductNames()
        {
            try
            {
                List<string> productNames = new List<string>();
                productNames = await _service.GetAllProductNames();

                if (productNames != null)
                {
                    return Ok(productNames);
                }
                return NotFound("Could not found any Products.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting products: {ex.Message}");
            }
            return BadRequest("Could not found any products.");
        }

        [HttpGet("product/{productName}")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            try
            {
                ProductViewModel product = await _service.GetProductByName(productName);

                if (product != null)
                {
                    return Ok(product);
                }
                return NotFound("Could not found any Product.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting product: {ex.Message}");
            }
            return BadRequest("Could not found any product.");
        }

        [HttpGet("search/{tag}")]
        public async Task<IActionResult> SearchCustomerByName(string tag)
        {
            try
            {
                CustomerViewModel customer = await _service.SearchCustomerByName(tag);

                if (customer != null)
                {
                    return Ok(customer);
                }
                return NotFound("Could not found any customer.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting customer: {ex.Message}");
            }
            return BadRequest("Could not found any customer.");
        }

    }
}
