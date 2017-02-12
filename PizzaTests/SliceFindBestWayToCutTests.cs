using System.Linq;
using NFluent;
using NUnit.Framework;
using Pizza;
using Pizza.Utils;

namespace PizzaTests
{
    public class SliceFindBestWayToCutTests
    {
        [Test]
        public void Find_Best_Way_To_Cut_A_Single_Ingredient_Pizza()
        {
            var parsingResult = PizzaParser.ParseOneLineString("1 1 1 1,M");
            var cutStrat = new SlicerOptimizer(parsingResult.Constraints).FindBestWayToCut(parsingResult.Pizza);

            Check.That(cutStrat.PointEarned).IsEqualTo(0);
            Check.That(cutStrat.ValidSlices).HasSize(0);
        }

        [Test]
        public void Find_Best_Way_To_Cut_A_Tiny_Pizza()
        {
            var parsingResult = PizzaParser.ParseOneLineString("2 2 1 4,TM,MM");
            var cutStrat = new SlicerOptimizer(parsingResult.Constraints).FindBestWayToCut(parsingResult.Pizza);

            Check.That(cutStrat.PointEarned).IsEqualTo(4);
            Check.That(cutStrat.ValidSlices).HasSize(1);
        }

        [Test]
        public void Find_Best_Way_To_Cut_A_3x3_Pizza()
        {
            var parsingResult = PizzaParser.ParseOneLineString("3 3 1 4,TMM,MMM,MMT");
            var cutStrat = new SlicerOptimizer(parsingResult.Constraints).FindBestWayToCut(parsingResult.Pizza);

            Check.That(cutStrat.PointEarned).IsEqualTo(7);
            Check.That(cutStrat.ValidSlices).HasSize(2);
        }

        [Test]
        public void Find_Best_Way_To_Cut_Sample()
        {
            var parsingResult = PizzaParser.ParseOneLineString("3 5 1 6,TTTTT,TMMMT,TTTTT");
            var cutStrat = new SlicerOptimizer(parsingResult.Constraints).FindBestWayToCut(parsingResult.Pizza);

            Check.That(cutStrat.PointEarned).IsEqualTo(15);
            Check.That(cutStrat.ValidSlices).HasSize(3);
        }

        [Test]
        public void Find_Best_Way_To_Cut_Small()
        {
            var parsingResult = PizzaParser.ParseOneLineString("6 7 1 5,TMMMTTT,MMMMTMM,TTMTTMT,TMMTMMM,TTTTTTM,TTTTTTM");
            var cutStrat = new SlicerOptimizer(parsingResult.Constraints).FindBestWayToCut(parsingResult.Pizza);

            Check.That(cutStrat.PointEarned).IsEqualTo(42);            
        }

        [Test, Explicit]
        public void Poor_Man_Perf_Test()
        {
            Enumerable
                .Range(0, 10000)
                .ForEach(
                    _ =>
                    {
                        var parsingResult = PizzaParser.ParseOneLineString("6 7 1 5,TMMMTTT,MMMMTMM,TTMTTMT,TMMTMMM,TTTTTTM,TTTTTTM");
                        var cutStrat = new SlicerOptimizer(parsingResult.Constraints)
                            .FindBestWayToCut(parsingResult.Pizza);
                        Check.That(cutStrat.PointEarned).IsEqualTo(42);
                    });

        }
    }
}