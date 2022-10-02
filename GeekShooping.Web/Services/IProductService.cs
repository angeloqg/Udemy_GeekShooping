using GeekShooping.Web.Models;

namespace GeekShooping.Web.Services
{
    public interface IProductService
    {
        Task<List<ProductModel>?> FindAllProducts(string token);
        Task<ProductModel?> FindByIdProduct(long id, string token);
        Task<ProductModel?> CreateProduct(ProductModel model, string token);
        Task<ProductModel?> UpdateProduct(ProductModel model, string token);
        Task<bool> DeleteProductById(long id, string token);
    }
}
