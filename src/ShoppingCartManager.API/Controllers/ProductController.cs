using ShoppingCartManager.Application.Product.Abstractions;
using ShoppingCartManager.Application.Product.Models;

namespace ShoppingCartManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ProductListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default
    )
    {
        var result = await productService.Get(skip, take, cancellationToken);

        return result.Match(Ok, ErrorActionResultHandler.Handle);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await productService.GetById(id, cancellationToken);

        return result.Match(Ok, ErrorActionResultHandler.Handle);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await productService.Create(request, cancellationToken);

        return result.Match(Ok, ErrorActionResultHandler.Handle);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await productService.Update(request, cancellationToken);

        return result.Match(Ok, ErrorActionResultHandler.Handle);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await productService.Delete(id, cancellationToken);

        return result.Match(None: NoContent, Some: ErrorActionResultHandler.Handle);
    }

    [HttpPost("{id:guid}/in-cart")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAsInCart(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await productService.MarkAsInCart(id, cancellationToken);

        return result.Match(None: NoContent, Some: ErrorActionResultHandler.Handle);
    }

    [HttpPost("{id:guid}/not-in-cart")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveFromCart(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await productService.RemoveFromCart(id, cancellationToken);

        return result.Match(None: NoContent, Some: ErrorActionResultHandler.Handle);
    }
}
