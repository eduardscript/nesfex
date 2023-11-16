using ConsoleApp1.Mappers.Entities;
using ConsoleApp1.Services;

public class Program
{
    static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    const string VertuoJsonPath = @"C:\Users\edscriptdev\Desktop\New folder\data\vertuo.json";
    const string OriginalJsonPath = @"C:\Users\edscriptdev\Desktop\New folder\data\original.json";

    public static async Task Main(string[] args)
    {
        var mergedData = await RawDataService.GetRawData(VertuoJsonPath, OriginalJsonPath);

        var mappedData = MapRawDataToDomainEntities(mergedData);
    }

    public static IEnumerable<Entities.Category> MapRawDataToDomainEntities(RawData data)
    {
        // Map categories
        var filteredRawCategories = data.Categories.ToDomainCategory().ToList();
        var categoryTypes = filteredRawCategories.GroupBy(x => x.Type).ToList();

        var mappedCategories = new List<Entities.Category>();

        foreach (var capsuleCategories in categoryTypes.Where(x => x.Key == CategoryType.Capsules))
        {
            // For each category of capsule type 
            foreach (var capsuleCategory in capsuleCategories)
            {
                var mappedCapsules = new List<Entities.Capsule>();

                var capsuleCategoryCapsules = data.Products
                    .Where(x => x.Ranges.Contains(capsuleCategory.Id))
                    .ToList();

                // For each capsule in the category
                foreach (var capsuleInCateogry in capsuleCategoryCapsules)
                {
                    // Get the capsule flavors
                    var flavorsNames = GetNamesByCategoryType(
                        categoryTypes,
                        CategoryType.Flavor,
                        capsuleInCateogry.Flavors);

                    // Get the capsule labels 
                    var labelsNames = GetNamesByCategoryType(
                        categoryTypes,
                        CategoryType.Label,
                        capsuleInCateogry.Ranges);

                    // Get the capsule cup sizes
                    var cupSizesNames = GetNamesByCategoryType(
                        categoryTypes,
                        CategoryType.CupSize,
                        capsuleInCateogry.CupSizes
                        ).ToList();

                    // For ristretto capsules, if there are no cup sizes, add ristretto
                    // because nespresso json does not have cup sizes for ristretto capsules
                    if (!cupSizesNames.Any() && capsuleInCateogry.Ranges.Any(x => x.Contains("ristretto")))
                    {
                        cupSizesNames = new List<string> { "Ristretto" };
                    }

                    var mappedInfo = new Entities.Info(
                        capsuleInCateogry.Intensity,
                        capsuleInCateogry.Bitterness,
                        capsuleInCateogry.Acidity,
                        capsuleInCateogry.RoastLevel,
                        capsuleInCateogry.Body,
                        labelsNames,
                        cupSizesNames,
                        flavorsNames);

                    // Add the capsule to the list
                    var mappedCapsule = new Entities.Capsule(
                        capsuleInCateogry.Name,
                        capsuleInCateogry.Description,
                        capsuleInCateogry.Image.Url,
                        capsuleInCateogry.Price,
                        mappedInfo);

                    mappedCapsules.Add(mappedCapsule);
                }

                // Map technologies for each capsule category
                var technologiesIds = capsuleCategoryCapsules
                    .SelectMany(x => x.Technologies)
                    .Distinct()
                    .ToList();

                if (technologiesIds.Count > 1)
                {
                    throw new Exception($"The number of technologies is greater than default (1)");
                }

                var technology = data.EnabledTechnologies.First(x => x.Id == technologiesIds.First());

                var mappedTechnology = new Entities.Technology(technology.Name);

                var mappedCategory = new Entities.Category(
                    capsuleCategory.Name,
                    capsuleCategory.Description,
                    mappedTechnology, mappedCapsules);

                mappedCategories.Add(mappedCategory);
            }
        }

        return mappedCategories.ToList();
    }

    private static IEnumerable<string> GetNamesByCategoryType(
        IEnumerable<IGrouping<CategoryType, FilteredRawCategory>> categoryTypes,
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