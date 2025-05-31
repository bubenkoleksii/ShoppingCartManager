namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_05_04_1400_36)]
public sealed class AddStoreIdForeignKeyToProduct : AddForeignKeyMigrationTemplate<Product, Category>
{
    protected override string ForeignColumn => nameof(Product.StoreId);
}
