namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_05_04_1400_05)]
public sealed class AddUserIdForeignKeyToProduct : AddForeignKeyMigrationTemplate<Category, User>
{
    protected override string ForeignColumn => nameof(Category.UserId);

    protected override Rule OnDeleteRule => Rule.Cascade;
    protected override Rule OnUpdateRule => Rule.Cascade;
}
