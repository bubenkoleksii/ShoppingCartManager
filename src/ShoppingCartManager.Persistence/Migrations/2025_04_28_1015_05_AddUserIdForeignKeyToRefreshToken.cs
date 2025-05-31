namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_04_28_1015_05)]
public sealed class AddUserForeignKeyToRefreshToken
    : AddForeignKeyMigrationTemplate<RefreshToken, User>
{
    protected override string ForeignColumn => nameof(RefreshToken.UserId);

    protected override Rule OnDeleteRule => Rule.Cascade;
    protected override Rule OnUpdateRule => Rule.Cascade;
}
