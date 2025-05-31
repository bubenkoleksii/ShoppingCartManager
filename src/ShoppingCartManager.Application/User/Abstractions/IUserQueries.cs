namespace ShoppingCartManager.Application.User.Abstractions;

using User = Domain.Entities.User;

public interface IUserQueries
{
    Task<Option<User>> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Option<User>> GetByEmail(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExists(string email, CancellationToken cancellationToken = default);
}
