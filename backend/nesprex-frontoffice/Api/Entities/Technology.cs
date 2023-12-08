namespace Api.Entities;


/// <summary>
/// Represents a user technology with its selected capsules
/// </summary>
/// <param name="UserId">Represents the user identifier</param>
/// <param name="Name">Represents the user selected technology name</param>
/// <param name="Capsules">The added / updated capsules selected by the user</param>
public record Technology(
    Guid UserId,
    string Name,
    IEnumerable<SelectedCapsule> Capsules)
{
    /// <summary>
    /// When the technology was added in the user's account
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// When some capsule was added / updated in the technology in the user's account
    /// </summary>
    public DateTime? UpdatedAt { get; init; }
}
    
/// <summary>
/// The user selected capsule
/// </summary>
/// <param name="Name">Represent the user´s selected capsule name</param>
/// <param name="Quantity">Represents the selected capsule quantity</param>
public record SelectedCapsule(
    string Name,
    int Quantity = 0);