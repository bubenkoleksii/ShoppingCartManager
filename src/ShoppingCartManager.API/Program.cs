using Microsoft.OpenApi.Models;
using ShoppingCartManager.Application.DependencyInjection;
using ShoppingCartManager.Infrastructure.DependencyInjection;
using ShoppingCartManager.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new ArgumentException("Connection string is missing");

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddPersistence(connectionString);

builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddHttpLogging();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        name: "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                []
            },
        }
    );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseHttpLogging();

app.MapControllers();

app.Run();
