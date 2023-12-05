using Microsoft.Extensions.Caching.Distributed;

namespace EmployeeEntities.Data.Interface;
public interface ICacheClient
{
    Task<T?> GetFromCache<T>(string key) where T : class;
    Task SetCache<T>(string key, T value) where T : class;
    Task ClearCache(string key);
}