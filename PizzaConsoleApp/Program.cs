﻿using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using Pizza;
using Pizza.Models;

namespace PizzaConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var cliArguments = new CliArguments();
            if (!Parser.Default.ParseArguments(args, cliArguments))
            {
                Environment.Exit(1);
            }

            var inputFile = cliArguments.FilePath;
            var outputFile = Path.ChangeExtension(inputFile, ".out");
            var metadataFile = Path.ChangeExtension(inputFile, ".metadata");

            var parsingResult = PizzaParser.ParseFile(inputFile);
            Slice slice = parsingResult.Pizza;

            var stopwatch = Stopwatch.StartNew();
            var bestWayToCut = new SlicerOptimizer(parsingResult.Constraints, cliArguments.TopPercent).FindBestWayToCut(slice);
            stopwatch.Stop();

            File.WriteAllText(outputFile, bestWayToCut.ToOutputString());

            var elapsed = stopwatch.Elapsed;

            File.WriteAllText(metadataFile,
                $@"Slicing took : {elapsed.Hours}H {elapsed.Minutes}M {elapsed.Seconds}S {elapsed.Milliseconds}MS
{bestWayToCut.PointEarned} / {slice.Size} ({(bestWayToCut.PointEarned / (double)slice.Size):P})");
        }
    }
}
