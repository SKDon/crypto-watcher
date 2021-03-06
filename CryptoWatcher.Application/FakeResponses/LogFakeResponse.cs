﻿using System;
using System.Collections.Generic;
using CryptoWatcher.Application.Responses;

namespace CryptoWatcher.Application.FakeResponses
{
    public static class LogFakeResponse
    {
        public static LogResponse GetFake_Add_Indicator()
        {
            return new LogResponse
            {              
                LogId = Guid.NewGuid(),
                Action = "Add",
                Entity = "Indicator",
                EntityId = "master_price",
                Time = DateTime.Now,
                Json = "{}"
            };
        }
        public static List<LogResponse> GetFake_List()
        {
            return new List<LogResponse>
            {
                GetFake_Add_Indicator()
            };
        }
    }
}
