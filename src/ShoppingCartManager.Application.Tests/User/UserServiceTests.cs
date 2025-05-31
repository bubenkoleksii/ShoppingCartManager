using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Application.User.Implementations;
using ShoppingCartManager.Application.User.Models;

namespace ShoppingCartManager.Application.Tests.User;

using User = Domain.Entities.User;

public class UserServiceTests
{
    private readonly Mock<IUserQueries> _queries = new();
    private readonly Mock<IUserCommands> _commands = new();
    private readonly Mock<IUserContext> _context = new();
    private readonly Mock<ILogger<UserService>> _logger = new();
    private readonly UserService _service;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly User _testUser;

    public UserServiceTests()
    {
        _testUser = new User
        {
            Id = _userId,
            FullName = "Test User",
            Email = "test@example.com",
        };
        _context.Setup(c => c.UserId).Returns(_userId);
        _service = new UserService(
            _queries.Object,
            _commands.Object,
            _context.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task GetById_ReturnsUser_WhenExists()
    {
        // Arrange
        _queries
            .Setup(q => q.GetById(_userId, CancellationToken.None))
            .ReturnsAsync(Some(_testUser));

        // Act
        var result = await _service.GetById(_userId);

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(_userId, result.RightToList()[0].Id);
    }

    [Fact]
    public async Task GetById_ReturnsError_WhenNotFound()
    {
        // Arrange
        _queries
            .Setup(q => q.GetById(_userId, CancellationToken.None))
            .ReturnsAsync(Option<User>.None);

        // Act
        var result = await _service.GetById(_userId);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task GetByEmail_ReturnsUser_WhenExists()
    {
        // Arrange
        _queries
            .Setup(q => q.GetByEmail(_testUser.Email, CancellationToken.None))
            .ReturnsAsync(Some(_testUser));

        // Act
        var result = await _service.GetByEmail(_testUser.Email);

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(_testUser.Email, result.RightToList()[0].Email);
    }

    [Fact]
    public async Task GetByEmail_ReturnsError_WhenNotFound()
    {
        // Arrange
        _queries
            .Setup(q => q.GetByEmail(_testUser.Email, CancellationToken.None))
            .ReturnsAsync(Option<User>.None);

        // Act
        var result = await _service.GetByEmail(_testUser.Email);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Update_ReturnsError_WhenUnauthorized()
    {
        // Arrange
        _context.Setup(c => c.UserId).Returns(Guid.NewGuid());
        var request = new UpdateUserRequest
        {
            Id = _userId,
            FullName = "New Name",
            Email = "new@example.com",
        };

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Update_ReturnsError_WhenUserNotFound()
    {
        // Arrange
        var request = new UpdateUserRequest
        {
            Id = _userId,
            FullName = "New Name",
            Email = "new@example.com",
        };
        _queries
            .Setup(q => q.GetById(_userId, CancellationToken.None))
            .ReturnsAsync(Option<User>.None);

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<UserNotFoundError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Update_ReturnsUpdatedUser_WhenSuccess()
    {
        // Arrange
        var request = new UpdateUserRequest
        {
            Id = _userId,
            FullName = "Updated",
            Email = "updated@example.com",
        };
        _queries
            .Setup(q => q.GetById(_userId, CancellationToken.None))
            .ReturnsAsync(Some(_testUser));
        _commands
            .Setup(c => c.Update(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(
                Right<Error, User>(
                    new User
                    {
                        Id = _userId,
                        FullName = request.FullName,
                        Email = request.Email,
                    }
                )
            );

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal("Updated", result.RightToList()[0].FullName);
    }

    [Fact]
    public async Task Delete_ReturnsError_WhenUnauthorized()
    {
        // Arrange
        _context.Setup(c => c.UserId).Returns(Guid.NewGuid());

        // Act
        var result = await _service.Delete(_userId);

        // Assert
        Assert.True(result.IsSome);
        Assert.IsType<UserNotFoundError>(result.First());
    }

    [Fact]
    public async Task Delete_ReturnsNone_WhenSuccess()
    {
        // Arrange
        _commands
            .Setup(c => c.Delete(_userId, CancellationToken.None))
            .ReturnsAsync(Option<Error>.None);

        // Act
        var result = await _service.Delete(_userId);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task Delete_ReturnsError_WhenFails()
    {
        // Arrange
        var expectedError = new UserNotFoundError(_userId);
        _commands
            .Setup(c => c.Delete(_userId, CancellationToken.None))
            .ReturnsAsync(Some<Error>(expectedError));

        // Act
        var result = await _service.Delete(_userId);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(expectedError, result.First());
    }
}
