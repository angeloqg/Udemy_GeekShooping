namespace GeekShooping.Web.Models
{
    public class CartViewModel
    {
        public CartHeaderViewModel? CartHeader { get; set; }
        public IEnumerable<CartDetailViewModel>? CartDetails { get; set; }

        public static implicit operator CartViewModel(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
