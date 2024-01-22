using Newtonsoft.Json;
using StackExchange.Redis;

namespace UTube.Common.Services
{
    /// <summary>
    /// This service manage distibuted cache using Redis
    /// </summary>
    public class DistributedCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public DistributedCacheService(IConnectionMultiplexer connection)
        {
            _db = connection.GetDatabase();
        }

        public Task AddAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            return AddAsync(key, value, null, cancellationToken);
        }

        public async Task AddAsync<T>(string key, T value, TimeSpan? timeSpan, CancellationToken cancellationToken = default)
        {
            var str = JsonConvert.SerializeObject(value);
            await _db.StringSetAsync(new RedisKey(key), new RedisValue(str), timeSpan).ConfigureAwait(false);
        }

        public async Task DecrementAsync(string key, double decrementBy = 1, CancellationToken cancellationToken = default)
        {
            await _db.StringDecrementAsync(key, decrementBy, CommandFlags.FireAndForget);
        }

        public async ValueTask<T?> GetValueAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var str = await _db.StringGetAsync(key);

            if (string.IsNullOrEmpty(str))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(str);
        }

        public async Task IncrementAsync(string key, double incrementBy = 1, CancellationToken cancellationToken = default)
        {
            await _db.StringIncrementAsync(key, incrementBy, CommandFlags.FireAndForget);
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return _db.KeyDeleteAsync(new RedisKey(key));
        }
    }
}
