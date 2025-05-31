using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Application.Category.Errors;
using ShoppingCartManager.Application.Category.Implementations;
using ShoppingCartManager.Application.Category.Models;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.Tests.Category;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryQueries> _queries = new();
    private readonly Mock<ICategoryCommands> _commands = new();
    private readonly Mock<IUserContext> _userContext = new();
    private readonly Mock<ILogger<CategoryService>> _logger = new();
    private readonly CategoryService _service;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _categoryId = Guid.NewGuid();

    public CategoryServiceTests()
    {
        _userContext.Setup(u => u.UserId).Returns(_userId);
        _service = new CategoryService(
            _queries.Object,
            _commands.Object,
            _userContext.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task GetAll_ReturnsEmpty_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetById_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.GetById(_categoryId);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task GetById_ReturnsError_WhenCategoryNotFound()
    {
        // Arrange
        _queries
            .Setup(q => q.GetById(_userId, _categoryId, CancellationToken.None))
            .ReturnsAsync(Option<Domain.Entities.Category>.None);

        // Act
        var result = await _service.GetById(_categoryId);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<CategoryNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Create_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);
        var request = new CreateCategoryRequest { Name = "Test" };

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
        var request = new UpdateCategoryRequest { Id = _categoryId, Name = "Updated" };

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Update_ReturnsError_WhenCategoryNotFound()
    {
        // Arrange
        _queries
            .Setup(q => q.GetById(_userId, _categoryId, CancellationToken.None))
            .ReturnsAsync(Option<Domain.Entities.Category>.None);
        var request = new UpdateCategoryRequest { Id = _categoryId, Name = "Updated" };

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<CategoryNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Delete_ReturnsError_WhenUserIdIsNull()
    {
        // Arrange
        _userContext.Setup(u => u.UserId).Returns((Guid?)null);

        // Act
        var result = await _service.Delete(_categoryId);

        // Assert
        Assert.True(result.IsSome);
        Assert.IsType<UserNotFoundError>(result.First());
    }

    [Fact]
    public async Task Delete_ReturnsNone_WhenDeletedSuccessfully()
    {
        // Arrange
        _commands
            .Setup(c => c.Delete(_categoryId, _userId, CancellationToken.None))
            .ReturnsAsync(Option<Error>.None);

        // Act
        var result = await _service.Delete(_categoryId);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task Delete_ReturnsError_WhenDeleteFails()
    {
        // Arrange
        var expectedError = new CategoryNotFoundError(_categoryId);
        _commands
            .Setup(c => c.Delete(_categoryId, _userId, CancellationToken.None))
            .ReturnsAsync(Some<Error>(expectedError));

        // Act
        var result = await _service.Delete(_categoryId);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(expectedError, result.First());
    }
}
