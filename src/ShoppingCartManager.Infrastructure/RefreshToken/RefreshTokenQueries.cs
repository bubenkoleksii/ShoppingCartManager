using ShoppingCartManager.Application.RefreshToken.Abstractions;
using ShoppingCartManager.Infrastructure.RefreshToken.Models;

namespace ShoppingCartManager.Infrastructure.RefreshToken;

using RefreshToken = Domain.Entities.RefreshToken;

public sealed class RefreshTokenQueries(IDbConnection connection) : IRefreshTokenQueries
{
    public async Task<Option<RefreshToken>> GetByToken(
        string token,
        CancellationToken cancellationToken = default
    )
    {
        var dbModelOption = await connection.GetSingleBy<RefreshTokenDbModel>(
            RefreshTokenDbModel.TableName,
            nameof(RefreshTokenDbModel.Token),
            token
        );

        return dbModelOption.Map(x => x.ToDomainEntity());
    }
}
