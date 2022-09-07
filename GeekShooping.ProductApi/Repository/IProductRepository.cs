using GeekShooping.ProductApi.Data.ValueObjects;

namespace GeekShooping.ProductApi.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductVO>> FindAll();
        Task<ProductVO> FindById(long id);
        Task<ProductVO?> Create(ProductVO vo);
        Task<ProductVO?> Update(ProductVO vo);
        Task<bool> Delete(long id);
    }
}
