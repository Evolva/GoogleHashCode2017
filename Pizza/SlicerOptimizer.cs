using System;
using System.Linq;
using Pizza.Models;

namespace Pizza
{
    public class SlicerOptimizer
    {
        private readonly SliceConstraints _constraints;
        private readonly Action<SlicingChallengeResponse> _actionOnNewMax;

        public SlicerOptimizer(SliceConstraints constraints, Action<SlicingChallengeResponse> actionOnNewMax = null)
        {
            _constraints = constraints;
            _actionOnNewMax = actionOnNewMax ?? (_ => { }) ;
        }

        public SlicingChallengeResponse FindBestWayToCut(Slice slice, bool shouldCallAction = true)
        {
            if (IsValid(slice)) return new SlicingChallengeResponse { ValidSlices = { slice } };
            if (!HaveRequestedIngredient(slice)) return SlicingChallengeResponse.Empty;

            var bestWayToCut = SlicingChallengeResponse.Empty;

            var horizontalWayToCut = Enumerable.Range(1, slice.Height - 1)
                .Select(i => new WayToCut { Direction = Direction.Horizontal, SliceSize = i });
            var verticalWayToCut = Enumerable.Range(1, slice.Width - 1)
                .Select(i => new WayToCut { Direction = Direction.Vertical, SliceSize = i });

            var allPossibleWayToCut = horizontalWayToCut.Union(verticalWayToCut).ToList();

            var bestSlicesCandidates = allPossibleWayToCut.Select(cut =>
            {
                var sliced = slice.Cut(cut.Direction, cut.SliceSize);
                return new
                {
                    Slice1 = sliced.Item1,
                    Slice2 = sliced.Item2,
                    MaxPossiblePoints = MaxPossiblePoints(sliced.Item1) + MaxPossiblePoints(sliced.Item2)
                };
            }).OrderByDescending(x => x.MaxPossiblePoints).Take((int)Math.Ceiling(allPossibleWayToCut.Count * 0.2)).ToList();

            foreach (var slices in bestSlicesCandidates)
            {
                if (bestWayToCut.PointEarned >= MaxPossiblePoints(slices.Slice1) + MaxPossiblePoints(slices.Slice2))
                {
                    //we can't no longer improve our score, let's 'cut' to the chase
                    break;
                }

                var bestWayToCutSlice1 = FindBestWayToCut(slices.Slice1, shouldCallAction: false);
                var bestWayToCutSlice2 = FindBestWayToCut(slices.Slice2, shouldCallAction: false);

                if (bestWayToCutSlice1.PointEarned + bestWayToCutSlice2.PointEarned > bestWayToCut.PointEarned)
                {
                    bestWayToCut = new SlicingChallengeResponse
                    {
                        ValidSlices = bestWayToCutSlice1.ValidSlices.Union(bestWayToCutSlice2.ValidSlices).ToList()
                    };

                    if (shouldCallAction) _actionOnNewMax(bestWayToCut);
                }
            }

            return bestWayToCut;
        }

        public bool IsValid(Slice slice)
        {
            return IsSmallEnough(slice) && HaveRequestedIngredient(slice);
        }

        private bool IsSmallEnough(Slice slice)
        {
            return slice.Size <= _constraints.MaximumSliceSize;
        }

        private bool HaveRequestedIngredient(Slice slice)
        {
            return slice.MushroomCount >= _constraints.MinimumIngredientCount
                   && slice.TomatoCount >= _constraints.MinimumIngredientCount;
        } 
        
        public int MaxPossiblePoints(Slice slice)
        {
            var maxNumberOfSlice = Math.Min(slice.MushroomCount, slice.TomatoCount) / _constraints.MinimumIngredientCount;
            return Math.Min(slice.Size, maxNumberOfSlice * _constraints.MaximumSliceSize); 
        }
    }
}