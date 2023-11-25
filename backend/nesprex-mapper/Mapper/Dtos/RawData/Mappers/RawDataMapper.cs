using Mapper.Dtos.FilteredRawCategory.Mappers;

namespace Mapper.Dtos.RawData.Mappers;

public static class RawDataMapper
{
    public static IEnumerable<Entities.Technology> MapRawDataToDomainEntities(this RawData data)
    {
        // Extract relevant properties from enabled technologies
        var filteredRawTechnologies = data.EnabledTechnologies
            .Select(x => new { x.Id, x.Name, x.Media.Url });

        // Convert raw category data to filtered / grouped categories
        var categoryTypes = data.Categories
            .ToFilteredCategories();

        var mappedTechnologies = filteredRawTechnologies.Select(rawTechnology =>
        {
            // Find capsules related to the current technology
            var capsulesOfTechnology = data.Products
                .Where(rawProduct => rawProduct.Technologies.Contains(rawTechnology.Id));

            // Find categories related to the capsules of the current technology
            var categoriesOfTechnology = categoryTypes
                .Capsules
                .Where(c => capsulesOfTechnology.Any(tc => tc.Ranges.Contains(c.Id)));

            // Map categories with their associated capsules
            var mappedCategoriesCapsules = categoriesOfTechnology
                .MapCategoriesWithCapsules(capsulesOfTechnology, categoryTypes);

            return new Entities.Technology(
                rawTechnology.Name,
                rawTechnology.Url,
                mappedCategoriesCapsules);
        });

        return mappedTechnologies;
    }

    private static IEnumerable<Entities.Category> MapCategoriesWithCapsules(
        this IEnumerable<FilteredRawCategory.FilteredRawCategory> categoriesOfCapsules,
        IEnumerable<Product> rawCapsules,
        GroupedFilteredRawCategories categoryTypes)
    {
        return categoriesOfCapsules.Select(categoryCapsule =>
        {
            // Get capsules of the current category
            var capsulesOfCategory = rawCapsules
                .Where(x => x.Ranges.Contains(categoryCapsule.Id))
                .ToList();

            // Map capsules associated with the current category
            var mappedCapsules = capsulesOfCategory.MapCapsules(categoryTypes);

            // Map category
            var mappedCategory = new Entities.Category(
                categoryCapsule.Name,
                categoryCapsule.Description,
                categoryCapsule.ImageUrl,
                mappedCapsules);

            return mappedCategory;
        });
    }

    private static IEnumerable<Entities.Capsule> MapCapsules(
        this IEnumerable<Product> capsulesOfCategory,
        GroupedFilteredRawCategories categoryTypes)
    {
        return capsulesOfCategory.Select(rawCapsule =>
        {
            // Map cup sizes names based on the intersection categories
            // of type CupSize and product.CupSizes
            var cupSizesNames = categoryTypes.CupSizes
                .Where(rawCupSize => rawCapsule.CupSizes.Contains(rawCupSize.Id))
                .Select(x => x.Name)
                .ToList();

            // For ristretto capsules, if there are no cup sizes, add "Ristretto"
            // because Nespresso JSON does not have cup sizes for ristretto capsules
            if (!cupSizesNames.Any() && rawCapsule.Ranges.Any(r => r.Contains("ristretto")))
            {
                cupSizesNames = new List<string> { "Ristretto" };
            }

            // Map flavors names based on the intersection categories
            // of type Flavors and product.Flavors
            var flavors = categoryTypes.Flavors
                .Where(rawFlavor => rawCapsule.Flavors.Contains(rawFlavor.Id))
                .Select(x => x.Name);

            // Map label names based on the intersection of categories
            // of type Labels and product.Ranges (which is a list of category ids)
            var labels = categoryTypes.Labels
                .Where(rawLabel => rawCapsule.Ranges.Contains(rawLabel.Id))
                .Select(x => x.Name);

            var mappedInfo = new Entities.Info(
                rawCapsule.Intensity,
                rawCapsule.Bitterness,
                rawCapsule.Acidity,
                rawCapsule.RoastLevel,
                rawCapsule.Body,
                labels.ToList(),
                cupSizesNames,
                flavors);

            return new Entities.Capsule(
                rawCapsule.Name,
                rawCapsule.Description,
                rawCapsule.Price,
                mappedInfo,
                rawCapsule.Image.Url);
        });
    }
}