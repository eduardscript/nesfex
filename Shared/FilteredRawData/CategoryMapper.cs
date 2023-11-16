public static class CategoryMapper
{
    public static IEnumerable<FilteredRawCategory> ToDomainCategory(this IEnumerable<ExternalNespressoCategory> category, RawData rawData)
    {
        return category.Select(x => x.ToDomainCategory(rawData));
    }

    public static FilteredRawCategory ToDomainCategory(
        this ExternalNespressoCategory category,
        RawData rawData)
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

    public static CategoryType ToCategoryType(this IEnumerable<string> superCategories, string categoryId)
    {
        var category = superCategories.First();

        if (categoryId.Contains("capsule-range-label"))
        {
            return CategoryType.Label;
        }
        
        if (category.Contains("capsule-cupSize"))
        {
            return CategoryType.CupSize;
        }

        if (category.Contains("capsule-range"))
        {
            return CategoryType.Capsules;
        }

        if (category.Contains("capsule-aromatic"))
        {
            return CategoryType.Flavor;
        }
      
        throw new Exception("Unknown category type");
    }
}