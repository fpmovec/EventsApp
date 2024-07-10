using Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            var data = await _distributedCache.GetStringAsync(key, cancellationToken);

            return JsonConvert.DeserializeObject<T>(data ?? string.Empty);
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            string dataString = JsonConvert.SerializeObject(value);

            await _distributedCache.SetStringAsync(key, dataString, cancellationToken);
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
