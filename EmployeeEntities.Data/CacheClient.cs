using EmployeeEntities.Data.Config;
using EmployeeEntities.Data.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EmployeeEntities.Data;

public class CacheClient : ICacheClient
{
    private readonly IDistributedCache _cache;
    private readonly CacheSettings _options;

    public CacheClient(
        IDistributedCache cache,
        IOptions<CacheSettings> options
    )
    {
        _cache = cache;
        _options = options.Value;
    }

    public async Task ClearCache(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task<T?> GetFromCache<T>(string key) where T : class
    {
        var cachedResponse = await _cache.GetStringAsync(key);
        if (cachedResponse == null)
        {
            return null;
        }
        return JsonConvert.DeserializeObject<T>(cachedResponse);
    }

    public async Task SetCache<T>(string key, T value) where T : class
    {
        var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(_options.TimeToLive));
        var response = JsonConvert.SerializeObject(value);
        await _cache.SetStringAsync(key, response, options);
    }
}
