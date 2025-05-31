namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_05_04_1805)]
public sealed class AddUserIdForeignKeyToCategory : AddForeignKeyMigrationTemplate<Category, User>
{
    protected override string ForeignColumn => nameof(Category.UserId);

    protected override Rule OnDeleteRule => Rule.Cascade;
    protected override Rule OnUpdateRule => Rule.Cascade;
}
