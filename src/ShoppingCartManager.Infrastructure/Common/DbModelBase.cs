namespace ShoppingCartManager.Infrastructure.Common;

public abstract class DbModelBase<TDomainEntity>
    where TDomainEntity : EntityBase
{
    protected DbModelBase() { }

    protected DbModelBase(TDomainEntity entity)
    {
        FromDomainEntity(entity);
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public DbModelBase<TDomainEntity> FromDomainEntity(TDomainEntity entity)
    {
        MapFromDomainEntityCore(entity);
        return this;
    }

    public TDomainEntity ToDomainEntity()
    {
        var entity = MapToDomainEntityCore();
        MapIdToDomainEntity(entity);
        MapAuditFieldsToDomainEntity(entity);
        return entity;
    }

    private void MapIdToDomainEntity(TDomainEntity entity) => entity.Id = Id;

    private void MapAuditFieldsToDomainEntity(TDomainEntity entity)
    {
        entity.CreatedAt = CreatedAt;
        entity.UpdatedAt = UpdatedAt;
    }

    protected abstract void MapFromDomainEntityCore(TDomainEntity entity);
    protected abstract TDomainEntity MapToDomainEntityCore();
}
