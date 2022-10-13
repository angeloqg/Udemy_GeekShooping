using GeekShooping.OrderApi.Model;
using GeekShooping.OrderApi.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShooping.OrderApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<MySqlContext> _context;

        public OrderRepository(DbContextOptions<MySqlContext> context)
        {
            _context = context;
        }

        public async Task<bool> AddOrder(OrderHeader header)
        {
            try
            {
                if(header == null)
                    return await Task.FromResult(false);

                await using var _db = new MySqlContext(_context);

                _db.OrderHeaders.Add(header);

                await _db.SaveChangesAsync();

                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> UpdateOrderPaymentStatus(long orderHeaderId, bool status)
        {
            try
            {
                await using var _db = new MySqlContext(_context);

                var header = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == orderHeaderId);

                if(header != null)
                {
                    header.PaymentStatus = status;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    return await Task.FromResult(false);
                }

                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
    }
}
