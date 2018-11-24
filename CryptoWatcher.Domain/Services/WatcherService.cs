﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoWatcher.Domain.Builders;
using CryptoWatcher.Domain.Messages;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Domain.Repositories;
using CryptoWatcher.Shared.Exceptions;

namespace CryptoWatcher.Domain.Services
{
    public class WatcherService
    {
        private readonly IWatcherRepository _watcherRepository;
        private readonly IUserRepository _userRepository;
        private readonly CacheService _cacheService;

        public WatcherService(
            IWatcherRepository watcherRepository,
            IUserRepository userRepository,
            CacheService cacheService)
        {
            _watcherRepository = watcherRepository;
            _userRepository = userRepository;
            _cacheService = cacheService;
        }

        public async Task<List<Watcher>> GetUserWatchers(string userId)
        {
            // Get user
            var user = await _userRepository.GetByUserId(userId);

            // Throw NotFound exception if it does not exist
            if (user == null) throw new NotFoundException(UserMessages.UserNotFound);

            // Get user watchers
            var userWatchers = await _watcherRepository.GetByUserId(userId);

            // Get currencies
            var currencies = await _cacheService.GetFromCache<Currency>();

            // Collect percentages
            var percentages = new decimal[currencies.Count];
            for (var i = 0; i < currencies.Count; i++)
            {
                percentages[i] = currencies[i].CurrencyPercentageChange24H;
            }

            // Add price change watcher
            foreach (var currency in currencies)
            {
                // Price watcher
                var priceChangeWatcher = new Watcher(
                    userId,
                    currency.CurrencyId,
                    WatcherType.PriceChange,
                    currency.CurrencyPercentageChange24H,
                    new WatcherSettings(5, 5),
                    new WatcherSettings(0, 0),
                    false);
                userWatchers.Add(priceChangeWatcher);
            }

            // Calculate all hypes
            WatcherBuilders.BuildHypes(percentages);

            // Add hype watcher
            var index = 0;
            foreach (var currency in currencies)
            {
                // Hype watcher
                var hypeWatcher = new Watcher(
                    userId,
                    currency.CurrencyId,
                    WatcherType.Hype,
                    percentages[index],
                    new WatcherSettings(5, 5),
                    new WatcherSettings(0, 0),
                    false);
                userWatchers.Add(hypeWatcher);
                index++;
            }

            // Return
            return userWatchers;
        }
        public async Task<Watcher> GetWatcher(string watcherId)
        {
            // Get watcher by id
            var watcher = await _watcherRepository.GetById(watcherId);

            // Throw NotFound exception if it does not exist
            if (watcher == null) throw new NotFoundException(WatcherMessages.WatcherNotFound);

            // Return
            return watcher;
        }
        public void AddWatcher(Watcher watcher)
        {
            // Add watcher
            _watcherRepository.Add(watcher);
        }
        public async Task<Watcher> UpdateWatcherSettings(string watcherId, decimal buyAt, decimal sellAt)
        {
            // Get watcher by id
            var watcher = await _watcherRepository.GetById(watcherId);

            // Throw NotFound exception if it does not exist
            if (watcher == null) throw new NotFoundException(WatcherMessages.WatcherNotFound);

            // Update settings
            var settings = new WatcherSettings(buyAt,sellAt);
            watcher.UpdateSettings(settings);

            // Return
            return watcher;
        }
    }
}