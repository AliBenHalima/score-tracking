using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ScoreTracking.App.Interfaces.Cache;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories.Cache
{
    public sealed class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ConcurrentDictionary<string, bool> keysList = new();

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
           string? value = await _distributedCache.GetStringAsync(key, cancellationToken);
            if(value == null)
            {
                return null;
            }

            var serializedValue = JsonConvert.DeserializeObject<T>(value);
            return serializedValue;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
        {
            T? cachedValue = await GetAsync<T>(key, cancellationToken);
            if (cachedValue is not null) return cachedValue;
            cachedValue = await factory();
            await SetAsync(key, cachedValue, cancellationToken);
            return cachedValue;

        }
        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, serializedValue, cancellationToken);
            keysList.TryAdd(key, true);
        }

        public async Task RemoveAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            keysList.TryRemove(key, out bool _);
        }

        public Task RemoveByPrefixAsync<T>(string prefix, CancellationToken cancellationToken = default) where T : class
        {
           IEnumerable<Task> tasks =  keysList.Keys.Where(k => k.StartsWith(prefix))
                         .Select((k) =>  RemoveAsync<T>(k, cancellationToken));
            return Task.WhenAll(tasks);
        }

    }
}
