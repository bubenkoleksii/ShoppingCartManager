namespace ShoppingCartManager.Domain.Errors;

public abstract record ApiError : Error
{
    public override string Type => "api_error";
    public override string DefaultErrorMessage => "Unknown API error";
}
