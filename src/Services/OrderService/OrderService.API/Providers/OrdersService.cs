﻿using System;
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
        private Guid _customerGuid, _contactDetailsGuid, _addressGuid, _orderGuid, _orderItemGuid, _productGuid, _productCatagoryGuid;

        public OrdersService(OrderContext dbContext)
        {
            _dbConext = dbContext;
        }

        public async Task CreateCustomer(CustomerViewModel addedCustomer)
        {
            _customerGuid = Guid.NewGuid();
            _contactDetailsGuid = Guid.NewGuid();
            _addressGuid = Guid.NewGuid();

            List<Address> addresses = new List<Address>();
            if (addedCustomer.Addresses != null)
            {
                foreach (var addressVM in addedCustomer.Addresses)
                {
                    Address address = new Address
                    {
                        AddressId = _addressGuid,
                        Street = addressVM.Street,
                        City = addressVM.City,
                        Province = addressVM.Province,
                        ZipCode = addressVM.ZipCode,
                        CustomerId = _customerGuid
                    };
                    addresses.Add(address);
                }
            }
            
            ContactDetails contactDetails = new ContactDetails
            {
                ContactDetailsId = _contactDetailsGuid,
                CustomerId = _customerGuid,
                Email = addedCustomer.Email,
                FacebookId = addedCustomer.FacebookId,
                MobilePhone = addedCustomer.MobilePhone,
                HomePhone = addedCustomer.HomePhone
            };

            Customer customer = new Customer
            {
                CustomerId = _customerGuid,
                FirstName = addedCustomer.FirstName,
                LastName = addedCustomer.LastName,
                Gender = addedCustomer.Gender
            };

            await _dbConext.Customers.AddAsync(customer);
            await _dbConext.ContactDetails.AddAsync(contactDetails);
            await _dbConext.Address.AddRangeAsync(addresses);
            await _dbConext.SaveChangesAsync();

        }

        public async Task CreateProduct(ProductViewModel addedProduct)
        {
            _productGuid = Guid.NewGuid();
            _productCatagoryGuid = Guid.NewGuid();

            ProductCategory productCategory = new ProductCategory
            {
                ProductCategoryId = _productCatagoryGuid,
                ProductId = _productGuid,
                ProductCategoryName = addedProduct.ProductCategoryName
            };

            Product product = new Product
            {
                ProductId = _productGuid,
                ProductName = addedProduct.ProductName,
                Description = addedProduct.Description,
                Price = addedProduct.Price,
                IsAvailable = addedProduct.IsAvailable
            };

            await _dbConext.Products.AddAsync(product);
            await _dbConext.ProductCategories.AddAsync(productCategory);
            await _dbConext.SaveChangesAsync();
        }

        //todo
        public async Task CreateOrder(string customerId, OrderViewModel addedOrder)
        {
            _customerGuid = Guid.Parse(customerId);
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
                        ProductId = itemVM.ProductId,
                    };
                    orderItems.Add(orderItem);
                }
            }
            
            Order order = new Order
            {
                OrderId = _orderGuid,
                CustomerId = _customerGuid,
                OrderDate = addedOrder.OrderDate,
                IsCompleted = addedOrder.IsCompleted
            };

            await _dbConext.Orders.AddAsync(order);
            await _dbConext.OrderItems.AddRangeAsync(orderItems);
            await _dbConext.SaveChangesAsync();
        }
              
        public Task AddProductsToOrder(string orderId, OrderItemViewModel addedProduct)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProductsFromOrder(string orderId, string orderItemId)
        {
            throw new NotImplementedException();
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
                    List<OrderItem> orderItems = await _dbConext.OrderItems.Where(o => o.OrderId.Equals(order.OrderId)).ToListAsync();
                    List<OrderItemViewModel> orderItemsVM = new List<OrderItemViewModel>();

                    foreach (var item in orderItems)
                    {
                        Product product = await _dbConext.Products.FirstOrDefaultAsync(p => p.ProductId.Equals(item.ProductId));
                        ProductCategory productCategory = await _dbConext.ProductCategories.FirstOrDefaultAsync(pc => pc.ProductId.Equals(item.ProductId));

                        OrderItemViewModel orderItemVM = new OrderItemViewModel
                        {
                            OrderItemId = item.OrderItemId,
                            Quantity = item.Quantity,
                            ProductId = item.ProductId,
                            ProductName = product.ProductName,
                            Description = product.Description,
                            Price = product.Price,
                            IsAvailable = product.IsAvailable,
                            ProductCategoryId = productCategory.ProductCategoryId,
                            ProductCategoryName = productCategory.ProductCategoryName
                        };
                        orderItemsVM.Add(orderItemVM);
                    }
                    
                    OrderViewModel orderVM = new OrderViewModel
                    {
                        OrderId = order.OrderId,
                        OrderDate = order.OrderDate,
                        IsCompleted = order.IsCompleted,
                        CustomerId = order.CustomerId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,    
                        OrderItems = orderItemsVM
                    };
                    ordersVM.Add(orderVM);
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
                List<OrderItem> orderItems = await _dbConext.OrderItems.Where(o => o.OrderId.Equals(order.OrderId)).ToListAsync();
                List<OrderItemViewModel> orderItemsVM = new List<OrderItemViewModel>();

                if (orderItems != null)
                {
                    foreach (var item in orderItems)
                    {
                        Product product = await _dbConext.Products.FirstOrDefaultAsync(p => p.ProductId.Equals(item.ProductId));
                        ProductCategory productCategory = await _dbConext.ProductCategories.FirstOrDefaultAsync(pc => pc.ProductId.Equals(item.ProductId));

                        OrderItemViewModel orderItemVM = new OrderItemViewModel
                        {
                            OrderItemId = item.OrderItemId,
                            Quantity = item.Quantity,
                            ProductId = item.ProductId,
                            ProductName = product.ProductName,
                            Description = product.Description,
                            Price = product.Price,
                            IsAvailable = product.IsAvailable,
                            ProductCategoryId = productCategory.ProductCategoryId,
                            ProductCategoryName = productCategory.ProductCategoryName
                        };
                        orderItemsVM.Add(orderItemVM);
                    }
                }
                
                OrderViewModel orderVM = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    IsCompleted = order.IsCompleted,
                    CustomerId = order.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    OrderItems = orderItemsVM
                };
                return orderVM;
            }
            return null;
        }
    }
}