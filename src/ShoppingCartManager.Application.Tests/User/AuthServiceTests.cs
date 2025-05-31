using ShoppingCartManager.Application.Common.Abstractions;
using ShoppingCartManager.Application.RefreshToken.Abstractions;
using ShoppingCartManager.Application.Security.Jwt;
using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Application.User.Errors;
using ShoppingCartManager.Application.User.Implementations;
using ShoppingCartManager.Application.User.Models;

namespace ShoppingCartManager.Application.Tests.User;

using User = Domain.Entities.User;

public class AuthServiceTests
{
    private readonly Mock<IUserCommands> _userCommands = new();
    private readonly Mock<IUserQueries> _userQueries = new();
    private readonly Mock<IRefreshTokenGenerator> _refreshTokenGenerator = new();
    private readonly Mock<IRefreshTokenCommands> _refreshTokenCommands = new();
    private readonly Mock<IRefreshTokenQueries> _refreshTokenQueries = new();
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator = new();
    private readonly Mock<IUserContext> _userContext = new();
    private readonly Mock<ILogger<AuthService>> _logger = new();
    private readonly Mock<IOnUserAddedHandler> _onUserAddedHandler = new();

    private readonly AuthService _service;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly User _testUser;
    private const string _password = "Test1234";
    private const string _token = "accessToken";
    private const string _refresh = "refreshToken";

    public AuthServiceTests()
    {
        _testUser = new User
        {
            Id = _userId,
            FullName = "Test User",
            Email = "test@example.com",
        };
        _service = new AuthService(
            _userCommands.Object,
            _userQueries.Object,
            _refreshTokenGenerator.Object,
            _refreshTokenCommands.Object,
            _refreshTokenQueries.Object,
            _jwtTokenGenerator.Object,
            _userContext.Object,
            _logger.Object,
            [_onUserAddedHandler.Object]
        );
    }

    [Fact]
    public async Task Register_ReturnsError_WhenEmailExists()
    {
        // Arrange
        _userQueries
            .Setup(x => x.EmailExists(_testUser.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.Register(
            new RegisterUserRequest
            {
                Email = _testUser.Email,
                FullName = _testUser.FullName!,
                Password = "testPassword06",
            }
        );

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<EmailAlreadyExistsError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Register_ReturnsAuthResult_WhenSuccess()
    {
        // Arrange
        _userQueries
            .Setup(x => x.EmailExists(_testUser.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _userCommands
            .Setup(x => x.Add(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Right<Error, User>(_testUser));
        _refreshTokenGenerator
            .Setup(x => x.Generate(_userId))
            .Returns(new Domain.Entities.RefreshToken { Token = _refresh, UserId = _userId });
        _refreshTokenCommands
            .Setup(x =>
                x.Add(It.IsAny<Domain.Entities.RefreshToken>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(Option<Error>.None);
        _jwtTokenGenerator.Setup(x => x.GenerateToken(_testUser)).Returns(_token);

        // Act
        var result = await _service.Register(
            new RegisterUserRequest
            {
                FullName = _testUser.FullName!,
                Email = _testUser.Email,
                Password = _password,
            }
        );

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(_token, result.RightToList()[0].Token);
        Assert.Equal(_refresh, result.RightToList()[0].RefreshToken);
    }

    [Fact]
    public async Task Login_ReturnsError_WhenUserNotFound()
    {
        // Arrange
        _userQueries
            .Setup(x => x.GetByEmail(_testUser.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Option<User>.None);

        // Act
        var result = await _service.Login(
            new LoginRequest { Email = _testUser.Email, Password = _password }
        );

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<InvalidCredentialsError>(result.LeftToList()[0]);
    }

    [Fact]
    public async Task Login_ReturnsError_WhenPasswordInvalid()
    {
        // Arrange
        _userQueries
            .Setup(x => x.GetByEmail(_testUser.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Some(_testUser));

        // Act
        var result = await _service.Login(
            new LoginRequest { Email = _testUser.Email, Password = "wrong" }
        );

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<InvalidCredentialsError>(result.LeftToList()[0]);
    }
}
