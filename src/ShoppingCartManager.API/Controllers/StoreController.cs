using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Application.Store.Models;

namespace ShoppingCartManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await storeService.GetById(id, cancellationToken);
        return result.Match(
            Right: store => Ok(new StoreResponse(store)),
            Left: ErrorActionResultHandler.Handle
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StoreResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await storeService.GetAll(cancellationToken);
        return Ok(result.Select(s => new StoreResponse(s)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStoreRequest request, CancellationToken cancellationToken)
    {
        var result = await storeService.Create(request, cancellationToken);
        return result.Match(
            Right: store => Ok(new StoreResponse(store)),
            Left: ErrorActionResultHandler.Handle
        );
    }

    [HttpPut]
    [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateStoreRequest request, CancellationToken cancellationToken)
    {
        var result = await storeService.Update(request, cancellationToken);
        return result.Match(
            Right: store => Ok(new StoreResponse(store)),
            Left: ErrorActionResultHandler.Handle
        );
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await storeService.Delete(id, cancellationToken);
        return result.Match(
            None: NoContent,
            Some: ErrorActionResultHandler.Handle
        );
    }
}
