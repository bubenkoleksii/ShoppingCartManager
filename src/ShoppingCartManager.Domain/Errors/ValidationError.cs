namespace ShoppingCartManager.Domain.Errors;

public abstract record ValidationError : Error
{
    public override string Type => "validation_request_error";
    public override string DefaultErrorMessage => "Unknown request error.";
    public virtual Dictionary<string, object> Details { get; init; } = [];
}
