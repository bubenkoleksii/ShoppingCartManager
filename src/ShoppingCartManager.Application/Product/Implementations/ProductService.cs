using ShoppingCartManager.Application.Common.Errors;
using ShoppingCartManager.Application.Product.Abstractions;
using ShoppingCartManager.Application.Product.Models;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.Product.Implementations;

using Product = Domain.Entities.Product;

public sealed class ProductService(
    IProductCommands productCommands,
    IProductQueries productQueries,
    IUserContext userContext,
    ILogger<ProductService> logger
) : IProductService
{
    public async Task<Either<Error, ProductListResponse>> Get(int skip, int take, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching products with pagination: skip={Skip}, take={Take}", skip, take);

        var userId = GetUserId("fetch products");
        if (userId.IsNone) return new UserNotFoundError();

        var (products, total) = await productQueries.Get(userId.First(), skip, take, cancellationToken);

        var response = new ProductListResponse(
            products.Select(p => new ProductResponse(p)),
            total,
            take
        );

        return response;
    }

    public async Task<Either<Error, ProductResponse>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching product with ID {ProductId}", id);

        var userId = GetUserId("fetch product", id);
        if (userId.IsNone) 
            return new UserNotFoundError();

        var result = await productQueries.GetById(userId.First(), id, cancellationToken);

        return result.Match<Either<Error, ProductResponse>>(
            Some: p => new ProductResponse(p),
            None: () => {
                logger.LogWarning("Product with ID {ProductId} not found for user {UserId}", id, userId.First());
                return new ProductNotFoundError(id);
            }
        );
    }

    public async Task<Either<Error, ProductResponse>> Create(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating product {ProductName}", request.Name);

        var userId = GetUserId("create product");
        if (userId.IsNone) return new UserNotFoundError();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            UserId = userId.First(),
            Name = request.Name,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow,
        };

        var result = await productCommands.Add(product, cancellationToken);

        return result.Map(p => new ProductResponse(p));
    }

    public async Task<Either<Error, ProductResponse>> Update(UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating product with ID {ProductId}", request.Id);

        var userId = GetUserId("update product", request.Id);
        if (userId.IsNone) 
            return new UserNotFoundError();

        var existing = await productQueries.GetById(userId.First(), request.Id, cancellationToken);
        if (existing.IsNone)
        {
            logger.LogWarning("Cannot update product {ProductId}: not found", request.Id);
            return new ProductNotFoundError(request.Id);
        }

        var product = existing.First();
        product.Name = request.Name;
        product.CategoryId = request.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;
        product.StoreId = request.StoreId;
        product.Price = request.Price;

        var result = await productCommands.Update(product, cancellationToken);

        return result.Map(p => new ProductResponse(p));
    }

    public async Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting product with ID {ProductId}", id);

        var userId = GetUserId("delete product", id);
        if (userId.IsNone) 
            return new UserNotFoundError();

        return await productCommands.Delete(id, userId.First(), cancellationToken);
    }

    public async Task<Option<Error>> MarkAsInCart(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Marking product with ID {ProductId} as in cart", id);

        var userId = GetUserId("mark product as in cart", id);
        if (userId.IsNone) 
            return new UserNotFoundError();

        return await productCommands.MarkAsInCart(id, userId.First(), cancellationToken);
    }

    public async Task<Option<Error>> RemoveFromCart(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Removing product with ID {ProductId} from cart", id);

        var userId = GetUserId("remove product from cart", id);
        if (userId.IsNone) 
            return new UserNotFoundError();

        return await productCommands.RemoveFromCart(id, userId.First(), cancellationToken);
    }

    private Option<Guid> GetUserId(string context, Guid? resourceId = null)
    {
        var userId = userContext.UserId;
        if (userId is not null)
            return userId.ToOption();

        if (resourceId is not null)
            logger.LogWarning("Failed to {Context} resource {ResourceId}: user not found in context", context, resourceId);
        else
            logger.LogWarning("Failed to {Context}: user not found in context", context);

        return userId.ToOption();
    }
}
