namespace ShoppingCartManager.Infrastructure.User.Models;

using User = Domain.Entities.User;

[Table(nameof(User))]
public sealed class UserDbModel : DbModelBase<User>
{
    public const string TableName = nameof(User);

    public string? FullName { get; set; }
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];

    public UserDbModel() { }

    public UserDbModel(User user)
        : base(user) { }

    protected override void MapFromDomainEntityCore(User user)
    {
        FullName = user.FullName;
        Email = user.Email;
        PasswordHash = user.PasswordHash;
        PasswordSalt = user.PasswordSalt;
    }

    protected override User MapToDomainEntityCore() =>
        new()
        {
            FullName = FullName,
            Email = Email,
            PasswordHash = PasswordHash,
            PasswordSalt = PasswordSalt,
        };
}
