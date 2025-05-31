using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartManager.Application.Cache.Abstractions;
using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Application.Product.Abstractions;
using ShoppingCartManager.Application.RefreshToken.Abstractions;
using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Infrastructure.Cache;
using ShoppingCartManager.Infrastructure.Category;
using ShoppingCartManager.Infrastructure.Product;
using ShoppingCartManager.Infrastructure.RefreshToken;
using ShoppingCartManager.Infrastructure.Store;
using ShoppingCartManager.Infrastructure.User;

namespace ShoppingCartManager.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddCache();

        services.AddScoped<IDbConnection>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        });

        AddUser(services);
        AddStore(services);
        AddCategory(services);
        AddProduct(services);

        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddLazyCache();
        services.AddSingleton<ICacheProvider, LazyCacheProvider>();

        return services;
    }

    public static IServiceCollection AddUser(this IServiceCollection services)
    {
        services.AddScoped<IUserCommands, UserCommands>();
        services.AddScoped<IUserQueries, UserQueries>();

        services.AddScoped<IRefreshTokenCommands, RefreshTokenCommands>();
        services.AddScoped<IRefreshTokenQueries, RefreshTokenQueries>();

        return services;
    }

    public static IServiceCollection AddStore(this IServiceCollection services)
    {
        services.AddScoped<IStoreCommands, StoreCommands>();
        services.AddScoped<IStoreQueries, StoreQueries>();

        return services;
    }

    public static IServiceCollection AddCategory(this IServiceCollection services)
    {
        services.AddScoped<ICategoryCommands, CategoryCommands>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();

        return services;
    }

    public static IServiceCollection AddProduct(this IServiceCollection services)
    {
        services.AddScoped<IProductCommands, ProductCommands>();
        services.AddScoped<IProductQueries, ProductQueries>();

        return services;
    }
}
