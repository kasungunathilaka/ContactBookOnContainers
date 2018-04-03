using OrderService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Providers
{
    public interface IProductService
    {
        Task CreateProduct(ProductViewModel addedProduct);
        Task<ProductViewModel> GetProductByName(string productName);
        Task<List<string>> GetAllProductNames();
        Task<List<string>> GetAllProductCategoryNames();
        Task<List<ProductViewModel>> GetAllProducts();
        Task DeleteProduct(string productId);
    }
}
