using GeekShooping.Web.Models;

namespace GeekShooping.Web.Services
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetCoupon(string code, string token);
    }
}
