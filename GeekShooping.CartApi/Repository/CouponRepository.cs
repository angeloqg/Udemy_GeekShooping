using GeekShooping.CartApi.Data.ValueObjects;
using GeekShooping.CartApi.Utils;
using System.Net.Http.Headers;

namespace GeekShooping.CartApi.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;
        private const string BasePath = "api/v1/Coupon";

        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentException(nameof(httpClient));
        }

        public async Task<CouponVO> GetCouponByCouponCode(string code, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{BasePath}/find-coupon/{code}");

            var result = await response.ReadContentAs<ResultVO>();

            if (result == null)
                return await Task.FromResult(new CouponVO());

            if (result.Success)
            {
                return await Task.FromResult(result.Data != null ? HttpClientExtensions.Desserialization<CouponVO>(result.Data) : new CouponVO());
            }
            else
            {
                return await Task.FromResult(new CouponVO());
            }
        }
    }
}
