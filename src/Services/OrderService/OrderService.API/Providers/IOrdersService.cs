using OrderService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Providers
{
    public interface IOrdersService
    {              
        Task CreateOrder(OrderViewModel addedOrder);
        Task UpdateOrder(string orderId, OrderViewModel editedOrder);
        Task DeleteOrder(string orderId);
        Task<List<OrderViewModel>> GetAllOrders();
        Task<OrderViewModel> GetOrderById(string orderId);
    }
}
