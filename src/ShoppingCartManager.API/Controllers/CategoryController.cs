using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Application.Category.Models;

namespace ShoppingCartManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await categoryService.GetById(id, cancellationToken);

        return result.Match(
            Right: category => Ok(new CategoryResponse(category)),
            Left: ErrorActionResultHandler.Handle
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await categoryService.GetAll(cancellationToken);
        return Ok(result.Select(c => new CategoryResponse(c)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await categoryService.Create(request, cancellationToken);

        return result.Match(
            Right: category => Ok(new CategoryResponse(category)),
            Left: ErrorActionResultHandler.Handle
        );
    }

    [HttpPut]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await categoryService.Update(request, cancellationToken);

        return result.Match(
            Right: category => Ok(new CategoryResponse(category)),
            Left: ErrorActionResultHandler.Handle
        );
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await categoryService.Delete(id, cancellationToken);

        return result.Match(None: NoContent, Some: ErrorActionResultHandler.Handle);
    }
}
