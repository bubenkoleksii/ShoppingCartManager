namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_05_01_1800_00)]
public sealed class CreateCategoryTable : CreateTableMigrationTemplate
{
    protected override string TableName => nameof(Category);

    protected override void AddCustomColumns(ICreateTableColumnOptionOrWithColumnSyntax table)
    {
        table
            .WithColumn(nameof(Category.UserId))
            .AsGuid()
            .NotNullable()
            .WithColumn(nameof(Category.Name))
            .AsString(size: 128)
            .NotNullable()
            .WithColumn(nameof(Category.IconId))
            .AsInt32()
            .Nullable();
    }
}
