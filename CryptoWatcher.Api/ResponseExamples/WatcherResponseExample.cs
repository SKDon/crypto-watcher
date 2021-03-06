﻿using CryptoWatcher.Application.FakeResponses;
using Swashbuckle.AspNetCore.Filters;

namespace CryptoWatcher.Api.ResponseExamples
{
    public class WatcherResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return WatcherFakeResponse.GetFake_master_Bitcoin_RSI();
        }
    }
    public class WatcherListResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return WatcherFakeResponse.GetFake_List();
        }
    }
}