using GeekShopping.CartApi.Data;
using GeekShopping.CartApi.Data.ValueObjects;
using GeekShopping.CartApi.Messages;
using GeekShopping.CartApi.RabbitMQSender;
using GeekShopping.CartApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

        private CartResult _result { get; set; }

        public CartController(ICartRepository cartRepository,
                              ICouponRepository couponRepository,
                              IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _cartRepository = cartRepository ?? throw new ArgumentException(nameof(cartRepository));
            _couponRepository = couponRepository ?? throw new ArgumentException(nameof(couponRepository));
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentException(nameof(rabbitMQMessageSender));
            _result = new CartResult();
        }

        [HttpGet("find-cart/{id}")]
        public async Task<IActionResult> FindById(string id)
        {
            var cart = await _cartRepository.FindCartUserId(id);

            if(cart.CartDetails != null && cart.CartHeader != null)
            {
                _result = new CartResult
                {
                    Success = true,
                    Message = "Carrinho encontrado com sucesso",
                    Data = cart
                };

                return Ok(_result);
            }
            else
            {
                _result = new CartResult { 
                   Success = false,
                   Message = "Nenhum carrinho encontrado",
                   Data = null               
                };

                return Ok(_result);
            }           
        }

        [HttpPost("add-cart")]
        public async Task<IActionResult> AddCart([FromBody] CartVO cartVO)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(cartVO);

            if (cart.CartDetails != null && cart.CartHeader != null)
            {
                _result = new CartResult
                {
                    Success = true,
                    Message = "Carrinho salvo com sucesso",
                    Data = cart
                };

                return Ok(_result);
            }
            else
            {
                _result = new CartResult
                {
                    Success = false,
                    Message = "Erro ao salvar o carrinho",
                    Data = null
                };

                return BadRequest(_result);
            }
        }

        [HttpPut("update-cart")]
        public async Task<IActionResult> UpdateCart([FromBody] CartVO cartVO)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(cartVO);

            if (cart.CartDetails != null && cart.CartHeader != null)
            {
                _result = new CartResult
                {
                    Success = true,
                    Message = "Carrinho atualizado com sucesso",
                    Data = cart
                };

                return Ok(_result);
            }
            else
            {
                _result = new CartResult
                {
                    Success = false,
                    Message = "Erro ao atualizar o carrinho",
                    Data = null
                };

                return BadRequest(_result);
            }
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<IActionResult> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveFromCart(id);

            if (status)
            {
                _result = new CartResult
                {
                    Success = true,
                    Message = "Carrinho salvo com sucesso",
                    Data = status
                };

                return Ok(_result);
            }
            else
            {
                _result = new CartResult
                {
                    Success = false,
                    Message = "Falha ao remover o carrinho",
                    Data = null
                };

                return BadRequest(_result);
            }
        }

        [HttpPost("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] CartVO vo)
        {
            var status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);

            if (status)
            {
                _result = new CartResult
                {
                    Success = true,
                    Message = "Coupon aplicado com sucesso",
                    Data = status
                };

                return Ok(_result);
            }
            else
            {
                _result = new CartResult
                {
                    Success = false,
                    Message = "Falha ao aplicar coupon",
                    Data = null
                };

                return BadRequest(_result);
            }
        }


        [HttpDelete("remove-coupon/{userId}")]
        public async Task<IActionResult> RemoceCoupon(string userId)
        {
            var status = await _cartRepository.RemoveCoupon(userId);

            if (status)
            {
                _result = new CartResult
                {
                    Success = true,
                    Message = "Coupon removido com sucesso",
                    Data = status
                };

                return Ok(_result);
            }
            else
            {
                _result = new CartResult
                {
                    Success = false,
                    Message = "Falha ao remover coupon",
                    Data = null
                };

                return BadRequest(_result);
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutHeaderVO vo)
        {
            string token = Request.Headers["Authorization"];
            if(vo?.UserId == null)
            {
                _result = new CartResult
                {
                    Success = false,
                    Message = "Falha ao efetuar o checkout, usuário não logado",
                    Data = null
                };

                return BadRequest(_result);
            }

            var cart = await _cartRepository.FindCartUserId(vo.UserId);

            if (cart.CartDetails != null && cart.CartHeader != null)
            {
                if (!String.IsNullOrEmpty(vo.CouponCode))
                {
                    CouponVO coupon = await _couponRepository.GetCouponByCouponCode(vo.CouponCode, token);
                
                    if(vo.DiscountAmount != coupon.DiscountAmount)
                    {
                        _result = new CartResult
                        {
                            Success = false,
                            Message = "Coupon não encontrado",
                            Data = null
                        };

                        return StatusCode(412, _result);
                    }               
                }

                vo.CartDetails = cart.CartDetails;
                vo.DateTime = DateTime.Now;

                // RabbitMQ logic comes here!!!
                _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

                await _cartRepository.ClearCart(vo.UserId);

                _result = new CartResult
                {
                    Success = true,
                    Message = "Checkout efetuado com sucesso",
                    Data = vo
                };

                return Ok(_result);
            }
            else
            {
                _result = new CartResult
                {
                    Success = false,
                    Message = "Falha ao efetuar o checkout",
                    Data = null
                };

                return BadRequest(_result);
            }
        }
    }
}
