// using System.Text.Json;
//
// namespace ConsoleApp1.Services;
//
// public class FileWriterService
// {
//     public static void WriteJsonFilesToDesktop(
//         FilteredRawCategories filteredRawCategories,
//         IEnumerable<FilteredRawProduct> domainProducts,
//         string path)
//     {
//         var categoriesJson = JsonSerializer.Serialize(filteredRawCategories);
//         var productsJson = JsonSerializer.Serialize(domainProducts);
//
//         File.WriteAllText(Path.Combine(path, "categories.json"), categoriesJson);
//         File.WriteAllText(Path.Combine(path, "products.json"), productsJson);
//     }
// }