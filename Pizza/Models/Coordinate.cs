namespace Pizza.Models
{
    public class Coordinate
    {
        public int Column { get; }
        public int Row { get; }

        public Coordinate(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}