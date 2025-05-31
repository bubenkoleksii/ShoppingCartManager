using ShoppingCartManager.Application.Cache.Abstractions;
using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Application.Product.Abstractions;
using ShoppingCartManager.Application.Statistics.Implementations;
using ShoppingCartManager.Application.Statistics.Models;
using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.Tests.Statistics;

using Category = Domain.Entities.Category;
using Product = Domain.Entities.Product;
using Store = Domain.Entities.Store;

public class StatisticsServiceTests
{
    private readonly Mock<IProductQueries> _productQueries = new();
    private readonly Mock<ICategoryQueries> _categoryQueries = new();
    private readonly Mock<IStoreQueries> _storeQueries = new();
    private readonly Mock<IUserContext> _userContext = new();
    private readonly Mock<ILogger<StatisticsService>> _logger = new();
    private readonly Mock<ICacheProvider> _cacheProvider = new();

    private readonly Guid _userId = Guid.NewGuid();

    public StatisticsServiceTests()
    {
        _userContext.Setup(x => x.UserId).Returns(_userId);
    }

    [Fact]
    public async Task GetStatistics_ReturnsExpectedStatistics()
    {
        // Arrange
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 12, 31);
        var categoryId = Guid.NewGuid();
        var storeId = Guid.NewGuid();

        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Milk",
                Price = 30,
                IsInCart = true,
                CategoryId = categoryId,
                StoreId = storeId,
                UserId = _userId,
            },
        };

        var categories = new List<Category>
        {
            new()
            {
                Id = categoryId,
                Name = "Dairy",
                UserId = _userId,
            },
        };

        var stores = new List<Store>
        {
            new()
            {
                Id = storeId,
                Name = "Supermarket",
                UserId = _userId,
            },
        };

        _productQueries
            .Setup(p => p.GetStats(_userId, from, to, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);
        _categoryQueries
            .Setup(c => c.Get(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        _storeQueries
            .Setup(s => s.Get(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stores);

        StatisticsResponse? cached = null;
        _cacheProvider
            .Setup(c =>
                c.GetOrAddAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<StatisticsResponse>>>(),
                    It.IsAny<TimeSpan>()
                )
            )
            .Returns<string, Func<Task<StatisticsResponse>>, TimeSpan>(
                async (key, factory, ttl) =>
                {
                    cached ??= await factory();
                    return cached;
                }
            );

        var service = new StatisticsService(
            _productQueries.Object,
            _categoryQueries.Object,
            _storeQueries.Object,
            _cacheProvider.Object,
            _userContext.Object,
            _logger.Object
        );

        // Act
        var result = await service.GetStatistics(from, to);

        // Assert
        Assert.True(result.IsRight);
        var response = result.Match(
            Right: r => r,
            Left: _ => throw new Exception("Expected Right but got Left")
        );

        Assert.Equal(1, response.TotalProducts);
        Assert.Equal(1, response.ProductsInCart);
        Assert.Single(response.CategoryBreakdown);
        Assert.Single(response.ProductBreakdown);
    }

    [Fact]
    public async Task GetStatistics_ShouldUseCache_AndOnlyQueryOnce()
    {
        // Arrange
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 12, 31);
        var categoryId = Guid.NewGuid();
        var storeId = Guid.NewGuid();

        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Milk",
                Price = 30,
                IsInCart = true,
                CategoryId = categoryId,
                StoreId = storeId,
                UserId = _userId,
            },
        };

        var categories = new List<Category>
        {
            new()
            {
                Id = categoryId,
                Name = "Dairy",
                UserId = _userId,
            },
        };

        var stores = new List<Store>
        {
            new()
            {
                Id = storeId,
                Name = "Supermarket",
                UserId = _userId,
            },
        };

        _productQueries
            .Setup(p => p.GetStats(_userId, from, to, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);
        _categoryQueries
            .Setup(c => c.Get(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        _storeQueries
            .Setup(s => s.Get(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stores);

        StatisticsResponse? cached = null;
        _cacheProvider
            .Setup(c =>
                c.GetOrAddAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<StatisticsResponse>>>(),
                    It.IsAny<TimeSpan>()
                )
            )
            .Returns<string, Func<Task<StatisticsResponse>>, TimeSpan>(
                async (key, factory, ttl) =>
                {
                    cached ??= await factory();
                    return cached;
                }
            );

        var service = new StatisticsService(
            _productQueries.Object,
            _categoryQueries.Object,
            _storeQueries.Object,
            _cacheProvider.Object,
            _userContext.Object,
            _logger.Object
        );

        // Act
        var result1 = await service.GetStatistics(from, to);
        var result2 = await service.GetStatistics(from, to);

        // Assert
        Assert.True(result1.IsRight);
        Assert.True(result2.IsRight);

        _productQueries.Verify(
            p => p.GetStats(_userId, from, to, It.IsAny<CancellationToken>()),
            Times.Once
        );
        _categoryQueries.Verify(c => c.Get(_userId, It.IsAny<CancellationToken>()), Times.Once);
        _storeQueries.Verify(s => s.Get(_userId, It.IsAny<CancellationToken>()), Times.Never);
        _cacheProvider.Verify(c => c.GetOrAddAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<StatisticsResponse>>>(),
            It.IsAny<TimeSpan>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetStatistics_ReturnsError_WhenUserNotFound()
    {
        // Arrange
        var from = DateTime.UtcNow;
        var to = from.AddDays(1);
        _userContext.Setup(x => x.UserId).Returns((Guid?)null);

        var service = new StatisticsService(
            _productQueries.Object,
            _categoryQueries.Object,
            _storeQueries.Object,
            _cacheProvider.Object,
            _userContext.Object,
            _logger.Object
        );

        // Act
        var result = await service.GetStatistics(from, to);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }
}
