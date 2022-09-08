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
        public ProductController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if(await _repository.FindById(id) == null)
            {
                return NotFound(new ProductVO());
            }
            else
            {
                return Ok(await _repository.FindById(id));
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var products = await _repository.FindAll();

            if (products.Any())
            {
                return Ok(products);
            }
            else
            {
                return NotFound(new List<ProductVO>());
            }            
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ProductCreateVO vo)
        {
            if(vo == null)
            {
                return BadRequest(await Task.FromResult(new ProductVO { Id = 0 }));
            }
            else
            {
                var product = _mapper.Map<ProductVO>(vo);
                return Ok(await _repository.Create(product));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ProductVO vo)
        {
            if (vo == null)
                return BadRequest(await Task.FromResult(new ProductVO()));

            return Ok(await _repository.Update(vo));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return BadRequest(await Task.FromResult(false));

            return Ok(await _repository.Delete(id.GetValueOrDefault()));
        }
    }
}
