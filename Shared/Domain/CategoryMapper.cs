namespace ConsoleApp1.Shared.Domain;

public static class CategoryMapper
{
//     public static IEnumerable<Entities.Category> ToEntity(this IEnumerable<FilteredRawProduct> filteredRawProducts)
//     {
//         var rawCategories = filteredRawProducts.GroupBy(x => new { x.MainCategory, x.Technology });
//
//         return rawCategories.Select(rawProductGroup => new Entities.Category(
//             rawProductGroup.Key.MainCategory.Name,
//             rawProductGroup.Key.MainCategory.Description,
//             new Entities.Technology(rawProductGroup.Key.Technology),
//             rawProductGroup.Select(capsule => new Entities.Capsule(
//                 capsule.Name,
//                 capsule.Description,
//                 capsule.ImageUrl,
//                 capsule.Price,
//                 // new Entities.Info(
//                 //     capsule.Intensity,
//                 //     capsule.Bitterness,
//                 //     capsule.Acidity,
//                 //     capsule.RoastLevel,
//                 //     capsule.Body,
//                 //     capsule.LabelCategories.Select(x => x.Name)
//                 // ),
//                 // capsule.AromaticCategories?.Select(x => new Entities.Flavor(x.Name))
//             ))));
// }

// public static IEnumerable<Entities.Category> ToEntity(this IEnumerable<FilteredRawProduct> filteredRawProducts)
// {
//     var rawCategories = filteredRawProducts.GroupBy(x => new { x.MainCategory, x.Technology });
//
//     List<Entities.Category> categories = new List<Entities.Category>();
//     foreach (var rawProduct in rawCategories)
//     {
//         var capsulesOfCategory = rawProduct.Select(capsule => new Entities.Capsule(
//             capsule.Name,
//             capsule.Description,
//             capsule.ImageUrl,
//             capsule.Price,
//             new Entities.Info(capsule.Intensity, capsule.Bitterness, capsule.Acidity, capsule.RoastLevel,
//                 capsule.Body),
//             capsule.AromaticCategories?.Select(x => new Entities.Flavor(x.Name))));
//
//         var category = new Entities.Category(
//             rawProduct.Key.MainCategory.Name,
//             rawProduct.Key.MainCategory.Description,
//             new(rawProduct.Key.Technology),
//             capsulesOfCategory);
//
//         categories.Add(category);
//     }
//
//     return categories;
// }

}