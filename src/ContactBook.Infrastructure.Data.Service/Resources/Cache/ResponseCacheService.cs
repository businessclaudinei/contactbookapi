using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ContactBook.Infrastructure.Data.Service.Resources.Cache {
    public class ResponseCacheService : IResponseCacheService {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService (IDistributedCache distributedCache) {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync (string cacheKey, object response, TimeSpan timeTimeLive) {
            if (response == null) {
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject (response);

            await _distributedCache.SetStringAsync (cacheKey, serializedResponse, new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = timeTimeLive
            });
        }

        public async Task<string> GetCachedResponseAsync (string cacheKey) {
            var cachedResponse = await _distributedCache.GetStringAsync (cacheKey);
            return string.IsNullOrEmpty (cachedResponse) ? null : cachedResponse;
        }

        public async Task<string> ManageTokenAsync (string cacheKey, object response = null) {
            if (response == null) {
                response = await GetCachedResponseAsync (cacheKey);
            }

            var serializedResponse = JsonConvert.SerializeObject (response);

            await _distributedCache.SetStringAsync (cacheKey, serializedResponse, new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = new TimeSpan (0, 0, Convert.ToInt32 (Environment.GetEnvironmentVariable ("TOKEN_EXPIRATION_SECONDS")))
            });
            return await GetCachedResponseAsync (cacheKey);
        }
    }
}