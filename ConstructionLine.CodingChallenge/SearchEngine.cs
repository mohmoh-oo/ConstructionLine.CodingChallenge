using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly Dictionary<Guid, List<Shirt>> categorisedByColors;
        private readonly Dictionary<Guid, List<Shirt>> categorisedBySizes;
        public SearchEngine(List<Shirt> shirts)
        {
            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
            categorisedByColors = shirts.ToDictionaryLookups(shirt => shirt.Color.Id, Color.All.Select(color => color.Id).ToList());
            categorisedBySizes = shirts.ToDictionaryLookups(shirt => shirt.Size.Id, Size.All.Select(size => size.Id).ToList());
        }

        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            var shirtWithGivenColors = categorisedByColors.Search(options.Colors.Select(color => color.Id).ToList());
            var shirtWithGivenSizes = categorisedBySizes.Search(options.Sizes.Select(size => size.Id).ToList());

            return new SearchResults
            {
                Shirts = shirtWithGivenColors
                    .SelectMany(categorised => categorised.Value)
                    .Except(shirtWithGivenSizes.SelectMany(categorised => categorised.Value))
                    .ToList(), 
                ColorCounts = Color
                    .All
                    .Select(categorised => new ColorCount { Color = categorised, Count = shirtWithGivenColors.Count(categorised.Id) })
                    .ToList(), 
                SizeCounts = Size
                    .All
                    .Select(categorised => new SizeCount { Size = categorised, Count = shirtWithGivenSizes.Count(categorised.Id) })
                    .ToList()
            };
        }
    }

    internal static class ShirtsExtensions
    {
        internal static Dictionary<Guid, List<Shirt>> ToDictionaryLookups(this List<Shirt> shirts, Func<Shirt, Guid> categoriseFunc, List<Guid> categories)
        {
           var categorisedByGuids = shirts
                .GroupBy(categoriseFunc)
                .ToDictionary(
                    grouped => grouped.Key,
                    grouped => grouped.ToList());

           return categories
               .ToDictionary(
                   category => category,
                   category => categorisedByGuids.ContainsKey(category) ? categorisedByGuids[category].ToList() : new List<Shirt>());
        }

        internal static Dictionary<Guid, List<Shirt>> Search(this Dictionary<Guid, List<Shirt>> cateogrisedDictionary, List<Guid> searchGuids)
        => cateogrisedDictionary
            .Where(categorised => searchGuids
                .Any(searchGuid => searchGuid.Equals(categorised.Key)))
            .ToDictionary(categorised => categorised.Key, categorised => categorised.Value.ToList());

        internal static int Count(this Dictionary<Guid, List<Shirt>> cateogrisedDictionary, Guid searchGuid)
            => cateogrisedDictionary.Where(shirts => shirts.Key.Equals(searchGuid))
                .SelectMany(shirts => shirts.Value)
                .Count();
    }
}