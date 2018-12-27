﻿using CryptoWatcher.Application.System.Responses;
using MediatR;

namespace CryptoWatcher.Application.System.Requests
{
    public class GetVersionRequest : IRequest<VersionResponse>
    {
    }
}