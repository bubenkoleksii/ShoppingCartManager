using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Application.Category.Implementations;
using ShoppingCartManager.Application.Common.Abstractions;
using ShoppingCartManager.Application.Product.Abstractions;
using ShoppingCartManager.Application.Product.Implementations;
using ShoppingCartManager.Application.RefreshToken.Abstractions;
using ShoppingCartManager.Application.RefreshToken.Implementations;
using ShoppingCartManager.Application.Security.Jwt;
using ShoppingCartManager.Application.Statistics.Abstractions;
using ShoppingCartManager.Application.Statistics.Implementations;
using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Application.Store.Implementations;
using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Application.User.Implementations;

namespace ShoppingCartManager.Application.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        AddJwt(services, configuration);
        AddUser(services);
        AddCategory(services);
        AddStore(services);
        AddProduct(services);
        AddStatistics(services);

        return services;
    }

    public static IServiceCollection AddJwt(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.JwtSettingsSectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()!;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
                        ),
                    };
                }
            );

        services.AddAuthorization();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }

    public static IServiceCollection AddUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddAuthorization();

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }

    public static IServiceCollection AddStore(this IServiceCollection services)
    {
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IOnUserAddedHandler, DefaultStoresOnUserAddedHandler>();
        return services;
    }

    public static IServiceCollection AddCategory(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOnUserAddedHandler, DefaultCategoriesOnUserAddedHandler>();
        return services;
    }

    public static IServiceCollection AddProduct(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        return services;
    }

    public static IServiceCollection AddStatistics(this IServiceCollection services)
    {
        services.AddScoped<IStatisticsService, StatisticsService>();
        return services;
    }
}
