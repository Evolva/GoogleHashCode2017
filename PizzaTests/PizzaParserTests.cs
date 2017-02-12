using System;
using NFluent;
using NUnit.Framework;
using Pizza;
using Pizza.Models;

namespace PizzaTests
{
    public class PizzaParserTests
    {
        [Test]
        public void PizzaParser_Should_Parse_Sample_And_Print_It_Back()
        {
            const string pizzaString = @"6 7 1 5
TMMMTTT
MMMMTMM
TTMTTMT
TMMTMMM
TTTTTTM
TTTTTTM
";
            var parsingResult = PizzaParser.Parse(pizzaString);

            Check.That(parsingResult.Constraints.MaximumSliceSize).IsEqualTo(5);
            Check.That(parsingResult.Constraints.MinimumIngredientCount).IsEqualTo(1);

            Check.That(parsingResult.Pizza.Line).IsEqualTo(6);
            Check.That(parsingResult.Pizza.Column).IsEqualTo(7);

            Check.That(parsingResult.Pizza.GetIngredientAt(new Coordinate(0, 0))).IsEqualTo(Ingredient.Tomato);
            Check.That(parsingResult.Pizza.GetIngredientAt(new Coordinate(1, 0))).IsEqualTo(Ingredient.Mushroom);
            Check.That(parsingResult.Pizza.GetIngredientAt(new Coordinate(2, 0))).IsEqualTo(Ingredient.Mushroom);

            Check.That(parsingResult.ToInputString()).IsEqualTo(pizzaString);
        }

        [Test]
        public void PizzaParser_Should_Throw_Exception_When_Parsing_Invalid_Pizza()
        {
            Check.ThatCode(() => PizzaParser.ParseOneLineString("3 4 1 4,TMM,MMM,MMT")).Throws<InvalidOperationException>();
        }
    }
}
