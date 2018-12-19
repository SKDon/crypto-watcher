﻿using CryptoWatcher.Application.Responses;
using MediatR;


namespace CryptoWatcher.Application.Requests
{
    public class AddIndicatorRequest : IRequest<IndicatorResponse>
    {
        public string IndicatorId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Formula { get; set; }
    }
}
