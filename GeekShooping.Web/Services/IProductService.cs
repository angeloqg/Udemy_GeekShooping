using GeekShooping.Web.Models;

namespace GeekShooping.Web.Services
{
    public interface IProductService
    {
        Task<List<ProductViewModel>?> FindAllProducts(string token);
        Task<ProductViewModel?> FindByIdProduct(long id, string token);
        Task<ProductViewModel?> CreateProduct(ProductViewModel model, string token);
        Task<ProductViewModel?> UpdateProduct(ProductViewModel model, string token);
        Task<bool> DeleteProductById(long id, string token);
    }
}
