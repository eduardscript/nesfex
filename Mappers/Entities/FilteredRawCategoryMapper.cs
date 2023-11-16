namespace ConsoleApp1.Mappers.Entities;

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