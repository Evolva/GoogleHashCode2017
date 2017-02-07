﻿using System;
using System.Linq;
using NFluent;
using NUnit.Framework;
using Pizza.Models;
using Pizza.Utils;

namespace PizzaTests
{
    public class SliceFindBestWayToCutTests
    {
        [Test]
        public void Find_Best_Way_To_Cut_A_Single_Ingredient_Pizza()
        {
            var pizzaString = "1 1 1 1,M".Replace(",", Environment.NewLine);
            var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();

            var cutStrat = originalSlice.FindBestWayToCut();

            Check.That(cutStrat.PointEarned).IsEqualTo(0);
            Check.That(cutStrat.ValidSlices).HasSize(0);
        }

        [Test]
        public void Find_Best_Way_To_Cut_A_Tiny_Pizza()
        {
            var pizzaString = "2 2 1 4,TM,MM".Replace(",", Environment.NewLine);
            var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();

            var cutStrat = originalSlice.FindBestWayToCut();

            Check.That(cutStrat.PointEarned).IsEqualTo(4);
            Check.That(cutStrat.ValidSlices).HasSize(1);
        }

        [Test]
        public void Find_Best_Way_To_Cut_A_3x3_Pizza()
        {
            var pizzaString = "3 3 1 4,TMM,MMM,MMT".Replace(",", Environment.NewLine);
            var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();

            var cutStrat = originalSlice.FindBestWayToCut();

            Check.That(cutStrat.PointEarned).IsEqualTo(7);
            Check.That(cutStrat.ValidSlices).HasSize(2);
        }

        [Test]
        public void Find_Best_Way_To_Cut_Sample()
        {
            var pizzaString = "3 5 1 6,TTTTT,TMMMT,TTTTT".Replace(",", Environment.NewLine);
            var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();

            var cutStrat = originalSlice.FindBestWayToCut();

            Check.That(cutStrat.PointEarned).IsEqualTo(15);
            Check.That(cutStrat.ValidSlices).HasSize(3);
        }

        [Test]
        public void Find_Best_Way_To_Cut_Small()
        {
            var pizzaString = "6 7 1 5,TMMMTTT,MMMMTMM,TTMTTMT,TMMTMMM,TTTTTTM,TTTTTTM".Replace(",", Environment.NewLine);

            var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();
            var cutStrat = originalSlice.FindBestWayToCut();
            Check.That(cutStrat.PointEarned).IsEqualTo(42);            
        }

        [Test, Explicit]
        public void Pseudo_Perf_Test()
        {
            var pizzaString = "6 7 1 5,TMMMTTT,MMMMTMM,TTMTTMT,TMMTMMM,TTTTTTM,TTTTTTM".Replace(",", Environment.NewLine);

            Enumerable
                .Range(0, 10000)
                .ForEach(
                    _ =>
                    {
                        var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();
                        var cutStrat = originalSlice.FindBestWayToCut();
                        Check.That(cutStrat.PointEarned).IsEqualTo(42);
                    });

        }
    }
}