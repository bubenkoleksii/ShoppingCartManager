using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartManager.Persistence.Migrations;

namespace ShoppingCartManager.Persistence.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        string connectionString
    )
    {
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runnerBuilder =>
                runnerBuilder
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(CreateUsersTable).Assembly)
                    .For.Migrations()
            )
            .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole());

        return services;
    }
}
