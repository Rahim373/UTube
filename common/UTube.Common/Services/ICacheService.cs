namespace UTube.Common.Services
{
    /// <summary>
    /// Service to handle cache store
    /// </summary>
    public interface ICacheService
    {
        Task AddAsync<T>(string key, T value, CancellationToken cancellationToken = default);
        Task AddAsync<T>(string key, T value, TimeSpan? timeSpan, CancellationToken cancellationToken = default);
        ValueTask<T?> GetValueAsync<T>(string key, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
