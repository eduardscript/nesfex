namespace ConsoleApp1.Domain.External;

public static class FilteredRawCategoryMapper
{
    public static IEnumerable<FilteredRawCategory> ToDomainCategory(this IEnumerable<ExternalNespressoCategory> category)
    {
        return category.Select(x => x.ToDomainCategory());
    }

    private static FilteredRawCategory ToDomainCategory(this ExternalNespressoCategory category)
    {
        return new FilteredRawCategory
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.Icon?.Url!,
            Type = category.SuperCategories.ToCategoryType(category.Id),
        };
    }

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