namespace UTube.Common.Services
{
    public interface ICacheService
    {
        public Task AddAsync<T>(string key, T value, CancellationToken cancellationToken = default);
        ValueTask<T?> GetValueAsync<T>(string key, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
