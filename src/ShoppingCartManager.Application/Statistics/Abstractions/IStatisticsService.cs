using ShoppingCartManager.Application.Statistics.Models;

namespace ShoppingCartManager.Application.Statistics.Abstractions;

public interface IStatisticsService
{
    Task<Either<Error, StatisticsResponse>> GetStatistics(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
    );
}
