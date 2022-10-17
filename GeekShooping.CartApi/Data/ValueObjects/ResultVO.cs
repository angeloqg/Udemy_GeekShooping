namespace GeekShopping.CartApi.Data.ValueObjects
{
    public class ResultVO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
