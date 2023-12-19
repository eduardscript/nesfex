namespace Shared.Domain.Entities;

using Common;

public record Technology(string Name, string ImageUrl, IEnumerable<Category> Categories) : BaseEntity;

public record Category(string Name, string Description, string ImageUrl, IEnumerable<Capsule> Capsules);

public record Capsule(string Name, string? Description, double Price, Info? Info, string ImageUrl);

public record Info(
	int? Intensity,
	int? Bitterness,
	int? Acidity,
	int? RoastLevel,
	int? Body,
	IEnumerable<string> Labels,
	IEnumerable<string>? CupSizes,
	IEnumerable<string>? Aromas);

public record Flavor(string Name);