namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_04_27_1940_00)]
public class CreateUsersTable : CreateTableMigrationTemplate
{
    protected override string TableName => nameof(User);

    protected override void AddCustomColumns(ICreateTableColumnOptionOrWithColumnSyntax table)
    {
        table
            .WithColumn(nameof(User.FullName))
            .AsString(size: 150)
            .Nullable()
            .WithColumn(nameof(User.Email))
            .AsString(size: 255)
            .NotNullable()
            .WithColumn(nameof(User.PasswordHash))
            .AsBinary()
            .NotNullable()
            .WithColumn(nameof(User.PasswordSalt))
            .AsBinary()
            .NotNullable();

        Create
            .Index($"IX_{TableName}_Email")
            .OnTable(TableName)
            .OnColumn(nameof(User.Email))
            .Unique();
    }
}
