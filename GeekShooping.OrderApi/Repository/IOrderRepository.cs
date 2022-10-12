using GeekShooping.OrderApi.Model;

namespace GeekShooping.OrderApi.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task<bool> UpdateOrderPaymentStatus(long orderHeaderId, bool paid);
    }
}
