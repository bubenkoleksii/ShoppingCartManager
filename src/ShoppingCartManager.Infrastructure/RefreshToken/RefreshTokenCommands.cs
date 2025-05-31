using ShoppingCartManager.Application.RefreshToken.Abstractions;
using ShoppingCartManager.Infrastructure.RefreshToken.Errors;
using ShoppingCartManager.Infrastructure.RefreshToken.Models;

namespace ShoppingCartManager.Infrastructure.RefreshToken;

using RefreshToken = Domain.Entities.RefreshToken;

public sealed class RefreshTokenCommands(
    IDbConnection connection,
    ILogger<RefreshTokenCommands> logger
) : IRefreshTokenCommands
{
    public async Task<Option<Error>> Add(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var dbModel = new RefreshTokenDbModel(refreshToken);
            await connection.Add(RefreshTokenDbModel.TableName, dbModel);

            logger.LogInformation("Refresh token added for user {UserId}", refreshToken.UserId);
            return Option<Error>.None;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to add refresh token for user {UserId}",
                refreshToken.UserId
            );
            return new RefreshTokenCreateFailed();
        }
    }

    public async Task<Option<Error>> Revoke(
        string token,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            const string sql = $"""
                    UPDATE [{RefreshTokenDbModel.TableName}]
                    SET [RevokedAt] = @RevokedAt
                    WHERE [Token] = @Token
                """;

            await connection.ExecuteAsync(sql, new { Token = token, RevokedAt = DateTime.UtcNow });

            return Option<Error>.None;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "Failed to revoke refresh token");
            return new RefreshTokenRevokeFailed();
        }
    }

    public async Task<Option<Error>> MarkReplacedBy(
        string oldToken,
        string newToken,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            const string sql = $"""
                    UPDATE [{RefreshTokenDbModel.TableName}]
                    SET [ReplacedByToken] = @NewToken
                    WHERE [Token] = @OldToken
                """;

            await connection.ExecuteAsync(sql, new { OldToken = oldToken, NewToken = newToken });

            return Option<Error>.None;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "Failed to mark refresh token as replaced");
            return new RefreshTokenReplaceFailed();
        }
    }
}
