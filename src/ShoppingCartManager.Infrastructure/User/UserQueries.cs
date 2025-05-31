using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Infrastructure.User.Models;

namespace ShoppingCartManager.Infrastructure.User;

using User = Domain.Entities.User;

public sealed class UserQueries(IDbConnection connection) : IUserQueries
{
    public async Task<Option<User>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var dbUserOption = await connection.GetSingleBy<UserDbModel>(
            UserDbModel.TableName,
            nameof(UserDbModel.Id),
            id
        );

        return dbUserOption.Match(
            Some: dbUser => Some(dbUser.ToDomainEntity()),
            None: () => Option<User>.None
        );
    }

    public async Task<Option<User>> GetByEmail(
        string email,
        CancellationToken cancellationToken = default
    )
    {
        var dbUserOption = await connection.GetSingleBy<UserDbModel>(
            UserDbModel.TableName,
            nameof(UserDbModel.Email),
            email
        );

        return dbUserOption.Match(
            Some: dbUser => Some(dbUser.ToDomainEntity()),
            None: () => Option<User>.None
        );
    }

    public Task<bool> EmailExists(string email, CancellationToken cancellationToken = default) =>
        connection.Exists(UserDbModel.TableName, nameof(UserDbModel.Email), email);
}
