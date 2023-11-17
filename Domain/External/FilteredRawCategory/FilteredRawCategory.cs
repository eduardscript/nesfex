namespace ConsoleApp1.Domain.External;

public class FilteredRawCategory
{
    public string Id { get; set; } = default!;
    
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;
    
    public CategoryType Type { get; set; }
}

/// <summary>
/// Defines the type of category
/// </summary>
public enum CategoryType
{
    /// <summary>
    /// Indicates that the category is a capsule category
    /// </summary>
    Capsules,
    /// <summary>
    /// Indicates that the category is a cup size category
    /// </summary>
    CupSize,
    /// <summary>
    /// Indicates that the category is a flavor category
    /// </summary>
    Flavor,
    /// <summary>
    /// Indicates that the category is a cup size category
    /// </summary>
    Label,
}