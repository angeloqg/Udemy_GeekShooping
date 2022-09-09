using GeekShooping.Web.Models;

namespace GeekShooping.Web.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> FindAllProducts();
        Task<ProductModel> FindByIdProduct(long id);
        Task<ProductModel> CreateProduct(ProductModel model);
        Task<ProductModel> UpdateProduct(ProductModel model);
        Task<bool> DeleteProductById(long id);
    }
}
