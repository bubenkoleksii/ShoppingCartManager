namespace ShoppingCartManager.Application.Security.Jwt;

public sealed class JwtSettings
{
    public const string JwtSettingsSectionName = "Jwt";

    [Required]
    public required string Issuer { get; init; }

    [Required]
    public required string Audience { get; init; }

    [Required, MinLength(32)]
    public required string SecretKey { get; init; }

    [Range(1, 43200)]
    public int ExpiryMinutes { get; init; } = 1440;
}
