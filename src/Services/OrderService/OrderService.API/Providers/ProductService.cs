﻿using Microsoft.EntityFrameworkCore;
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
            Product product;
            ProductCategory productCategory;

            if (addedProduct.ProductCategoryName !=  null)
            {
                List<string> productCategoryNames = await GetAllProductCategoryNames();

                if (productCategoryNames != null && productCategoryNames.Contains(addedProduct.ProductCategoryName))
                {
                    productCategory = await _dbConext.ProductCategories.FirstOrDefaultAsync(c => c.ProductCategoryName.Equals(addedProduct.ProductCategoryName));

                    product = new Product
                    {
                        ProductId = _productGuid,
                        ProductCategoryId = productCategory.ProductCategoryId,
                        ProductName = addedProduct.ProductName,
                        Description = addedProduct.Description,
                        Price = addedProduct.Price,
                        IsAvailable = addedProduct.IsAvailable
                    };
                    await _dbConext.Products.AddAsync(product);
                }
                else
                {
                    productCategory = new ProductCategory
                    {
                        ProductCategoryId = _productCatagoryGuid,
                        ProductCategoryName = addedProduct.ProductCategoryName
                    };

                    product = new Product
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
                }               
                _dbConext.SaveChanges();
            }           
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

        public async Task<List<string>> GetAllProductCategoryNames()
        {
            List<string> productCategoryNames = new List<string>();
            List<ProductCategory> productCategories = await _dbConext.ProductCategories.ToListAsync();

            if (productCategories.Count > 0)
            {
                foreach (var productCategory in productCategories)
                {
                    if (productCategory != null)
                    {
                        productCategoryNames.Add(productCategory.ProductCategoryName);
                    }
                }
                return productCategoryNames;
            }
            return null;
        }

        public async Task<List<ProductViewModel>> GetAllProducts()
        {
            List<ProductViewModel> productsVM = new List<ProductViewModel>();
            List<Product> products = await _dbConext.Products.OrderBy(p => p.ProductName).ToListAsync();

            if (products != null)
            {
                foreach (var product in products)
                {
                    if (product.ProductCategoryId != null)
                    {
                        ProductCategory productCategory = await _dbConext.ProductCategories.FirstOrDefaultAsync(p => p.ProductCategoryId.Equals(product.ProductCategoryId));

                        ProductViewModel productVM = new ProductViewModel
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            Description = product.Description,
                            Price = product.Price,
                            IsAvailable = product.IsAvailable,
                            ProductCategoryId = product.ProductCategoryId,
                            ProductCategoryName = productCategory.ProductCategoryName
                        };
                        productsVM.Add(productVM);
                    }                
                }
                return productsVM;
            }
            return null;
        }

        public async Task DeleteProduct(string productId)
        {
            if (productId != null)
            {
                Product product = await _dbConext.Products.FirstOrDefaultAsync(p => p.ProductId.ToString().Equals(productId));

                _dbConext.Products.Remove(product);
                _dbConext.SaveChanges();
            }
        }
    }
}
