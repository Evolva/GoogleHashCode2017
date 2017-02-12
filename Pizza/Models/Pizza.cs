namespace Pizza.Models
{
    public class Pizza
    {
        public int Line { get; }
        public int Column { get; }

        private readonly Ingredient[] _ingredients;

        public Pizza(int line, int column, Ingredient[] ingredients)
        {
            Line = line;
            Column = column;
            _ingredients = ingredients;
        }

        public Ingredient GetIngredientAt(Coordinate coordinate)
        {
            return _ingredients[coordinate.Column + coordinate.Row * Column];
        }

        public static implicit operator Slice(Pizza pizza)
        {
            return new Slice(pizza, new Coordinate(0, 0), new Coordinate(pizza.Column - 1, pizza.Line - 1));
        }
    }
}