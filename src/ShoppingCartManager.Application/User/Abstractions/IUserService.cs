using ShoppingCartManager.Application.User.Models;

namespace ShoppingCartManager.Application.User.Abstractions;

using User = Domain.Entities.User;

public interface IUserService
{
    Task<Either<Error, User>> GetById(Guid id, CancellationToken cancellationToken = default);

    Task<Either<Error, User>> GetByEmail(
        string email,
        CancellationToken cancellationToken = default
    );

    Task<Either<Error, User>> Update(UpdateUserRequest request, CancellationToken cancellationToken = default);

    Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default);
}
