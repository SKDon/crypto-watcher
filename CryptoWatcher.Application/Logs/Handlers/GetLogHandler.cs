﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CryptoWatcher.Application.Logs.Requests;
using CryptoWatcher.Application.Logs.Responses;
using CryptoWatcher.Domain.Messages;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Persistence.Repositories;
using CryptoWatcher.Shared.Exceptions;
using MediatR;

namespace CryptoWatcher.Application.Logs.Handlers
{
    public class GetLogHandler : IRequestHandler<GetLogRequest, LogResponse>
    {
        private readonly IRepository<Log> _logRepository;
        private readonly IMapper _mapper;

        public GetLogHandler(IRepository<Log> logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public async Task<LogResponse> Handle(GetLogRequest request, CancellationToken cancellationToken)
        {
            // Get log
            var log = await _logRepository.GetSingle(request.LogId);

            // Throw NotFound exception if the currency does not exist
            if (log == null) throw new NotFoundException(LogMessage.LogNotFound);

            // Response
            var response = _mapper.Map<LogResponse>(log);

            // Return
            return response;
        }
    }
}