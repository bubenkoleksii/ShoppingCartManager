namespace ShoppingCartManager.Persistence.Migrations.Templates;

public abstract class MigrationTemplate : Migration
{
    protected abstract string TableName { get; }

    public override void Up()
    {
        if (!ShouldRunUp())
            return;

        ApplyUp();
    }

    public override void Down()
    {
        if (!ShouldRunDown())
            return;

        ApplyDown();
    }

    protected abstract void ApplyUp();
    protected abstract void ApplyDown();

    protected virtual bool ShouldRunUp() => Schema.Table(TableName).Exists();

    protected virtual bool ShouldRunDown() => Schema.Table(TableName).Exists();
}
