public static class ProductMapper
{
    public static IEnumerable<FilteredRawProduct> ToDomainProduct(
        this IEnumerable<Product> products,
        FilteredRawCategories categories,
        IEnumerable<Technology> technologies)
    {
        return products.Select(x => x.ToDomainProduct(categories, technologies));
    }
    
    public static FilteredRawProduct ToDomainProduct(
        this Product product,
        FilteredRawCategories categories,
        IEnumerable<Technology> technologies)
    {
        var domainProduct = new FilteredRawProduct
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Headline,
            Technology = technologies.First(x => x.Id == product.Technologies.First()).Name,
            ImageUrl = product.Image.Url,
            Price = product.Price,
            SalesMultiple = product.SalesMultiple,
            UnitQuantity = product.UnitQuantity,
            InStock = product.InStock,
            Intensity = product.Intensity,
            Bitterness = product.Bitterness ?? 0,
            Acidity = product.Acidity ?? 0,
            RoastLevel = product.RoastLevel ?? 0,
            Body = product.Body ?? 0,
            MainCategory = categories.CapsuleCategories.First(category => product.Ranges.Contains(category.Id)),
            CupSizeCategory = categories.CupSizeCategories.FirstOrDefault(category =>
            {
                if (!product.CupSizes.Any() && product.Ranges.Any(x => x.Contains("ristretto")))
                {
                    return category is { Name: "Ristretto" };
                }

                return product.CupSizes.Contains(category.Id);
            }),
            AromaticCategories = categories.AromaticCategories.Where(category => product.Flavors.Contains(category.Id)).ToList(),
            LabelCategories = categories.LabelCategories.Where(category => product.Ranges.Contains(category.Id)).ToList(),
        };

        return domainProduct;
    }
}