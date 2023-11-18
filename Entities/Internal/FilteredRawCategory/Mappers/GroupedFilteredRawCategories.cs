namespace Domain.Entities.Internal;

public class GroupedFilteredRawCategories
{
    public IEnumerable<FilteredRawCategory> Capsules { get; set; }

    public IEnumerable<FilteredRawCategory> CupSizes { get; set; }

    public IEnumerable<FilteredRawCategory> Flavors { get; set; }

    public IEnumerable<FilteredRawCategory> Labels { get; set; }
}