using GeekShopping.CartApi.Data.ValueObjects;

namespace GeekShopping.CartApi.Repository
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCouponByCouponCode(string code, string token);
    }
}
