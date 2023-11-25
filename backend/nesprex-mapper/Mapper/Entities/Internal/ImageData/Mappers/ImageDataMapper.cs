namespace Mapper.Entities.Internal.ImageData.Mappers;

public static class ImageDataMapper
{
    public static IEnumerable<Domain.Entities.ImageData> GetDomainEntitiesImages(
        this IEnumerable<Domain.Entities.Technology> technologies,
        string? imageUrl = null)
    {
        var imagesData = new List<Domain.Entities.ImageData>();

        foreach (var technology in technologies)
        {
            imagesData.Add(new Domain.Entities.ImageData(
                new[] { "technologies" },
                technology.Name.ToKey(),
                $"{imageUrl?.TrimEnd('/')}{technology.ImageUrl.ToKey()}"));

            foreach (var category in technology.Categories)
            {
                imagesData.Add(new Domain.Entities.ImageData(
                    new[] { "categories", technology.Name.ToKey() },
                    category.Name.ToKey(),
                    category.ImageUrl.ToKey()));

                foreach (var capsule in category.Capsules)
                {
                    imagesData.Add(new Domain.Entities.ImageData(
                        new[] { "capsules", technology.Name.ToKey(), category.Name.ToKey(), },
                        capsule.Name.ToKey(),
                        $"{imageUrl?.TrimEnd('/')}{capsule.ImageUrl.ToKey()}"));
                }
            }
        }

        return imagesData;
    }
}