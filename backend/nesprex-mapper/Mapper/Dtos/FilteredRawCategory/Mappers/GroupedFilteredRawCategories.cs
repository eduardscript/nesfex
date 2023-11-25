namespace Mapper.Dtos.FilteredRawCategory.Mappers;

public class GroupedFilteredRawCategories
{
    public IEnumerable<FilteredRawCategory> Capsules { get; set; } = default!;

    public IEnumerable<FilteredRawCategory> CupSizes { get; set; } = default!;

    public IEnumerable<FilteredRawCategory> Flavors { get; set; } = default!;

    public IEnumerable<FilteredRawCategory> Labels { get; set; } = default!;
}