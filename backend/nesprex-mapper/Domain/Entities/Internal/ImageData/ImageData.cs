namespace Domain.Entities.Internal;

public record ImageData(
    IEnumerable<string> Folders,
    string FileName,
    string ImageUrl);