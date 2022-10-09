using GeekShooping.Web.Models;
using GeekShooping.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await FindUserCart());
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartViewModel model)
        {
            string? token = await HttpContext.GetTokenAsync("access_token");
            string? userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            bool response = await _cartService.ApplyCoupon(model, token);

            if (response)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon()
        {
            string? token = await HttpContext.GetTokenAsync("access_token");
            string? userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            bool response = await _cartService.RemoveCoupon(userId, token);

            if (response)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }


        public async Task<IActionResult> Remove(int id)
        {
            string? token = await HttpContext.GetTokenAsync("access_token");
            string? userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            bool response = await _cartService.RemoveFromCart(id, token);

            if (response)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        private async Task<CartViewModel?> FindUserCart()
        {
            string? token = await HttpContext.GetTokenAsync("access_token");
            string? userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            CartViewModel response = await _cartService.FindCartByUserId(userId, token);

            if (response?.CartHeader != null)
            {
                if(!String.IsNullOrEmpty(response.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon(response.CartHeader.CouponCode, token);

                    if(coupon?.CouponCode != null)
                    {
                        response.CartHeader.DescountTotal = coupon.DiscountAmount;
                    }
                }

                foreach (var detail in response.CartDetails)
                {
                    response.CartHeader.PurchaseAmount += (detail.Product.Price * detail.Count);
                }

                response.CartHeader.PurchaseAmount -= response.CartHeader.DescountTotal;
            }

            return response;
        }
    }
}
