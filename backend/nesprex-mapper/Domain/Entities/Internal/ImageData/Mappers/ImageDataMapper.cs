namespace Domain.Entities.Internal;

public static class ImageDataMapper
{
    public static IEnumerable<ImageData> GetDomainEntitiesImages(this IEnumerable<Technology> technologies)
    {
        var imagesData = new List<ImageData>();

        foreach (var technology in technologies)
        {
            imagesData.Add(new ImageData(
                new[] { "technologies" },
                technology.Name.ToKey(),
                technology.ImageUrl.ToKey()));

            foreach (var category in technology.Categories)
            {
                imagesData.Add(new ImageData(
                    new[] { "categories", technology.Name.ToKey() },
                    category.Name.ToKey(),
                    category.ImageUrl.ToKey()));

                foreach (var capsule in category.Capsules)
                {
                    imagesData.Add(new ImageData(
                        new[] { "capsules", technology.Name.ToKey(), category.Name.ToKey(), },
                        capsule.Name.ToKey(),
                        capsule.ImageUrl.ToKey()));
                }
            }
        }

        return imagesData;
    }
}