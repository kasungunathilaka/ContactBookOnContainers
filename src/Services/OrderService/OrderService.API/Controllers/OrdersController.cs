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

    }
}
