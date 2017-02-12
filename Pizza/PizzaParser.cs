using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pizza.Models;
using Pizza.Utils;

namespace Pizza
{
    public class PizzaParser
    {
        private static readonly Regex MetadataRegex = new Regex(@"(?<line>\d+) (?<column>\d+) (?<ingredient>\d+) (?<slicesize>\d+)");

        private static ParsingResult Parse(TextReader textReader)
        {
            var metadata = textReader.ReadLine();

            var m = MetadataRegex.Match(metadata);

            var minIngredient = int.Parse(m.Groups["ingredient"].Value);
            var maxSliceSize = int.Parse(m.Groups["slicesize"].Value);
            var nbColumn = int.Parse(m.Groups["column"].Value);
            var nbLine = int.Parse(m.Groups["line"].Value);

            var ingredients = new Ingredient[nbColumn * nbLine];

            Enumerable
                .Range(0, nbLine)
                .ForEach(row =>
                {
                    var lineContent = textReader.ReadLine();
                    lineContent.ForEach((c, col) =>
                    {
                        ingredients[col + row * nbColumn] = c.ToIngredient();
                    });
                });

            if (ingredients.Any(i => i == Ingredient.Unset))
            {
                throw new InvalidOperationException();
            }

            var pizza = new Models.Pizza(nbLine, nbColumn, ingredients);

            return new ParsingResult(pizza, new SliceConstraints(maxSliceSize, minIngredient));
        }

        public static ParsingResult Parse(string content)
        {
            return Parse(new StringReader(content));
        }

        public static ParsingResult ParseOneLineString(string oneLineString, string separator = ",")
        {
            var content = oneLineString.Replace(separator, Environment.NewLine);
            return Parse(new StringReader(content));
        }

        public static ParsingResult ParseFile(string filePath)
        {
            return Parse(new StreamReader(File.OpenRead(filePath)));
        }
    }
}