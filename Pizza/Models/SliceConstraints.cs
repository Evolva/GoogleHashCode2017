namespace Pizza.Models
{
    public class SliceConstraints
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