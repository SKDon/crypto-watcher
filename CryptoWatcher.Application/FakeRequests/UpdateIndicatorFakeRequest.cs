﻿using CryptoWatcher.Application.Requests;

namespace CryptoWatcher.Application.FakeRequests
{
    public static class UpdateIndicatorFakeRequest
    {
        public static UpdateIndicatorRequest GetFake_RSI()
        {
            return new UpdateIndicatorRequest
            {
                IndicatorId = "rsi",
                Name = "RSI",
                Description = @"The Relative Strength Index (RSI) is a momentum oscillator that measures the speed and change of price movements.
                                RSI oscillates between zero and 100. Traditionally, and according to Wilder, RSI is considered overbought when above 70 and oversold when below 30.",
                Dependencies = new[] {"price", "price-change-24hrs" }
            };
        }       
    }
}
