using LazyCache;

namespace ShoppingCartManager.Infrastructure.Cache;

using ICacheProvider = Application.Cache.Abstractions.ICacheProvider;

public sealed class LazyCacheProvider(IAppCache cache) : ICacheProvider
{
    public async Task<T> GetOrAddAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan ttl
    )
    {
        return await cache.GetOrAddAsync(key, factory, ttl);
    }
}
