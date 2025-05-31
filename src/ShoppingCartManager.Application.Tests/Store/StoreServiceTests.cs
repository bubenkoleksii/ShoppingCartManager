using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Application.Store.Errors;
using ShoppingCartManager.Application.Store.Implementations;
using ShoppingCartManager.Application.Store.Models;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.Tests.Store;

using Store = Domain.Entities.Store;

public class StoreServiceTests
{
    private readonly Mock<IStoreQueries> _queries = new();
    private readonly Mock<IStoreCommands> _commands = new();
    private readonly Mock<IUserContext> _context = new();
    private readonly Mock<ILogger<StoreService>> _logger = new();
    private readonly StoreService _service;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _storeId = Guid.NewGuid();
    private readonly Store _testStore;

    public StoreServiceTests()
    {
        _testStore = new Store
        {
            Id = _storeId,
            UserId = _userId,
            Name = "Test Store",
            Color = "#ffffff",
        };
        _context.Setup(c => c.UserId).Returns(_userId);
        _service = new StoreService(
            _queries.Object,
            _commands.Object,
            _context.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task GetById_ReturnsStore_WhenExists()
    {
        // Arrange
        _queries
            .Setup(q => q.GetById(_userId, _storeId, CancellationToken.None))
            .ReturnsAsync(Some(_testStore));

        // Act
        var result = await _service.GetById(_storeId);

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(_storeId, result.RightToList()[0].Id);
    }

    [Fact]
    public async Task GetById_ReturnsError_WhenNotFound()
    {
        // Arrange
        _queries
            .Setup(q => q.GetById(_userId, _storeId, CancellationToken.None))
            .ReturnsAsync(Option<Store>.None);

        // Act
        var result = await _service.GetById(_storeId);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<StoreNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task GetById_ReturnsError_WhenUserNotFound()
    {
        // Arrange
        _context.Setup(c => c.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.GetById(_storeId);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task GetAll_ReturnsStores_WhenUserExists()
    {
        // Arrange
        var stores = new List<Store> { _testStore };
        _queries.Setup(q => q.Get(_userId, CancellationToken.None)).ReturnsAsync(stores);

        // Act
        var result = (await _service.GetAll()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(_storeId, result.First().Id);
    }

    [Fact]
    public async Task GetAll_ReturnsEmpty_WhenUserNotFound()
    {
        // Arrange
        _context.Setup(c => c.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Create_ReturnsStore_WhenSuccess()
    {
        // Arrange
        var request = new CreateStoreRequest { Name = "New", Color = "#000000" };
        var newStore = new Store
        {
            Id = Guid.NewGuid(),
            UserId = _userId,
            Name = request.Name,
            Color = request.Color,
        };

        _commands
            .Setup(c => c.Add(It.IsAny<Store>(), CancellationToken.None))
            .ReturnsAsync(Right<Error, Store>(newStore));

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(request.Name, result.RightToList()[0].Name);
    }

    [Fact]
    public async Task Create_ReturnsError_WhenUserNotFound()
    {
        // Arrange
        _context.Setup(c => c.UserId).Returns((Guid?)null);
        var request = new CreateStoreRequest { Name = "New", Color = "#000000" };

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Update_ReturnsError_WhenStoreNotFound()
    {
        // Arrange
        var request = new UpdateStoreRequest
        {
            Id = _storeId,
            Name = "Updated",
            Color = "#222222",
        };
        _queries
            .Setup(q => q.GetById(_userId, _storeId, CancellationToken.None))
            .ReturnsAsync(Option<Store>.None);

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<StoreNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Update_ReturnsUpdatedStore_WhenSuccess()
    {
        // Arrange
        var request = new UpdateStoreRequest
        {
            Id = _storeId,
            Name = "Updated",
            Color = "#222222",
        };
        _queries
            .Setup(q => q.GetById(_userId, _storeId, CancellationToken.None))
            .ReturnsAsync(Some(_testStore));
        _commands
            .Setup(c => c.Update(It.IsAny<Store>(), CancellationToken.None))
            .ReturnsAsync(Right<Error, Store>(_testStore));

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(_storeId, result.RightToList()[0].Id);
    }

    [Fact]
    public async Task Delete_ReturnsNone_WhenSuccess()
    {
        // Arrange
        _commands
            .Setup(c => c.Delete(_storeId, _userId, CancellationToken.None))
            .ReturnsAsync(Option<Error>.None);

        // Act
        var result = await _service.Delete(_storeId);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task Delete_ReturnsError_WhenFails()
    {
        // Arrange
        _commands
            .Setup(c => c.Delete(_storeId, _userId, CancellationToken.None))
            .ReturnsAsync(Some<Error>(new StoreNotFoundError(_storeId)));

        // Act
        var result = await _service.Delete(_storeId);

        // Assert
        Assert.True(result.IsSome);
        Assert.IsType<StoreNotFoundError>(result.First());
    }

    [Fact]
    public async Task Delete_ReturnsError_WhenUserNotFound()
    {
        // Arrange
        _context.Setup(c => c.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.Delete(_storeId);

        // Assert
        Assert.True(result.IsSome);
        Assert.IsType<UserNotFoundError>(result.First());
    }
}
