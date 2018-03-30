using Microsoft.EntityFrameworkCore;
using OrderService.API.Infrastructure;
using OrderService.API.Models;
using OrderService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Providers
{
    public class ProductService : IProductService
    {
        private OrderContext _dbConext;
        private Guid _productGuid, _productCatagoryGuid;

        public ProductService(OrderContext dbContext)
        {
            _dbConext = dbContext;
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
