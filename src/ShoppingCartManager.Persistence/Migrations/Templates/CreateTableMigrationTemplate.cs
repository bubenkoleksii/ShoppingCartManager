namespace ShoppingCartManager.Persistence.Migrations.Templates;

public abstract class CreateTableMigrationTemplate : Migration
{
    protected abstract string TableName { get; }

    public override void Up()
    {
        if (Schema.Table(TableName).Exists())
            return;

        var table = CreateBaseTable(TableName);
        AddCustomColumns(table);
        AddAuditColumns(table);
    }

    public override void Down() => Delete.Table(TableName);

    protected abstract void AddCustomColumns(ICreateTableColumnOptionOrWithColumnSyntax table);

    private ICreateTableColumnOptionOrWithColumnSyntax CreateBaseTable(string tableName)
    {
        return Create
            .Table(tableName)
            .WithColumn(nameof(EntityBase.Id))
            .AsGuid()
            .PrimaryKey()
            .NotNullable();
    }

    private static void AddAuditColumns(ICreateTableColumnOptionOrWithColumnSyntax table)
    {
        table
            .WithColumn(nameof(EntityBase.CreatedAt))
            .AsDateTime()
            .NotNullable()
            .WithColumn(nameof(EntityBase.UpdatedAt))
            .AsDateTime()
            .Nullable();
    }
}
