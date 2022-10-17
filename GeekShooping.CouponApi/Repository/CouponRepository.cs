using AutoMapper;
using GeekShopping.CouponApi.Data.ValueObjects;
using GeekShopping.CouponApi.Model.Context;

namespace GeekShopping.CouponApi.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly MySqlContext _context;
        private readonly IMapper _mapper;

        public CouponRepository(MySqlContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponVO> GetCouponByCouponCode(string? couponCode)
        {
            try
            {
                if (!String.IsNullOrEmpty(couponCode))
                {
                    var coupon = _context.Coupons.FirstOrDefault(c => c.CouponCode == couponCode);

                    return _mapper.Map<CouponVO>(coupon);
                }
                else
                {
                    return await Task.FromResult(new CouponVO());
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(new CouponVO());
            }
        }
    }
}
