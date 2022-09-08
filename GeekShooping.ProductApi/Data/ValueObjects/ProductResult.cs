namespace GeekShooping.ProductApi.Data.ValueObjects
{
    public class ProductResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
