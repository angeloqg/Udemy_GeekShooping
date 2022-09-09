using GeekShooping.Web.Models;
using GeekShooping.Web.Utils;

namespace GeekShooping.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private const string BasePath = "api/v1/Product";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentException(nameof(httpClient));
        }

        public async Task<IEnumerable<ProductModel>> FindAllProducts()
        {
            var response = await _httpClient.GetAsync(BasePath);

            var result = await response.ReadContentAs<ProductResultModel>();

            if (result == null)
                return await Task.FromResult(new List<ProductModel>());

            if (result.Success)
            {
                return await Task.FromResult(result.Data != null ?(List<ProductModel>)result.Data : new List<ProductModel>());
            }
            else
            {
                return await Task.FromResult(new List<ProductModel>());
            }
        }

        public async Task<ProductModel> FindByIdProduct(long id)
        {
            var response = await _httpClient.GetAsync($"{BasePath}/{id}");

            var result = await response.ReadContentAs<ProductResultModel>();

            if (result == null)
                return await Task.FromResult(new ProductModel());

            if (result.Success)
            {
                return await Task.FromResult(result.Data != null ? (ProductModel)result.Data : new ProductModel());
            }
            else
            {
                return await Task.FromResult(new ProductModel());
            }
        }

        public async Task<ProductModel> CreateProduct(ProductModel model)
        {
            var response = await _httpClient.PostAsJson(BasePath, model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ProductResultModel>();

                if (result == null)
                    return await Task.FromResult(new ProductModel());

                if (result.Success)
                {
                    return await Task.FromResult(result.Data != null ? (ProductModel)result.Data : new ProductModel());
                }
                else
                {
                    return await Task.FromResult(new ProductModel());
                }
            }
            else
            {
                throw new Exception("Somenthing went wrong calling API");
            }           
        }

        public async Task<ProductModel> UpdateProduct(ProductModel model)
        {
            var response = await _httpClient.PutAsJson(BasePath, model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ProductResultModel>();

                if (result == null)
                    return await Task.FromResult(new ProductModel());

                if (result.Success)
                {
                    return await Task.FromResult(result.Data != null ? (ProductModel)result.Data : new ProductModel());
                }
                else
                {
                    return await Task.FromResult(new ProductModel());
                }
            }
            else
            {
                throw new Exception("Somenthing went wrong calling API");
            }
        }

        public async Task<bool> DeleteProductById(long id)
        {
            var response = await _httpClient.DeleteAsync($"{BasePath}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ProductResultModel>();

                if (result == null)
                    return await Task.FromResult(false);

                if (result.Success)
                {
                    return await Task.FromResult(result.Data != null ? (bool)result.Data : false);
                }
                else
                {
                    return await Task.FromResult(false);
                }
            }
            else
            {
                throw new Exception("Somenthing went wrong calling API");
            }      
        }
    }
}
