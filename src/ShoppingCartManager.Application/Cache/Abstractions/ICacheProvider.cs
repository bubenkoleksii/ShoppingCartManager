namespace ShoppingCartManager.Application.Cache.Abstractions;

public interface ICacheProvider
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl);
}
