namespace ShoppingCartManager.Infrastructure.RefreshToken.Models;

using RefreshToken = Domain.Entities.RefreshToken;

[Table(nameof(RefreshToken))]
public sealed class RefreshTokenDbModel : DbModelBase<RefreshToken>
{
    public const string TableName = nameof(RefreshToken);

    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }

    public RefreshTokenDbModel() { }

    public RefreshTokenDbModel(RefreshToken token)
        : base(token) { }

    protected override void MapFromDomainEntityCore(RefreshToken token)
    {
        UserId = token.UserId;
        Token = token.Token;
        ExpiresAt = token.ExpiresAt;
        RevokedAt = token.RevokedAt;
        ReplacedByToken = token.ReplacedByToken;
    }

    protected override RefreshToken MapToDomainEntityCore() =>
        new()
        {
            UserId = UserId,
            Token = Token,
            ExpiresAt = ExpiresAt,
            RevokedAt = RevokedAt,
            ReplacedByToken = ReplacedByToken,
        };
}
