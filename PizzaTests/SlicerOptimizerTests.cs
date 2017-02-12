using NFluent;
using NUnit.Framework;
using Pizza;

namespace PizzaTests
{
    public class SlicerOptimizerTests
    {
        [TestCase("3 3 1 4,TMM,MMM,MMM", 4)]
        [TestCase("3 3 1 4,TMM,MMM,MMT", 8)]
        [TestCase("3 3 1 4,TMM,MTM,MMT", 9)]
        public void Test_MaxPossiblePoints(string pizzaString, int expectedMaxPossiblePoints)
        {
            var parseResult = PizzaParser.ParseOneLineString(pizzaString);
            var slicerOptimizer = new SlicerOptimizer(parseResult.Constraints);
            
            Check.That(slicerOptimizer.MaxPossiblePoints(parseResult.Pizza))
                .IsEqualTo(expectedMaxPossiblePoints);
        }

        [TestCase("3 3 1 4,TMM,MMM,MMT", false)]
        [TestCase("3 3 3 9,TMM,MMM,MMT", false)]
        [TestCase("3 3 2 9,TMM,MMM,MMT", true)]
        [TestCase("3 3 3 9,MTT,TTT,TTM", false)]
        public void IsValid_Test(string pizzaString, bool shouldBeValid)
        {
            var parseResult = PizzaParser.ParseOneLineString(pizzaString);
            var slicerOptimizer = new SlicerOptimizer(parseResult.Constraints);
            
            Check.That(slicerOptimizer.IsValid(parseResult.Pizza)).IsEqualTo(shouldBeValid);
        }
    }
}