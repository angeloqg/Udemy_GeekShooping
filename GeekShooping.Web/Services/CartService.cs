using GeekShooping.Web.Models;
using GeekShooping.Web.Utils;
using System.Net.Http.Headers;
using System.Reflection;

namespace GeekShooping.Web.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private const string BasePath = "api/v1/Cart";

        public CartService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentException(nameof(httpClient));
        }

        public async Task<CartViewModel> FindCartByUserId(string? userId, string? token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{BasePath}/find-cart/{userId}");

            var result = await response.ReadContentAs<ResultViewModel>();

            if (result == null)
                return await Task.FromResult(new CartViewModel());

            if (result.Success)
            {
                return await Task.FromResult(result.Data != null ? HttpClientExtensions.Desserialization<CartViewModel>(result.Data) : new CartViewModel());
            }
            else
            {
                return new CartViewModel();
            }
        }

        public async Task<CartViewModel> AddItemToCart(CartViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJson($"{BasePath}/add-cart", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ResultViewModel>();

                if (result == null)
                    return await Task.FromResult(new CartViewModel());

                if (result.Success)
                {
                    return await Task.FromResult(result.Data != null ? HttpClientExtensions.Desserialization<CartViewModel>(result.Data) : new CartViewModel());
                }
                else
                {
                    return await Task.FromResult(new CartViewModel());
                }
            }
            else
            {
                throw new Exception("Somenthing went wrong calling API");
            }
        }

        public async Task<CartViewModel> UpdateItemToCart(CartViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJson($"{BasePath}/update-cart", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ResultViewModel>();

                if (result == null)
                    return await Task.FromResult(new CartViewModel());

                if (result.Success)
                {
                    return await Task.FromResult(result.Data != null ? HttpClientExtensions.Desserialization<CartViewModel>(result.Data) : new CartViewModel());
                }
                else
                {
                    return await Task.FromResult(new CartViewModel());
                }
            }
            else
            {
                throw new Exception("Somenthing went wrong calling API");
            }
        }

        public async Task<bool> RemoveFromCart(int cartId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{BasePath}/remove-cart/{cartId}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ResultViewModel>();

                if (result == null)
                    return await Task.FromResult(false);

                if (result.Success)
                {
                    return await Task.FromResult(result.Success);
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

        public async Task<bool> ApplyCoupon(CartViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"{BasePath}/apply-coupon", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ResultViewModel>();

                if (result == null)
                    return await Task.FromResult(false);

                if (result.Success)
                {
                    return await Task.FromResult(result.Success);
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

        public async Task<bool> RemoveCoupon(string userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{BasePath}/remove-coupon/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ResultViewModel>();

                if (result == null)
                    return await Task.FromResult(false);

                if (result.Success)
                {
                    return await Task.FromResult(result.Success);
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

        public async Task<CartHeaderViewModel> Checkout(CartHeaderViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"{BasePath}/checkout", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadContentAs<ResultViewModel>();

                if (result == null)
                    return await Task.FromResult(new CartHeaderViewModel());

                if (result.Success)
                {
                    return await Task.FromResult(result.Data != null ? HttpClientExtensions.Desserialization<CartHeaderViewModel>(result.Data) : new CartHeaderViewModel());
                }
                else
                {
                    return await Task.FromResult(new CartHeaderViewModel());
                }
            }
            else
            {
                throw new Exception("Somenthing went wrong calling API");
            }
        }

        public async Task<bool> ClearCart(string userId, string token)
        {
            throw new NotImplementedException();
        }
    }
}
