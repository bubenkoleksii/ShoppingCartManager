namespace ShoppingCartManager.Domain.Entities;

public class RefreshToken : EntityBase
{
    public required Guid UserId { get; set; }
    public required string Token { get; set; }

    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }

    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsExpired => ExpiresAt <= DateTime.UtcNow;
    public bool IsActive => !IsRevoked && !IsExpired;
}
