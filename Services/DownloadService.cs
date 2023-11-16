// using System.Text;
// using System.Text.Json;
//
// namespace ConsoleApp1.Services;
//
// public static class DownloadService
// {
//     public static async Task DownloadImages(IEnumerable<Entities.Category> filteredRawCategories)
//     {
//         var httpClient = new HttpClient();
//         httpClient.BaseAddress = new Uri("http://localhost:7071/api/");
//
//         var domainProductsArray = domainProducts.ToArrSAay();
//
//         // var domainProductsIterated = domainProducts as FilteredRawProduct[] ?? domainProducts.ToArray();
//         // var mainCategory = domainProductsIterated.Select(x => x.MainCategory);
//         // var cupSizeCategory = domainProductsIterated.Select(x => x.CupSizeCategory);
//         // var labelCategories = domainProductsIterated.SelectMany(x => x.LabelCategories);
//         // var aromaticCategories =
//         //     domainProductsIterated.SelectMany(x => x.AromaticCategories ?? new List<FilteredRawCategory>());
//         //
//         // var allCategories = mainCategory
//         //     .Concat(cupSizeCategory)
//         //     .Concat(labelCategories)
//         //     .Concat(aromaticCategories)
//         //     .Distinct();
//
//         foreach (var category in allCategories.Where(c => c != null && !string.IsNullOrWhiteSpace(c.ImageUrl)))
//         {
//             var directory = category.Type == CategoryType.CupSize ? category.Type.ToString() : "Categories";
//
//             await DownloadImageAsync(
//                 httpClient,
//                 category.Name,
//                 category.ImageUrl,
//                 new[] { directory });
//         }
//
//         foreach (var capsule in domainProductsArray)
//         {
//             await DownloadImageAsync(httpClient,
//                 capsule.Name,
//                 capsule.ImageUrl,
//                 new[] { CategoryType.Capsules.ToString(), capsule.MainCategory.Name });
//         }
//
//         Console.ReadKey();
//     }
//
//     private static async Task DownloadImageAsync(HttpClient httpClient,
//         string fileName,
//         string imageUrl,
//         IEnumerable<string> directories)
//     {
//         try
//         {
//             var newProperty = new
//             {
//                 FileName = fileName,
//                 FilePaths = directories,
//                 ImageUrl = imageUrl,
//             };
//
//             var newJson = JsonSerializer.Serialize(newProperty);
//
//             await httpClient.PostAsync("download", new StringContent(newJson, Encoding.UTF8, "application/json"))
//                 .ConfigureAwait(false);
//
//             Console.WriteLine("Downloaded image from " + newProperty.FileName);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Failed to download image from {imageUrl}. Error: {ex.Message}");
//         }
//     }
// }