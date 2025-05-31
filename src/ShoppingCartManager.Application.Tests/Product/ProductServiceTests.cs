using ShoppingCartManager.Application.Product.Abstractions;
using ShoppingCartManager.Application.Product.Implementations;
using ShoppingCartManager.Application.Product.Models;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.Tests.Product;

using Product = Domain.Entities.Product;

public class ProductServiceTests
{
    private readonly Mock<IProductCommands> _commands = new();
    private readonly Mock<IProductQueries> _queries = new();
    private readonly Mock<IUserContext> _userContext = new();
    private readonly Mock<ILogger<ProductService>> _logger = new();
    private readonly ProductService _service;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _productId = Guid.NewGuid();

    public ProductServiceTests()
    {
        _userContext.Setup(u => u.UserId).Returns(_userId);
        _service = new ProductService(
            _commands.Object,
            _queries.Object,
            _userContext.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task GetById_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.GetById(_productId);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task GetById_ReturnsError_WhenProductNotFound()
    {
        // Arrange
        _queries
            .Setup(q => q.GetById(_userId, _productId, CancellationToken.None))
            .ReturnsAsync(Option<Product>.None);

        // Act
        var result = await _service.GetById(_productId);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<ProductNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Create_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);
        var request = new CreateProductRequest { Name = "Test Product" };

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Update_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);
        var request = new UpdateProductRequest { Id = _productId, Name = "Updated" };

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Update_ReturnsError_WhenProductNotFound()
    {
        // Arrange
        _queries
            .Setup(q => q.GetById(_userId, _productId, CancellationToken.None))
            .ReturnsAsync(Option<Product>.None);
        var request = new UpdateProductRequest { Id = _productId, Name = "Updated" };

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<ProductNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Delete_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.Delete(_productId);

        // Assert
        Assert.True(result.IsSome);
        Assert.IsType<UserNotFoundError>(result.First());
    }

    [Fact]
    public async Task MarkAsInCart_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.MarkAsInCart(_productId);

        // Assert
        Assert.True(result.IsSome);
        Assert.IsType<UserNotFoundError>(result.First());
    }

    [Fact]
    public async Task RemoveFromCart_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.RemoveFromCart(_productId);

        // Assert
        Assert.True(result.IsSome);
        Assert.IsType<UserNotFoundError>(result.First());
    }
}
