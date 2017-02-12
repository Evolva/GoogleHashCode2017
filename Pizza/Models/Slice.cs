using System;
using System.Text;
using Pizza.Utils;

namespace Pizza.Models
{
    public class Slice
    {
        private readonly Pizza _parsingResult;

        public Slice(Pizza context, Coordinate topLeft, Coordinate bottomRight)
        {
            _parsingResult = context;

            TopLeft = topLeft;
            BottomRight = bottomRight;

            for (var col = TopLeft.Column; col <= BottomRight.Column; col++)
            {
                for (var row = TopLeft.Row; row <= BottomRight.Row; row++)
                {
                    var ingredient = _parsingResult.GetIngredientAt(new Coordinate(col, row));

                    switch (ingredient)
                    {
                        case Ingredient.Tomato:
                            TomatoCount++;
                            continue;
                        case Ingredient.Mushroom:
                            MushroomCount++;
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(ingredient), ingredient, null);
                    }
                }
            }
        }

        public Tuple<Slice, Slice> Cut(Direction direction, int firstSliceSize)
        {
            Slice slice1, slice2;

            if (direction == Direction.Horizontal)
            {
                if (firstSliceSize <= 0 || firstSliceSize >= Height)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(firstSliceSize),
                        firstSliceSize,
                        $"0 < {nameof(firstSliceSize)} < {Height} (Height)");
                }

                slice1 = new Slice(_parsingResult,
                    new Coordinate(TopLeft.Column, TopLeft.Row),
                    new Coordinate(BottomRight.Column, TopLeft.Row + firstSliceSize - 1));
                slice2 = new Slice(_parsingResult,
                    new Coordinate(TopLeft.Column, TopLeft.Row + firstSliceSize),
                    new Coordinate(BottomRight.Column, BottomRight.Row));
            }
            else
            {
                if (firstSliceSize <= 0 || firstSliceSize >= Width)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(firstSliceSize),
                        firstSliceSize,
                        $"0 < {nameof(firstSliceSize)} < {Width} (Width)");
                }


                slice1 = new Slice(_parsingResult,
                    new Coordinate(TopLeft.Column, TopLeft.Row),
                    new Coordinate(TopLeft.Column + firstSliceSize - 1, BottomRight.Row));
                slice2 = new Slice(_parsingResult,
                    new Coordinate(TopLeft.Column + firstSliceSize, TopLeft.Row),
                    new Coordinate(BottomRight.Column, BottomRight.Row));
            }

            return Tuple.Create(slice1, slice2);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var row = TopLeft.Row; row <= BottomRight.Row; row++)
            {
                for (var col = TopLeft.Column; col <= BottomRight.Column; col++)
                {
                    sb.Append(_parsingResult.GetIngredientAt(new Coordinate(col, row)).ToSingleCharString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #region Properties
        public Coordinate TopLeft { get; }

        public Coordinate BottomRight { get; }

        public int Width => BottomRight.Column - TopLeft.Column + 1;
        public int Height => BottomRight.Row - TopLeft.Row + 1;
        public int Size => Width * Height;

        public int TomatoCount { get; }
        public int MushroomCount { get; }
        #endregion
    }
}