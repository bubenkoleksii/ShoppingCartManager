using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Infrastructure.User.Errors;
using ShoppingCartManager.Infrastructure.User.Models;

namespace ShoppingCartManager.Infrastructure.User;

using User = Domain.Entities.User;

public sealed class UserCommands(IDbConnection connection) : IUserCommands
{
    public async Task<Either<Error, User>> Add(
        User user,
        CancellationToken cancellationToken = default
    )
    {
        var userModel = new UserDbModel(user);

        try
        {
            var inserted = await connection.Add(UserDbModel.TableName, userModel);
            if (!inserted)
                return new UserCreateFailed(user.Email);
        }
        catch
        {
            return new UserCreateFailed(user.Email);
        }

        var addedUserModel = await connection.GetSingleBy<UserDbModel>(
            UserDbModel.TableName,
            nameof(UserDbModel.Id),
            userModel.Id
        );

        return addedUserModel.Match<Either<Error, User>>(
            Some: model => model.ToDomainEntity(),
            None: () => new UserCreateFailed(user.Email)
        );
    }

    public async Task<Either<Error, User>> Update(
        User user,
        CancellationToken cancellationToken = default
    )
    {
        var userModel = new UserDbModel(user) { UpdatedAt = DateTime.UtcNow };

        try
        {
            var updated = await connection.Update(UserDbModel.TableName, userModel);
            if (!updated)
                return new UserUpdateFailed(user.Id);
        }
        catch
        {
            return new UserUpdateFailed(user.Id);
        }

        var updatedUserModel = await connection.GetSingleBy<UserDbModel>(
            UserDbModel.TableName,
            nameof(UserDbModel.Id),
            userModel.Id
        );

        return updatedUserModel.Match<Either<Error, User>>(
            Some: model => model.ToDomainEntity(),
            None: () => new UserUpdateFailed(user.Id)
        );
    }

    public async Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var deleted = await connection.DeleteById(UserDbModel.TableName, id);

            return deleted
                ? Option<Error>.None
                : new UserDeleteFailed(id);
        }
        catch (Exception)
        {
            return new UserDeleteFailed(id);
        }
    }

}
