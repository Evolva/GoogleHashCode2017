using CommandLine;

namespace PizzaConsoleApp
{
    public class CliArguments
    {
        [Option(shortName: 'f', longName: "file", Required = true, HelpText = "input file")]
        public string FilePath { get; set; }

        [Option(shortName: 'v', longName: "verbose", Required = false, DefaultValue = false, HelpText = "Print each new best way to cut to stdout")]
        public bool PrintNewMax { get; set; }
    }
}