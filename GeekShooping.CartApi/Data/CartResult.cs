﻿namespace GeekShooping.CartApi.Data
{
    public class CartResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
