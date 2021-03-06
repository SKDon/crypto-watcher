﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CryptoWatcher.Domain.Builders;
using CryptoWatcher.Domain.Expressions;
using Hangfire;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Persistence.Repositories;
using CryptoWatcher.Persistence.Contexts;
using CryptoWatcher.Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.BackgroundJobs
{
    public class UpdateLinesJob
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ILogger<UpdateLinesJob> _logger;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<Indicator> _indicatorRepository;
        private readonly IRepository<IndicatorDependency> _indicatorDependencyRepository;
        private readonly IRepository<Watcher> _watcherRepository;
        private readonly IRepository<DataPoint> _lineRepository;

        public UpdateLinesJob(
            MainDbContext mainDbContext,
            ILogger<UpdateLinesJob> logger,
            IRepository<Currency> currencyRepository,
            IRepository<Indicator> indicatorRepository,
            IRepository<IndicatorDependency> indicatorDependencyRepository,
            IRepository<Watcher> watcherRepository,
            IRepository<DataPoint> lineRepository)
        {
            _mainDbContext = mainDbContext;
            _logger = logger;
            _currencyRepository = currencyRepository;
            _indicatorRepository = indicatorRepository;
            _indicatorDependencyRepository = indicatorDependencyRepository;
            _watcherRepository = watcherRepository;
            _lineRepository = lineRepository;
        }

        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Run()
        {
            try
            {
                // Start watch

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Get all currencies
                var currencies = await _currencyRepository.GetAll();

                // Get all indicators
                var indicators = await _indicatorRepository.GetAll();

                // Set all indicators dependencies
                await SetIndicatorDependencies(indicators);

                // Get non-default watchers with buy or sell
                var watchers = await _watcherRepository.GetAll(WatcherExpression.WatcherWillingToBuyOrSell());

                // Set current lines as no longer current
                await SetCurrentLinesAsNoLongerCurrent();

                // Build new lines
                var lines = LineBuilder.BuildLines(currencies, indicators, watchers);

                // Set new lines
                _lineRepository.AddRange(lines);

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Stop watch
                stopwatch.Stop();

                // Log into Splunk
                _logger.LogSplunkJob(new
                {
                    lines.Count,
                    ExecutionTime = stopwatch.Elapsed.TotalSeconds
                });

                // Return
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Log into Splunk 
                _logger.LogSplunkJob(new
                {
                    JobFailed = ex.Message
                });

                // Log error into Splunk
                _logger.LogSplunkError(ex);
            }
        }

        private async Task SetIndicatorDependencies(List<Indicator> indicators)
        {
            foreach (var indicator in indicators)
            {
                var dependencies = await _indicatorDependencyRepository.GetAll(IndicatorDependencyExpression.IndicatorDependencyFilter(indicator.IndicatorId, null));
                indicator.SetDependencies(dependencies);
            }
        }
        private async Task SetCurrentLinesAsNoLongerCurrent()
        {
            // Get current lines
            var currentLines = await _lineRepository.GetAll(LineExpression.CurrentLine());

            // Set as no longer current
            LineBuilder.SetLinesAsNoLongerCurrent(currentLines);

            // Update
            _lineRepository.UpdateRange(currentLines);
        }
    }
}