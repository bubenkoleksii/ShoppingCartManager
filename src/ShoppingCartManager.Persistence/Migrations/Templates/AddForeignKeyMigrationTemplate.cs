namespace ShoppingCartManager.Persistence.Migrations.Templates;

public abstract class AddForeignKeyMigrationTemplate<TFromEntity, TToEntity> : MigrationTemplate
{
    protected abstract string ForeignColumn { get; }

    protected virtual string FromTable => typeof(TFromEntity).Name;
    protected virtual string ToTable => typeof(TToEntity).Name;
    protected virtual string ToPrimaryColumn => nameof(EntityBase.Id);

    protected virtual Rule OnDeleteRule => Rule.None;
    protected virtual Rule OnUpdateRule => Rule.None;

    protected virtual string ForeignKeyName => $"FK_{FromTable}_{ToTable}_{ForeignColumn}";

    protected override string TableName => FromTable;

    protected override void ApplyUp()
    {
        if (IsSQLite())
            return;

        Create
            .ForeignKey(ForeignKeyName)
            .FromTable(FromTable)
            .ForeignColumn(ForeignColumn)
            .ToTable(ToTable)
            .PrimaryColumn(ToPrimaryColumn)
            .OnDelete(OnDeleteRule)
            .OnUpdate(OnUpdateRule);
    }

    protected override void ApplyDown()
    {
        if (IsSQLite())
            return;

        Delete.ForeignKey(ForeignKeyName).OnTable(FromTable);
    }

    private bool IsSQLite()
    {
        return ConnectionString?.Contains("Data Source=", StringComparison.OrdinalIgnoreCase)
            == true;
    }
}
