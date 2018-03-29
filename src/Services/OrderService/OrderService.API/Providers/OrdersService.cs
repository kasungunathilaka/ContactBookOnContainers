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
            await _dbConext.Addresses.AddRangeAsync(addresses);
            _dbConext.SaveChanges();

        }

        public async Task CreateProduct(ProductViewModel addedProduct)
        {
            _productGuid = Guid.NewGuid();
            _productCatagoryGuid = Guid.NewGuid();

            ProductCategory productCategory = new ProductCategory
            {
                ProductCategoryId = _productCatagoryGuid,
                ProductCategoryName = addedProduct.ProductCategoryName
            };

            Product product = new Product
            {
                ProductId = _productGuid,
                ProductCategoryId = _productCatagoryGuid,
                ProductName = addedProduct.ProductName,
                Description = addedProduct.Description,
                Price = addedProduct.Price,
                IsAvailable = addedProduct.IsAvailable
            };

            await _dbConext.Products.AddAsync(product);
            await _dbConext.ProductCategories.AddAsync(productCategory);
            _dbConext.SaveChanges();
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
            _dbConext.SaveChanges();
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
                    
                    Customer customer = await _dbConext.Customers.FirstOrDefaultAsync(c => c.CustomerId.Equals(order.CustomerId));
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
                    
                    OrderViewModel orderVM = new OrderViewModel
                    {
                        OrderId = order.OrderId,
                        OrderDate = order.OrderDate,
                        IsCompleted = order.IsCompleted,
                        CustomerId = customer.CustomerId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Gender = customer.Gender,
                        ContactDetailsId = contactDetails.ContactDetailsId,
                        Email = contactDetails.Email,
                        MobilePhone = contactDetails.MobilePhone,
                        HomePhone = contactDetails.HomePhone,
                        FacebookId = contactDetails.FacebookId,
                        Addresses = addressesVM,
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

                Customer customer = await _dbConext.Customers.FirstOrDefaultAsync(c => c.CustomerId.Equals(order.CustomerId));
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
                
                OrderViewModel orderVM = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    IsCompleted = order.IsCompleted,
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Gender = customer.Gender,
                    ContactDetailsId = contactDetails.ContactDetailsId,
                    Email = contactDetails.Email,
                    MobilePhone = contactDetails.MobilePhone,
                    HomePhone = contactDetails.HomePhone,
                    FacebookId = contactDetails.FacebookId,
                    Addresses = addressesVM,
                    OrderItems = orderItemsVM
                };
                return orderVM;
            }
            return null;
        }

        public async Task<List<string>> GetAllCustomerNames()
        {
            List<string> contactNames = new List<string>();
            List<Customer> customers = await _dbConext.Customers.ToListAsync();

            if (customers.Count > 0)
            {
                foreach (var customer in customers)
                {
                    if (customer != null)
                    {
                        string name = customer.FirstName + " " + customer.LastName;
                        contactNames.Add(name);
                    }
                }
                return contactNames;
            }
            return null;
        }

        public async Task<CustomerViewModel> SearchCustomerByName(string name)
        {
            List<Customer> customers = new List<Customer>();

            string[] words = name.Split(' ');
            foreach (var word in words)
            {
                List<Customer> resultfirst = await _dbConext.Customers.Where(c => c.FirstName.Contains(word)).ToListAsync();
                if (resultfirst.Count > 0)
                {
                    customers.AddRange(resultfirst);
                }
                List<Customer> resultLast = await _dbConext.Customers.Where(c => c.LastName.Contains(word)).ToListAsync();
                if (resultLast.Count > 0)
                {
                    customers.AddRange(resultLast);
                }
            }

            if (customers.Count > 0)
            {
                Customer customer = customers.Distinct().FirstOrDefault();
                if (customer != null)
                {
                    ContactDetails details = await _dbConext.ContactDetails.FirstOrDefaultAsync(d => d.CustomerId.Equals(customer.CustomerId));
                    List<Address> addresses = await _dbConext.Addresses.Where(a => a.CustomerId.Equals(customer.CustomerId)).ToListAsync();

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

                    CustomerViewModel customerDetails = new CustomerViewModel
                    {
                        CustomerId = customer.CustomerId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Gender = customer.Gender,
                        Email = details.Email,
                        MobilePhone = details.MobilePhone,
                        HomePhone = details.HomePhone,
                        FacebookId = details.FacebookId,
                        ContactDetailsId = details.ContactDetailsId,
                        Addresses = addressesVM
                    };
                    return customerDetails;
                }              
            }return null;
        }

        public async Task<ProductViewModel> GetProductByName(string productName)
        {
            Product product = await _dbConext.Products.FirstOrDefaultAsync(p => p.ProductName.Equals(productName));            

            if (product != null)
            {                 
                ProductCategory productCategory = await _dbConext.ProductCategories.FirstOrDefaultAsync(c => c.ProductCategoryId.Equals(product.ProductCategoryId));
                ProductViewModel productVM = new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    IsAvailable = product.IsAvailable,
                    Price = product.Price,
                    ProductCategoryId = product.ProductCategoryId,
                    ProductCategoryName = productCategory.ProductCategoryName
                };
                return productVM;
            }
            return null;
        }

        public async Task<List<string>> GetAllProductNames()
        {
            List<string> productNames = new List<string>();
            List<Product> products = await _dbConext.Products.ToListAsync();

            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    if (product != null)
                    {
                        productNames.Add(product.ProductName);
                    }
                }
                return productNames;
            }
            return null;
        }

    }
}
