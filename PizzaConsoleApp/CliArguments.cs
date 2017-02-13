using CommandLine;

namespace PizzaConsoleApp
{
    public class CliArguments
    {
        [Option(shortName: 'f', longName: "file", Required = true, HelpText = "input file")]
        public string FilePath { get; set; }

        [Option(shortName: 't', longName: "topPercent", Required = false, DefaultValue = 0.2, HelpText = "Top percent")]
        public double TopPercent { get; set; }
    }
}