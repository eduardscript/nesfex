public class FilteredRawProduct
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public string Technology { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public double Price { get; set; }

    public int SalesMultiple { get; set; }

    public int UnitQuantity { get; set; }

    public bool InStock { get; set; }

    public int Intensity { get; set; }
    
    public int Bitterness { get; set; }

    public int Acidity { get; set; }
    
    public int RoastLevel { get; set; }
    
    public int Body { get; set; }

    public FilteredRawCategory MainCategory { get; set; } = default!;

    public FilteredRawCategory? CupSizeCategory { get; set; }

    public IEnumerable<FilteredRawCategory>? AromaticCategories { get; set; }
    
    public IEnumerable<FilteredRawCategory> LabelCategories { get; set; } = default!;
}