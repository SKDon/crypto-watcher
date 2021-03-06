﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CryptoWatcher.Application.Responses;
using CryptoWatcher.Domain.Expressions;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Persistence.Repositories;

namespace CryptoWatcher.Application.Services
{
    public class LineService
    {
        private readonly IRepository<DataPoint> _lineRepository;
        private readonly IMapper _mapper;

        public LineService(IRepository<DataPoint> lineRepository, IMapper mapper)
        {
            _lineRepository = lineRepository;
            _mapper = mapper;
        }
        public async Task<List<DataPointResponse>> GetAllLines(string currencyId = null, IndicatorType? indicatorType = null, string indicatorId = null, string userId = null)
        {
            // Get all lines
            var lines = await _lineRepository.GetAll(LineExpression.LineFilter(currencyId, indicatorType, indicatorId, userId));

            // Response
            var response = _mapper.Map<List<DataPointResponse>>(lines);

            // Return
            return response;
        }
    }
}
