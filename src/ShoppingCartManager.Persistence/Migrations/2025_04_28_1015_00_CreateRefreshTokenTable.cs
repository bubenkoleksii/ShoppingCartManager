namespace ShoppingCartManager.Persistence.Migrations;

[Migration(version: 2025_04_28_1015_00)]
public sealed class CreateRefreshTokenTable : CreateTableMigrationTemplate
{
    protected override string TableName => nameof(RefreshToken);

    protected override void AddCustomColumns(ICreateTableColumnOptionOrWithColumnSyntax table)
    {
        table
            .WithColumn(nameof(RefreshToken.UserId))
            .AsGuid()
            .NotNullable()
            .WithColumn(nameof(RefreshToken.Token))
            .AsString(512)
            .NotNullable()
            .WithColumn(nameof(RefreshToken.ExpiresAt))
            .AsDateTime()
            .NotNullable()
            .WithColumn(nameof(RefreshToken.RevokedAt))
            .AsDateTime()
            .Nullable()
            .WithColumn(nameof(RefreshToken.ReplacedByToken))
            .AsString(512)
            .Nullable();
    }
}
