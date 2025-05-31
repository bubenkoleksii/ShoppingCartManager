namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_05_04_1400_16)]
public sealed class AddCategoryIdForeignKeyToProduct
    : AddForeignKeyMigrationTemplate<Product, Category>
{
    protected override string ForeignColumn => nameof(Product.CategoryId);
}
