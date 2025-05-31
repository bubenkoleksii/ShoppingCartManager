using ShoppingCartManager.Application.Statistics.Abstractions;
using ShoppingCartManager.Application.Statistics.Models;

namespace ShoppingCartManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class StatisticsController(IStatisticsService statisticsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(StatisticsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken = default
    )
    {
        var result = await statisticsService.GetStatistics(from, to, cancellationToken);

        return result.Match(Ok, ErrorActionResultHandler.Handle);
    }
}
