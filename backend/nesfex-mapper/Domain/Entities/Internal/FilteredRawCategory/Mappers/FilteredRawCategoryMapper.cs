namespace Domain.Entities.Internal;

public static class FilteredRawCategoryMapper
{
    public static GroupedFilteredRawCategories ToFilteredCategories(
        this IEnumerable<ExternalNespressoCategory> categories)
    {
        var filteredRawCategories = categories
            .Select(x => x.ToFilteredRawCategory())
            .ToList();

        return new()
        {
            Capsules = filteredRawCategories.GetFilteredCategories(CategoryType.Capsules),
            CupSizes = filteredRawCategories.GetFilteredCategories(CategoryType.CupSize),
            Flavors = filteredRawCategories.GetFilteredCategories(CategoryType.Flavor),
            Labels = filteredRawCategories.GetFilteredCategories(CategoryType.Label),
        };
    }

    private static IEnumerable<FilteredRawCategory> GetFilteredCategories(
        this IEnumerable<FilteredRawCategory> categories,
        CategoryType categoryType) => categories.Where(x => x.Type == categoryType);

    private static FilteredRawCategory ToFilteredRawCategory(this ExternalNespressoCategory category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description,
        ImageUrl = category.Icon?.Url!,
        Type = category.SuperCategories.ToCategoryType(category.Id),
    };

    private static CategoryType ToCategoryType(this IEnumerable<string> superCategories, string categoryId)
    {
        var category = superCategories.First();

        return category switch
        {
            _ when categoryId.Contains("capsule-range-label") => CategoryType.Label,
            _ when category.Contains("capsule-cupSize") => CategoryType.CupSize,
            _ when category.Contains("capsule-range") => CategoryType.Capsules,
            _ when category.Contains("capsule-aromatic") => CategoryType.Flavor,
            _ => throw new Exception("Unknown category type")
        };
    }
}