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
        private IOrdersService _orderService;
        private IProductService _productService;
        private ICustomerService _customerService;
        private ILogger<OrdersController> _logger;
        public OrdersController(IOrdersService orderService, ICustomerService customerService, IProductService productService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _productService = productService;
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                List<OrderViewModel> orders = new List<OrderViewModel>();
                orders = await _orderService.GetAllOrders();

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

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            try
            {
                OrderViewModel order = new OrderViewModel();
                order = await _orderService.GetOrderById(id);

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

        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                List<CustomerViewModel> customers = new List<CustomerViewModel>();
                customers = await _customerService.GetAllCustomers();

                if (customers != null)
                {
                    return Ok(customers);
                }
                return NotFound("Could not found any customers.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting customers: {ex.Message}");
            }
            return BadRequest("Could not found any customers.");
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                List<ProductViewModel> products = new List<ProductViewModel>();
                products = await _productService.GetAllProducts();

                if (products != null)
                {
                    return Ok(products);
                }
                return NotFound("Could not found any products.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting products: {ex.Message}");
            }
            return BadRequest("Could not found any products.");
        }

        [HttpPost("createCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerViewModel addedCustomer)
        {
            try
            {
                if (addedCustomer != null)
                {
                    await _customerService.CreateCustomer(addedCustomer);
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
                    await _productService.CreateProduct(addedProduct);
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
        public async Task<IActionResult> CreateOrder([FromBody]OrderViewModel addedOrder)
        {
            try
            {
                if (addedOrder != null)
                {
                    await _orderService.CreateOrder(addedOrder);
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
                contactNames = await _customerService.GetAllCustomerNames();

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
                productNames = await _productService.GetAllProductNames();

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

        [HttpGet("productCategoryNames")]
        public async Task<IActionResult> GetAllProductCategoryNames()
        {
            try
            {
                List<string> productCategoryNames = new List<string>();
                productCategoryNames = await _productService.GetAllProductCategoryNames();

                if (productCategoryNames != null)
                {
                    return Ok(productCategoryNames);
                }
                return NotFound("Could not found any Product Categories.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while getting Product Categories: {ex.Message}");
            }
            return BadRequest("Could not found any Product Categories.");
        }

        [HttpGet("product/{productName}")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            try
            {
                ProductViewModel product = await _productService.GetProductByName(productName);

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
                CustomerViewModel customer = await _customerService.SearchCustomerByName(tag);

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

        [HttpDelete("orders/{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            try
            {
                await _orderService.DeleteOrder(id);
                return Ok("Order Deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while deleting the order: {ex.Message}");
            }
            return BadRequest("Failed to delete the order.");
        }

        [HttpDelete("customers/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            try
            {
                await _customerService.DeleteCustomer(id);
                return Ok("Customer Deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while deleting the customer: {ex.Message}");
            }
            return BadRequest("Failed to delete the customer.");
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok("Product Deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while deleting the product: {ex.Message}");
            }
            return BadRequest("Failed to delete the product.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody]OrderViewModel editedOrder)
        {
            try
            {
                await _orderService.UpdateOrder(id, editedOrder);
                return Ok("Order Edited.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while editing the order: {ex.Message}");
            }
            return BadRequest("Failed to edit the order.");
        }

    }
}
