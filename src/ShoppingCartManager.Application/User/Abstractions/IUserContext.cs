namespace ShoppingCartManager.Application.User.Abstractions;

public interface IUserContext
{
    Guid? UserId { get; }
}
