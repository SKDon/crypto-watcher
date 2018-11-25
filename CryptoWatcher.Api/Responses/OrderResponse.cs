﻿


using CryptoWatcher.Domain.Models;

namespace CryptoWatcher.Api.Responses
{
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public decimal Quantity { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}