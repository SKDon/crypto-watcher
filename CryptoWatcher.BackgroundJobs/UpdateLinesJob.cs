﻿using System;
using System.Threading.Tasks;
using CryptoWatcher.Domain.Builders;
using Hangfire;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Domain.Services;
using CryptoWatcher.Shared.Domain;
using CryptoWatcher.Persistence.Contexts;
using CryptoWatcher.Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.BackgroundJobs
{
    public class UpdateLinesJob
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ILogger<UpdateLinesJob> _logger;
        private readonly IRepository<Indicator> _indicatorRepository;
        private readonly CacheService _cacheService;
        public UpdateLinesJob(
            MainDbContext mainDbContext,
            ILogger<UpdateLinesJob> logger,
            IRepository<Indicator> indicatorRepository,
            CacheService cacheService)
        {
            _mainDbContext = mainDbContext;
            _logger = logger;
            _indicatorRepository = indicatorRepository;
            _cacheService = cacheService;
        }

        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Run()
        {
            try
            {
                // Get all currencies
                var currencies = await _cacheService.GetFromCache<Currency>(CacheKey.Currencies);

                // Get all indicators
                var indicators = await _indicatorRepository.GetAll();

                // Build lines
                var lines = LineBuilder.BuildLines(currencies, indicators);

                // Set lines
                await _cacheService.SetInCache(CacheKey.Lines, lines);

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Log into Splunk
                _logger.LogSplunkInformation(new
                {
                    LinesCreated = lines.Count
                });

                // Return
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
               // Log into Splunk 
                _logger.LogSplunkError(ex);
            }
        }
    }
}