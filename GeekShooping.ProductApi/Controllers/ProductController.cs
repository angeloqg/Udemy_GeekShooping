using AutoMapper;
using GeekShooping.ProductApi.Data.ValueObjects;
using GeekShooping.ProductApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.ProductApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        private ProductResult _result { get; set; }

        public ProductController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));

            _result = new ProductResult();
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if((await _repository.FindById(id)).Id == 0)
            {
                _result = new ProductResult
                {
                    Success = false,
                    Message = "Nenhum produto encontrado",
                    Data = null
                };

                return NotFound(_result);
            }
            else
            {
                _result = new ProductResult
                {
                    Success = true,
                    Message = "Produto encontrado com sucesso",
                    Data = await _repository.FindById(id)
                };
                return Ok(_result);
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var products = await _repository.FindAll();

            if (products.Any())
            {
                _result = new ProductResult
                {
                    Success = true,
                    Message = "Produtos encontrado com sucesso",
                    Data = products
                };

                return Ok(_result);
            }
            else
            {
                _result = new ProductResult
                {
                    Success = false,
                    Message = "Nenhum produto encontrado",
                    Data = null
                };

                return NotFound(_result);
            }            
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ProductCreateVO vo)
        {
            if(vo == null)
            {
                _result = new ProductResult
                {
                    Success = false,
                    Message = "Nenhum dado informado",
                    Data = null
                };
                return BadRequest(await Task.FromResult(_result));
            }
            else
            {
                var product = _mapper.Map<ProductVO>(vo);

                var result = await _repository.Create(product);

                if(result?.Id == 0)
                {
                    _result = new ProductResult
                    {
                        Success = false,
                        Message = "Nenhum dado cadastrado",
                        Data = null
                    };

                    return BadRequest(await Task.FromResult(_result));
                }
                else
                {
                    _result = new ProductResult
                    {
                        Success = true,
                        Message = "Dado cadastrado com sucesso",
                        Data = result
                    };
                    return Ok(await Task.FromResult(_result));
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ProductVO vo)
        {
            if (vo == null)
            {
                _result = new ProductResult
                {
                    Success = false,
                    Message = "Nenhum dado informado",
                    Data = null
                };
                return BadRequest(await Task.FromResult(_result));
            }
            else
            {
                var result = await _repository.Update(vo);

                if (result?.Status == true)
                {
                    _result = new ProductResult
                    {
                        Success = true,
                        Message = "Dado alterado com sucesso",
                        Data = result
                    };
                    return Ok(_result);
                }
                else
                {
                    _result = new ProductResult
                    {
                        Success = false,
                        Message = "Nenhum dado alterado",
                        Data = null
                    };
                    return Ok(await Task.FromResult(_result));
                }
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            var _resultDelete = new ProductResultDelete();

            if (id == null)
            {
                _resultDelete = new ProductResultDelete
                {
                    Success = false,
                    Message = "Nenhum id informado"
                };

                return BadRequest(await Task.FromResult(_resultDelete));
            }
            else
            {
                var result = await _repository.Delete(id.GetValueOrDefault());

                if (result)
                {
                    _resultDelete = new ProductResultDelete
                    {
                        Success = true,
                        Message = "Dado excluido com sucesso"
                    };

                    return Ok(await Task.FromResult(_resultDelete));
                }
                else
                {
                    _resultDelete = new ProductResultDelete
                    {
                        Success = false,
                        Message = "Nenhum dado excluido"
                    };

                    return BadRequest(await Task.FromResult(_resultDelete));
                }
            }
        }
    }
}
