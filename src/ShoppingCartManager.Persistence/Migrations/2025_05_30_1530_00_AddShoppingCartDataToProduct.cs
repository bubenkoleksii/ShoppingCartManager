namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_05_30_1530_00)]
public sealed class AddShoppingCartDataToProduct : MigrationTemplate
{
    protected override string TableName => nameof(Product);

    protected override void ApplyUp()
    {
        Alter.Table(TableName)
            .AddColumn(nameof(Product.IsInCart)).AsBoolean().NotNullable().WithDefaultValue(false)
            .AddColumn(nameof(Product.InCartAt)).AsDateTime().Nullable()
            .AddColumn(nameof(Product.Price)).AsDecimal().NotNullable().WithDefaultValue(0);
    }

    protected override void ApplyDown()
    {
        Delete.Column(nameof(Product.IsInCart)).FromTable(TableName);
        Delete.Column(nameof(Product.InCartAt)).FromTable(TableName);
        Delete.Column(nameof(Product.Price)).FromTable(TableName);
    }
}
