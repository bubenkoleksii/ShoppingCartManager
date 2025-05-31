using ShoppingCartManager.Application.Cache.Abstractions;
using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Application.Common.Errors;
using ShoppingCartManager.Application.Product.Abstractions;
using ShoppingCartManager.Application.Statistics.Abstractions;
using ShoppingCartManager.Application.Statistics.Models;
using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.Statistics.Implementations;

public sealed class StatisticsService(
    IProductQueries productQueries,
    ICategoryQueries categoryQueries,
    IStoreQueries storeQueries,
    ICacheProvider cache,
    IUserContext userContext,
    ILogger<StatisticsService> logger
) : IStatisticsService
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(3);

    public async Task<Either<Error, StatisticsResponse>> GetStatistics(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
    )
    {
        var userId = userContext.UserId;

        if (userId is null)
        {
            logger.LogWarning("Statistics request failed: user not found in context");
            return new UserNotFoundError();
        }

        var cacheKey = $"statistics_{userId}_{from:yyyyMMdd}_{to:yyyyMMdd}";

        return await cache.GetOrAddAsync(cacheKey, factory, CacheTtl);

        async Task<StatisticsResponse> factory()
        {
            logger.LogInformation(
                "Generating statistics for user {UserId} from {From} to {To}",
                userId,
                from,
                to
            );

            var products = await productQueries.GetStats(userId.Value, from, to, cancellationToken);

            var totalProducts = products.Count;
            var productsInCart = products.Count(p => p.IsInCart);

            var categories = await categoryQueries.Get(userId.Value, cancellationToken);

            var stores = await storeQueries.Get(userId.Value, cancellationToken);

            var categoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

            var storeMap = stores.ToDictionary(s => s.Id, s => s.Name);

            var categoryBreakdown = products
                .GroupBy(p => p.CategoryId!.Value)
                .Select(group => new CategoryBreakdownItem(
                    categoryId: group.Key,
                    categoryName: categoryMap[group.Key],
                    totalPrice: group.Sum(p => p.Price),
                    inCartPrice: group.Where(p => p.IsInCart).Sum(p => p.Price)
                ));

            var productBreakdown = products.Select(p => new ProductBreakdownItem(
                productId: p.Id,
                productName: p.Name,
                price: p.Price,
                isInCart: p.IsInCart,
                categoryId: p.CategoryId!.Value,
                categoryName: categoryMap[p.CategoryId!.Value],
                storeId: p.StoreId,
                storeName: p.StoreId is null ? null : storeMap[p.StoreId.Value]
            ));

            return new StatisticsResponse(
                from: from,
                to: to,
                totalProducts: totalProducts,
                productsInCart: productsInCart,
                categoryBreakdown: categoryBreakdown,
                productBreakdown: productBreakdown
            );
        }
    }
}
