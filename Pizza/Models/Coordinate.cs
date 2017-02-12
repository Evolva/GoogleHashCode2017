namespace Pizza.Models
{
    public struct Coordinate
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