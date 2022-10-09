using GeekShooping.CartApi.Data;
using GeekShooping.CartApi.Data.ValueObjects;
using GeekShooping.CartApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.CartApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        private CartResult _result { get; set; }

        public CartController(ICartRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));

            _result = new CartResult();
        }

        [HttpGet("find-cart/{id}")]
        public async Task<IActionResult> FindById(string id)
        {
            var cart = await _repository.FindCartUserId(id);

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
            var cart = await _repository.SaveOrUpdateCart(cartVO);

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
            var cart = await _repository.SaveOrUpdateCart(cartVO);

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
            var status = await _repository.RemoveFromCart(id);

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
            var status = await _repository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);

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
            var status = await _repository.RemoveCoupon(userId);

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
    }
}
