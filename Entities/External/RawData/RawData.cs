namespace Domain.Entities.External;

public class RawData
{
    public List<ExternalNespressoCategory> Categories { get; set; }
    public List<Technology> EnabledTechnologies { get; set; }
    public List<Product> Products { get; set; }

    public RawData(
        List<ExternalNespressoCategory> categories,
        List<Technology> enabledTechnologies,
        List<Product> products)
    {
        Categories = categories;
        EnabledTechnologies = enabledTechnologies;
        Products = products;
    }
}