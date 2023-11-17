namespace ConsoleApp1.Domain.External;

public static class RawDataMapper
{
    public static IEnumerable<InternalEntities.Category> MapRawDataToDomainEntities(this RawData data)
    {
        // Transform raw data categories into a more refined set of filtered categories
        var filteredRawCategories = data.Categories.ToDomainCategory().ToList();

        // Group filtered domain categories by their types
        var categoryTypes = filteredRawCategories.GroupBy(category => category.Type).ToList();

        var mappedCategories = new List<InternalEntities.Category>();

        // Iterate through each category of capsule type
        foreach (var capsuleCategories in categoryTypes.Where(x => x.Key == CategoryType.Capsules))
        {
            // Iterate through each specific capsule category
            foreach (var capsuleCategory in capsuleCategories)
            {
                // Initialize a list to store the mapped capsules for the current category
                var mappedCapsules = new List<InternalEntities.Capsule>();

                // Retrieve capsules belonging to the current category
                var capsuleCategoryCapsules = data.Products
                    .Where(x => x.Ranges.Contains(capsuleCategory.Id))
                    .ToList();

                // Iterate through each capsule in the capsule category and process its details
                foreach (var capsuleInCategory in capsuleCategoryCapsules)
                {
                    // Get the names of flavors associated with the current capsule
                    var flavorsNames = categoryTypes
                        .GetNamesByCategoryType( CategoryType.Flavor, capsuleInCategory.Flavors);

                    // Get the names of labels associated with the current capsule
                    var labelsNames = categoryTypes
                        .GetNamesByCategoryType(CategoryType.Label, capsuleInCategory.Ranges);

                    // Get the names of cup sizes associated with the current capsule
                    var cupSizesNames = categoryTypes
                        .GetNamesByCategoryType(CategoryType.CupSize, capsuleInCategory.CupSizes)
                        .ToList();

                    // For ristretto capsules, if there are no cup sizes, add "Ristretto"
                    // because Nespresso JSON does not have cup sizes for ristretto capsules
                    if (!cupSizesNames.Any() && capsuleInCategory.Ranges.Any(x => x.Contains("ristretto")))
                    {
                        cupSizesNames = new List<string> { "Ristretto" };
                    }

                    // Create an Info object, mapping details about the current capsule
                    var mappedInfo = new InternalEntities.Info(
                        capsuleInCategory.Intensity,
                        capsuleInCategory.Bitterness,
                        capsuleInCategory.Acidity,
                        capsuleInCategory.RoastLevel,
                        capsuleInCategory.Body,
                        labelsNames,
                        cupSizesNames,
                        flavorsNames);

                    // Create a Capsule object, mapping details about the current capsule
                    // and associating it with the previously mapped Info object
                    var mappedCapsule = new InternalEntities.Capsule(
                        capsuleInCategory.Name,
                        capsuleInCategory.Description,
                        capsuleInCategory.Image.Url,
                        capsuleInCategory.Price,
                        mappedInfo);

                    mappedCapsules.Add(mappedCapsule);
                }

                // Identify unique technology IDs associated with capsules in the current category
                var technologiesIds = capsuleCategoryCapsules
                    .SelectMany(x => x.Technologies)
                    .Distinct()
                    .ToList();

                // Ensure that a category does not have more than one technology associated with its capsules
                if (technologiesIds.Count > 1)
                {
                    throw new InvalidOperationException(
                        $"The category '{capsuleCategory.Name}' has more technologies than allowed.");
                }

                // Retrieve the technology information for the category
                var technology = data.EnabledTechnologies.First(x => x.Id == technologiesIds.First());
                var mappedTechnology = new InternalEntities.Technology(technology.Name);

                // Create a new Category, mapping the capsules, their information, and the technology to the category
                var mappedCategory = new InternalEntities.Category(
                    capsuleCategory.Name,
                    capsuleCategory.Description,
                    mappedTechnology,
                    mappedCapsules.ToList());

                // Add the newly mapped category to the result collection
                mappedCategories.Add(mappedCategory);
            }
        }

        return mappedCategories.ToList();
    }

    private static IEnumerable<string> GetNamesByCategoryType(
        this IEnumerable<IGrouping<CategoryType, FilteredRawCategory>> categoryTypes,
        CategoryType categoryType,
        IEnumerable<string> categoryTypeNames)
    {
        return categoryTypes
            .Where(x => x.Key == categoryType)
            .SelectMany(x => x)
            .Where(x => categoryTypeNames.Contains(x.Id))
            .Select(x => x.Name)
            .ToList();
    }
}