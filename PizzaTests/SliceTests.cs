using System;
using System.Linq;
using NFluent;
using NUnit.Framework;
using Pizza;
using Pizza.Models;

namespace PizzaTests
{
    public class SliceTests
    {
        [Test]
        public void ToStringTest()
        {
            const string expectedString =
@"TMM
MMM
MMT
";
            Slice slice = PizzaParser.ParseOneLineString("3 3 1 4,TMM,MMM,MMT").Pizza;

            Check.That(slice.ToString()).IsEqualTo(expectedString);
        }

        [TestCase("3 3 1 4,TMM,MMM,MMT", 7, 2, false)]
        [TestCase("3 3 3 9,TMM,MMM,MMT", 7, 2, false)]
        [TestCase("3 3 2 9,TMM,MMM,MMT", 7, 2, true)]
        [TestCase("3 3 3 9,MTT,TTT,TTM", 2, 7, false)]
        public void Slice_Properties_Tests(string pizzaString, int expectedMushroom, int expectedTomato, bool shouldBeValid)
        {
            var parseResult = PizzaParser.ParseOneLineString(pizzaString);
            Slice slice = parseResult.Pizza;

            Check.That(slice.Width).IsEqualTo(3);
            Check.That(slice.Height).IsEqualTo(3);
            Check.That(slice.Size).IsEqualTo(9);

            Check.That(slice.MushroomCount).IsEqualTo(expectedMushroom);
            Check.That(slice.TomatoCount).IsEqualTo(expectedTomato);

            Check.That(slice.MushroomCount + slice.TomatoCount).IsEqualTo(slice.Size);
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 1)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 2)]
        public void Valid_Horizontal_Cut_Test(string pizzaString, int firstSliceSize)
        {
            Slice slice = PizzaParser.ParseOneLineString(pizzaString).Pizza;
            
            var slices = slice.Cut(Direction.Horizontal, firstSliceSize);

            var topSlice = slices.Item1;
            Check.That(topSlice.Width).IsEqualTo(slice.Width);
            Check.That(topSlice.Height).IsEqualTo(firstSliceSize);

            var bottomSlice = slices.Item2;
            Check.That(bottomSlice.Width).IsEqualTo(slice.Width);
            Check.That(bottomSlice.Height).IsEqualTo(slice.Height - firstSliceSize);

            Check.That(topSlice.ToString() + bottomSlice.ToString())
                .IsEqualTo(slice.ToString());
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 1)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 2)]
        public void Valid_Vertical_Cut_Test(string pizzaString, int firstSliceSize)
        {
            Slice slice = PizzaParser.ParseOneLineString(pizzaString).Pizza;

            var slices = slice.Cut(Direction.Vertical, firstSliceSize);

            var leftSlice = slices.Item1;
            Check.That(leftSlice.Height).IsEqualTo(slice.Height);
            Check.That(leftSlice.Width).IsEqualTo(firstSliceSize);

            var rightSlice = slices.Item2;
            Check.That(rightSlice.Height).IsEqualTo(slice.Height);
            Check.That(rightSlice.Width).IsEqualTo(slice.Width - firstSliceSize);

            var leftSliceString = leftSlice.ToString().Split(new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            var rightSliceString = rightSlice.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var mergedString = leftSliceString.Zip(rightSliceString, (left, right) => left + right + Environment.NewLine)
                    .Aggregate(string.Empty, (accu, str) => accu + str);
            Check.That(mergedString).IsEqualTo(slice.ToString());
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Vertical,0)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Vertical,4)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Horizontal,0)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Horizontal,3)]
        public void Invalid_Cut_Should_Throw(string pizzaString, Direction direction, int firstSliceSize)
        {
            Slice slice = PizzaParser.ParseOneLineString(pizzaString).Pizza;

            Check.ThatCode(() =>
                slice.Cut(direction, firstSliceSize))
            .Throws<ArgumentOutOfRangeException>();
        }
    }
}