using AutoMapper;
using GeekShopping.ProductApi.Data.ValueObjects;
using GeekShopping.ProductApi.Model;
using GeekShopping.ProductApi.Model.Context;
using GeekShopping.ProductApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySqlContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(MySqlContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductVO>> FindAll()
        {
            try
            {
                List<Product> products = new List<Product>();

                if (await _context.Products.AnyAsync())
                {
                    products = await _context.Products.AsNoTracking().ToListAsync();
                }

                return _mapper.Map<List<ProductVO>>(products);

            }
            catch (Exception)
            {
                return _mapper.Map<List<ProductVO>>(new List<Product>());
            }
        }

        public async Task<ProductVO> FindById(long id)
        {
            try
            {
                Product product = new Product();

                if(await _context.Products.AnyAsync(p => p.Id == id))
                {
                    product = await _context.Products
                                    .AsNoTracking()
                                    .FirstAsync(p => p.Id == id);
                }

                return _mapper.Map<ProductVO>(product);
            }
            catch (Exception)
            {
                return _mapper.Map<ProductVO>(new Product());
            }
        }

        public async Task<ProductVO?> Create(ProductVO vo)
        {
            if (vo == null)
                return null;

            Product product = _mapper.Map<Product>(vo);

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                product.Id = 0;
            }

            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductUpdateVO?> Update(ProductVO vo)
        {
            if (vo == null)
                return null;

            Product product = _mapper.Map<Product>(vo);

            try
            {
                var oldProduct = await FindById(vo.Id);
                _context.Products.Update(product);

                await _context.SaveChangesAsync();

                oldProduct.Price = Convert.ToDecimal(oldProduct.Price.ToString("F2"));
                vo.Price = Convert.ToDecimal(vo.Price.ToString("F2"));

                var result = _mapper.Map<ProductUpdateVO>(product);
                result.Status = Compare.ObjectTo(oldProduct, vo);

                return result;
            }
            catch (Exception)
            {
                var result = _mapper.Map<ProductUpdateVO>(product);
                result.Status = false;
                return result;
            }
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                if (await _context.Products.AnyAsync(p => p.Id == id))
                {
                    Product product = await _context.Products.FirstAsync(p => p.Id == id);

                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }       
    }
}
