namespace ShoppingCartManager.Domain.Errors;

public abstract record Error
{
    public abstract string Type { get; }
    public virtual string Title => GetType().Name;

    public abstract string? ErrorMessage { get; }
    public virtual string DefaultErrorMessage => "Unknown error";
}
