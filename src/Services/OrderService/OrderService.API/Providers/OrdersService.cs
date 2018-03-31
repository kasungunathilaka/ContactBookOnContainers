using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Infrastructure;
using OrderService.API.Models;
using OrderService.API.ViewModels;

namespace OrderService.API.Providers
{
    public class OrdersService : IOrdersService
    {
        private OrderContext _dbConext;
        private Guid _orderGuid;

        public OrdersService(OrderContext dbContext)
        {
            _dbConext = dbContext;
        }
                        
        public async Task CreateOrder(OrderViewModel addedOrder)
        {
            if (addedOrder.Customer != null)
            {
                _orderGuid = Guid.NewGuid();
                List<OrderItem> orderItems = new List<OrderItem>();

                if (addedOrder.OrderItems != null)
                {
                    foreach (var itemVM in addedOrder.OrderItems)
                    {
                        OrderItem orderItem = new OrderItem
                        {
                            OrderItemId = Guid.NewGuid(),
                            OrderId = _orderGuid,
                            Quantity = itemVM.Quantity,
                            ProductId = itemVM.ProductId
                        };
                        orderItems.Add(orderItem);
                    }
                }

                Order order = new Order
                {
                    OrderId = _orderGuid,
                    CustomerId = addedOrder.Customer.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    IsCompleted = addedOrder.IsCompleted
                };

                await _dbConext.Orders.AddAsync(order);
                await _dbConext.OrderItems.AddRangeAsync(orderItems);
                _dbConext.SaveChanges();
            }           
        }
             
        public Task UpdateOrder(string orderId, OrderViewModel editedOrder)
        {
            throw new NotImplementedException();
        }
        public Task DeleteOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderViewModel>> GetAllOrders()
        {
            List<OrderViewModel> ordersVM = new List<OrderViewModel>();
            List<Order> orders = await _dbConext.Orders.OrderBy(o => o.OrderDate).ToListAsync();

            if (orders.Count > 0 && orders != null)
            {
                foreach (var order in orders)
                {                                                                              
                    Customer customer = await _dbConext.Customers.FirstOrDefaultAsync(c => c.CustomerId.Equals(order.CustomerId));                   
                                     
                    if (customer != null)
                    {
                        List<OrderItem> orderItems = await _dbConext.OrderItems.Where(o => o.OrderId.Equals(order.OrderId)).ToListAsync();
                        List<OrderItemViewModel> orderItemsVM = new List<OrderItemViewModel>();

                        if (orderItems != null)
                        {
                            foreach (var item in orderItems)
                            {
                                Product product = await _dbConext.Products.FirstOrDefaultAsync(p => p.ProductId.Equals(item.ProductId));
                                ProductCategory productCategory = await _dbConext.ProductCategories.FirstOrDefaultAsync(pc => pc.ProductCategoryId.Equals(product.ProductCategoryId));

                                if (product != null)
                                {
                                    OrderItemViewModel orderItemVM = new OrderItemViewModel
                                    {
                                        OrderItemId = item.OrderItemId,
                                        Quantity = item.Quantity,
                                        ProductId = item.ProductId,
                                        ProductName = product.ProductName,
                                        Description = product.Description,
                                        Price = product.Price,
                                        IsAvailable = product.IsAvailable,
                                        ProductCategoryId = product.ProductCategoryId,
                                        ProductCategoryName = productCategory.ProductCategoryName
                                    };
                                    orderItemsVM.Add(orderItemVM);
                                }
                            }
                        }

                        ContactDetails contactDetails = await _dbConext.ContactDetails.FirstOrDefaultAsync(c => c.CustomerId.Equals(order.CustomerId));
                        List<Address> addresses = await _dbConext.Addresses.Where(a => a.CustomerId.Equals(order.CustomerId)).ToListAsync();
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

                        CustomerViewModel customerVM = new CustomerViewModel
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

                        OrderViewModel orderVM = new OrderViewModel
                        {
                            OrderId = order.OrderId,
                            OrderDate = order.OrderDate.ToString(),
                            IsCompleted = order.IsCompleted,
                            OrderItems = orderItemsVM,
                            Customer = customerVM
                        };
                        ordersVM.Add(orderVM);                        
                    }                   
                }
                return ordersVM;
            }
            return null;           
        }

        public async Task<OrderViewModel> GetOrderById(string orderId)
        {
            Order order = await _dbConext.Orders.FirstOrDefaultAsync(o => o.OrderId.ToString().Equals(orderId));

            if (order != null)
            {               
                Customer customer = await _dbConext.Customers.FirstOrDefaultAsync(c => c.CustomerId.Equals(order.CustomerId));
                
                if (customer != null)
                {
                    List<OrderItem> orderItems = await _dbConext.OrderItems.Where(o => o.OrderId.Equals(order.OrderId)).ToListAsync();
                    List<OrderItemViewModel> orderItemsVM = new List<OrderItemViewModel>();

                    if (orderItems != null)
                    {
                        foreach (var item in orderItems)
                        {
                            Product product = await _dbConext.Products.FirstOrDefaultAsync(p => p.ProductId.Equals(item.ProductId));
                            ProductCategory productCategory = await _dbConext.ProductCategories.FirstOrDefaultAsync(pc => pc.ProductCategoryId.Equals(product.ProductCategoryId));

                            OrderItemViewModel orderItemVM = new OrderItemViewModel
                            {
                                OrderItemId = item.OrderItemId,
                                Quantity = item.Quantity,
                                ProductId = item.ProductId,
                                ProductName = product.ProductName,
                                Description = product.Description,
                                Price = product.Price,
                                IsAvailable = product.IsAvailable,
                                ProductCategoryId = product.ProductCategoryId,
                                ProductCategoryName = productCategory.ProductCategoryName
                            };
                            orderItemsVM.Add(orderItemVM);
                        }
                    }

                    ContactDetails contactDetails = await _dbConext.ContactDetails.FirstOrDefaultAsync(c => c.CustomerId.Equals(order.CustomerId));
                    List<Address> addresses = await _dbConext.Addresses.Where(a => a.CustomerId.Equals(order.CustomerId)).ToListAsync();
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

                    CustomerViewModel customerVM = new CustomerViewModel
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

                    OrderViewModel orderVM = new OrderViewModel
                    {
                        OrderId = order.OrderId,
                        OrderDate = order.OrderDate.Date.ToString(),
                        IsCompleted = order.IsCompleted,
                        Customer = customerVM,
                        OrderItems = orderItemsVM
                    };
                    return orderVM;
                }
                
            }
            return null;
        }
             
    }
}
