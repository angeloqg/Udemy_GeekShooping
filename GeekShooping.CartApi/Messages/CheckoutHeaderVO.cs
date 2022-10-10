using GeekShooping.CartApi.Data.ValueObjects;

namespace GeekShooping.CartApi.Messages
{
    public class CheckoutHeaderVO
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateTime { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? CardNumber { get; set; }
        public string? Cvv { get; set; }
        public int CarTotalItens { get; set; }
        public IEnumerable<CartDetailVO>? CartDetails { get; set; }
    }
}
