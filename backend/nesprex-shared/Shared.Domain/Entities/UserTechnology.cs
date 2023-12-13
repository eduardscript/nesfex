namespace Shared.Domain.Entities;

public record UserTechnology(
    Guid UserId,
    string Name,
    IEnumerable<SelectedCapsule> Capsules)
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; init; }
}

public record SelectedCapsule(
    string Name,
    int Quantity = 0);