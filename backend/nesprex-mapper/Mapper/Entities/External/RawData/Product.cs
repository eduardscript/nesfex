namespace Mapper.Entities.External.RawData;

public class ExternalNespressoCategory
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Icon? Icon { get; set; } = default!;
    public Icon? DetailsIcon { get; set; }
    public string? Url { get; set; }
    public string? CapacityLabel { get; set; }
    public string? RangeLink { get; set; }

    public IEnumerable<string> SubCategories { get; set; } = Array.Empty<string>();
    public IEnumerable<string> SuperCategories { get; set; } = Array.Empty<string>();
}

public class Icon
{
    public string Url { get; set; } = default!;
    public string? AltText { get; set; }
}

public class Product
{
    public string Id { get; set; } = default!;
    public string InternationalId { get; set; } = default!;
    public string LegacyId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string InternationalName { get; set; } = default!;
    public string? Description { get; set; }
    public Icon Image { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string? Headline { get; set; } = default!;

    public double Price { get; set; } = default!;
    
    public string Url { get; set; } = default!;
    public int SalesMultiple { get; set; }
    public int UnitQuantity { get; set; }
    public int? MaxOrderQuantity { get; set; }
    public bool Available { get; set; }
    public bool InStock { get; set; }
    public bool PushRatingEnabled { get; set; }
    public IEnumerable<string> Technologies { get; set; } = Array.Empty<string>();
    public bool HidePrice { get; set; }
    public IEnumerable<string> HighlightLabels { get; set; } = Array.Empty<string>();
    public int Intensity { get; set; }
    public int? Bitterness { get; set; }
    public int? Acidity { get; set; }
    public int? RoastLevel { get; set; }
    public int? Body { get; set; }

    public IEnumerable<string> Ranges { get; set; } = Array.Empty<string>();
    public IEnumerable<string> CupSizes { get; set; } = Array.Empty<string>();
    public IEnumerable<string> Flavors { get; set; }  = Array.Empty<string>();

    public string? PackagingType { get; set; }
}

public class Technology
{
    public string? Id { get; set; }
    public bool? IsSelected { get; set; }
    public Icon Media { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Url { get; set; } = default!;
}