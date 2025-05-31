using CaseExtensions;
using ShoppingCartManager.Domain.Errors;

namespace ShoppingCartManager.API.Errors;

public static class ErrorActionResultHandler
{
    public static IActionResult Handle(Error error)
    {
        var problemDetails = new ProblemDetails
        {
            Type = error.Type,
            Title = error.Title.ToSnakeCase(),
            Detail = error.ErrorMessage ?? error.DefaultErrorMessage,
            Status = GetStatusCode(error),
        };

        if (error is ValidationError validationError)
        {
            problemDetails.Extensions["details"] = validationError.Details;
        }

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }

    private static int GetStatusCode(Error error) =>
        error switch
        {
            ValidationError => StatusCodes.Status400BadRequest,
            _ when error.Title.Contains("NotFound", StringComparison.OrdinalIgnoreCase) =>
                StatusCodes.Status404NotFound,
            _ when error.Title.Contains("Exists", StringComparison.OrdinalIgnoreCase) =>
                StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };
}
