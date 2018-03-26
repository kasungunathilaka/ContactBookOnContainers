using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _customerGuid = new Guid();
            _contactDetailsGuid = new Guid();
            _addressGuid = new Guid();

            List<Address> addresses = new List<Address>();
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
            _productGuid = new Guid();
            _productCatagoryGuid = new Guid();

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

        public async Task CreateOrder(string customerId, OrderViewModel addedOrder)
        {
            throw new NotImplementedException();
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
    }
}
