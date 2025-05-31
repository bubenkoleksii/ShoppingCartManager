namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_04_28_1728_00)]
public sealed class CreateStoreTable : CreateTableMigrationTemplate
{
    protected override string TableName => nameof(Store);

    protected override void AddCustomColumns(ICreateTableColumnOptionOrWithColumnSyntax table)
    {
        table.WithColumn(nameof(Store.UserId)).AsGuid().NotNullable()
            .WithColumn(nameof(Store.Name)).AsString(size: 150).NotNullable()
            .WithColumn(nameof(Store.Color)).AsString(size: 30).Nullable();
    }
}
