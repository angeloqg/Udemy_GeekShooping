using GeekShooping.CouponApi.Data;
using GeekShooping.CouponApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.CouponApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _repository;
        private CouponResult _result { get; set; }

        public CouponController(ICouponRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            _result = new CouponResult();
        }

        [HttpGet("find-coupon/{couponcode}")]
        public async Task<IActionResult> FindCoupon(string? couponcode)
        {
            var coupom = await _repository.GetCouponByCouponCode(couponcode);

            if (coupom != null)
            {
                _result = new CouponResult
                {
                    Success = true,
                    Message = "Coupom encontrado com sucesso",
                    Data = coupom
                };

                return Ok(_result);
            }
            else
            {
                _result = new CouponResult
                {
                    Success = false,
                    Message = "Nenhum coupon encontrado",
                    Data = null
                };

                return Ok(_result);
            }
        }
    }
}
