using GeekShopping.OrderApi.Model;

namespace GeekShopping.OrderApi.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task<bool> UpdateOrderPaymentStatus(long orderHeaderId, bool paid);
    }
}
