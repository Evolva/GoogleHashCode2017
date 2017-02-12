using System.Text;
using Pizza.Models;
using Pizza.Utils;

namespace Pizza
{
    public class ParsingResult
    {
        internal ParsingResult(Models.Pizza pizza, SliceConstraints sliceConstraints)
        {
            Pizza = pizza;
            Constraints = sliceConstraints;
        }

        #region Properties
        public Models.Pizza Pizza { get; }
        public SliceConstraints Constraints { get; }
        #endregion

        public string ToInputString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{Pizza.Line} {Pizza.Column} {Constraints.MinimumIngredientCount} {Constraints.MaximumSliceSize}");

            for (var row = 0; row < Pizza.Line; row++)
            {
                for (var col = 0; col < Pizza.Column; col++)
                {
                    sb.Append(Pizza.GetIngredientAt(new Coordinate(col, row)).ToSingleCharString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}