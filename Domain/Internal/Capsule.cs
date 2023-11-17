namespace ConsoleApp1.Domain.Internal;

public record Category(
    string Name,
    string Description,
    Technology Technology,
    IEnumerable<Capsule> Capsules);

public record Technology(
    string Name);

public record Capsule(
    string Name,
    string? Description,
    string? ImageUrl,
    double Price,
    Info? Info);

public record Info(
    int? Intensity,
    int? Bitterness,
    int? Acidity,
    int? RoastLevel,
    int? Body,
    IEnumerable<string> Labels,
    IEnumerable<string>? CupSizes,
    IEnumerable<string>? Aromas);
    
public record Flavor(
    string Name);