namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_04_28_1728_05)]
public sealed class AddUserIdForeignKeyToStore : AddForeignKeyMigrationTemplate<Store, User>
{
    protected override string ForeignColumn => nameof(RefreshToken.UserId);

    protected override Rule OnDeleteRule => Rule.Cascade;
    protected override Rule OnUpdateRule => Rule.Cascade;
}
