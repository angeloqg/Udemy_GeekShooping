namespace GeekShopping.ProductApi.Data
{
    public class ProductResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
