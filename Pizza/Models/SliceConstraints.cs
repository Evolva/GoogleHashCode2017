namespace Pizza.Models
{
    public struct SliceConstraints
    {
        public int MaximumSliceSize { get; }
        public int MinimumIngredientCount { get; }

        public SliceConstraints(int maximumSliceSize, int minimumIngredientCount)
        {
            MaximumSliceSize = maximumSliceSize;
            MinimumIngredientCount = minimumIngredientCount;
        }
    }
}