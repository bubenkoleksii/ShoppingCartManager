namespace ShoppingCartManager.Application.Common.Abstractions;

public interface IOnUserAddedHandler
{
    Task Handle(Guid userId, CancellationToken cancellationToken = default);
}
