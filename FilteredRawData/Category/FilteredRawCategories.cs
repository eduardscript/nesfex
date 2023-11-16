public class FilteredRawCategories
{
    public IEnumerable<FilteredRawCategory> AromaticCategories { get; private set; }
    public IEnumerable<FilteredRawCategory> CupSizeCategories { get; private set; }
    public IEnumerable<FilteredRawCategory> LabelCategories { get; private set; }
    public IEnumerable<FilteredRawCategory> CapsuleCategories { get; private set; }

    public FilteredRawCategories(IEnumerable<FilteredRawCategory> categories)
    {
        var filteredRawCategories = categories.ToArray();

        AromaticCategories = filteredRawCategories.Where(x => x.Type == CategoryType.Flavor);
        CupSizeCategories = filteredRawCategories.Where(x => x.Type == CategoryType.CupSize);
        LabelCategories = filteredRawCategories.Where(x => x.Type == CategoryType.Label);
        CapsuleCategories = filteredRawCategories.Where(x => x.Type == CategoryType.Capsules);
    }
}