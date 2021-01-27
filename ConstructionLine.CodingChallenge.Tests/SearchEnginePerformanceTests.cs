using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConstructionLine.CodingChallenge.Tests.SampleData;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEnginePerformanceTests : SearchEngineTestsBase
    {
        private List<Shirt> _shirts;
        private SearchEngine _searchEngine;

        [SetUp]
        public void Setup()
        {
            
            var dataBuilder = new SampleDataBuilder(50000);

            _shirts = dataBuilder.CreateShirts();

            _searchEngine = new SearchEngine(_shirts);
        }

        [TestCaseSource(nameof(TestCaseSource))]
        public void PerformanceTest(Color[] colors, Size[] sizes)
        {
            var sw = new Stopwatch();
            sw.Start();

            var options = new SearchOptions
            {
                Colors = colors.ToList(), Sizes = sizes.ToList()
            };

            var results = _searchEngine.Search(options);

            sw.Stop();
            Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");

            AssertResults(results.Shirts, options);
            AssertSizeCounts(_shirts, options, results.SizeCounts);
            AssertColorCounts(_shirts, options, results.ColorCounts);
        }

        public static IEnumerable<object[]> TestCaseSource()
        {
            yield return new object[] { new[] { Color.Red }, new Size[0] };
            yield return new object[] { new Color[] { }, new [] { Size.Large, Size.Medium } };
            yield return new object[] { new[] { Color.Red, Color.Black, Color.Blue, Color.White, Color.Yellow }, new [] { Size.Large, Size.Medium, Size.Small } };
        }
    }
}
