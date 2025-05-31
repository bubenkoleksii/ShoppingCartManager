namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_05_04_1400_00)]
public sealed class CreateProductTable : CreateTableMigrationTemplate
{
    protected override string TableName => nameof(Product);

    protected override void AddCustomColumns(ICreateTableColumnOptionOrWithColumnSyntax table)
    {
        table
            .WithColumn(nameof(Product.UserId))
            .AsGuid()
            .NotNullable()
            .WithColumn(nameof(Product.CategoryId))
            .AsGuid()
            .Nullable()
            .WithColumn(nameof(Product.StoreId))
            .AsGuid()
            .Nullable()
            .WithColumn(nameof(Product.Name))
            .AsString(255)
            .NotNullable();
    }
}
