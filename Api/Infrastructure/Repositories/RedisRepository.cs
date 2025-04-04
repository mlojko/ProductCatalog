using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Api.Infrastructure.Repositories
{
    public class RedisRepository(IDistributedCache distributedCache) : IRedisRepository
    {
        
        private readonly IDistributedCache _database = distributedCache;

        public async Task SetString<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.SetStringAsync(key, json, options);
        }
        public async Task<T?> GetString<T>(string key)
        {
            var json = await _database.GetStringAsync(key);
            return !string.IsNullOrWhiteSpace(json) ? JsonSerializer.Deserialize<T>(json!) : default;
        }
        public async Task DeleteKey(string key)
        {
            await _database.RemoveAsync(key);
        }
    }
}
