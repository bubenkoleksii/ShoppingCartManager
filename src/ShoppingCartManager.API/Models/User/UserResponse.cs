namespace ShoppingCartManager.API.Models.User;

using User = Domain.Entities.User;

public sealed record UserResponse
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public string Email { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public UserResponse(User user)
    {
        Id = user.Id;
        FullName = user.FullName;
        Email = user.Email;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
    }
}
