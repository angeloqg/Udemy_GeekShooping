using GeekShooping.CartApi.Data.ValueObjects;

namespace GeekShooping.CartApi.Repository
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCouponByCouponCode(string code, string token);
    }
}
