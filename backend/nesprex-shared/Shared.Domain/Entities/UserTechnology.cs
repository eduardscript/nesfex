namespace Shared.Domain.Entities;

public record UserTechnologies(
    Guid UserId,
    IEnumerable<UserTechnology> Technologies);

public record UserTechnology(
    string Name,
    IEnumerable<SelectedCapsule> Capsules)
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; init; }
}

public record SelectedCapsule(
    string Name,
    int Quantity = 0);