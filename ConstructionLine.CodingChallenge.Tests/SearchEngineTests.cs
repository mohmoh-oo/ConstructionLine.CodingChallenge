using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [TestCaseSource(nameof(TestCaseSource))]
        public void Test(Color[] colors, Size[] sizes)
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = colors.ToList(),
                Sizes = sizes.ToList()
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        public static IEnumerable<object[]> TestCaseSource()
        {
            yield return new object[] { new[] { Color.Red }, new Size[0] };
            yield return new object[] { new Color[] { }, new[] { Size.Large, Size.Medium } };
            yield return new object[] { new[] { Color.Red, Color.Black, Color.Blue, Color.White, Color.Yellow }, new[] { Size.Large, Size.Medium, Size.Small } };
        }
    }
}
