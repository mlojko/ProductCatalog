using Microsoft.Extensions.Caching.Distributed;

namespace Api.Infrastructure.Repositories
{
    public interface IRedisRepository
    {
        Task SetString<T>(string key, T value, DistributedCacheEntryOptions options);
        Task<T?> GetString<T>(string key);
        Task DeleteKey(string key);
    }
}