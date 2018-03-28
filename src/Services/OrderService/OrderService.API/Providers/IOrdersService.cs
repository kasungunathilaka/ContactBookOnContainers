using OrderService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Providers
{
    public interface IOrdersService
    {
        Task CreateCustomer(CustomerViewModel addedCustomer);
        Task CreateProduct(ProductViewModel addedProduct);
        Task CreateOrder(string customerId, OrderViewModel addedOrder);
        Task AddProductsToOrder(string orderId, OrderItemViewModel addedProduct);
        Task RemoveProductsFromOrder(string orderId, string orderItemId);
        Task UpdateOrder(string orderId, OrderViewModel editedOrder);
        Task DeleteOrder(string orderId);
        Task<List<OrderViewModel>> GetAllOrders();
        Task<OrderViewModel> GetOrderById(string orderId);
        Task<List<string>> GetAllCustomerNames();
        Task<CustomerViewModel> SearchCustomerByName(string name);
        Task<List<string>> GetAllProducts();
        Task<List<string>> GetAllProductCategories();
    }
}
