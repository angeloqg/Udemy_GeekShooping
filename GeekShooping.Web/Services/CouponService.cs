using GeekShopping.Web.Models;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly HttpClient _httpClient;
        private const string BasePath = "api/v1/Coupon";

        public CouponService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentException(nameof(httpClient));
        }


        public async Task<CouponViewModel> GetCoupon(string code, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{BasePath}/find-coupon/{code}");

            var result = await response.ReadContentAs<ResultViewModel>();

            if (result == null)
                return await Task.FromResult(new CouponViewModel());

            if (result.Success)
            {
                return await Task.FromResult(result.Data != null ? HttpClientExtensions.Desserialization<CouponViewModel>(result.Data) : new CouponViewModel());
            }
            else
            {
                return await Task.FromResult(new CouponViewModel());
            }
        }
    }
}
