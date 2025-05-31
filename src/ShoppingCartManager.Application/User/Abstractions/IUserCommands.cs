namespace ShoppingCartManager.Application.User.Abstractions;

using User = Domain.Entities.User;

public interface IUserCommands
{
    Task<Either<Error, User>> Add(User user, CancellationToken cancellationToken = default);
    Task<Either<Error, User>> Update(User user, CancellationToken cancellationToken = default);
    Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default);
}
