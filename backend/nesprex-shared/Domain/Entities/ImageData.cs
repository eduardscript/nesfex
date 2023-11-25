namespace Domain.Entities;

public record ImageData(
    IEnumerable<string> Folders,
    string FileName,
    string ImageUrl);