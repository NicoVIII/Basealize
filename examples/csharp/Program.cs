using Basealize.CSharp;
using Dozenalize.CSharp;
using Niftimalize.CSharp;
using Seximalize.CSharp;
using System;

namespace Basealize.Examples.CSharp
{
    class Program
    {
        static double? TryParseDouble(string line) {
            double outVar;
            if (Double.TryParse(line, out outVar)) {
                return outVar;
            } else {
                return null;
            }
        }

        static void Print(string baseString, BasePrinter printer, double number) {
            var printed = printer.PrintNumber(number, 3);
            Console.WriteLine("{1} (dezimal) is {0} in {2}.", printed, number, baseString);
        }

        static void Parse(string baseString, BaseParser parser, string line) {
            // Parser
            var parsed = parser.TryParseNumber(line);
            if (parsed != null) {
                Console.WriteLine("{1} ({2}) is {0} in dezimal.", parsed, line, baseString);
            } else {
                Console.WriteLine("Your string is not a {0} number.", baseString);
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Type a number: ");
            var line = Console.ReadLine();
            if (line == null) {
                Console.WriteLine("You have to input something.");
            } else {
                // Prepare arguments
                var numberNull = TryParseDouble(line);

                var config = Config.CreateAb();

                // Print
                switch (numberNull) {
                    case double number:
                        Print("dozenal", DozenalPrinter.Create(config), number);
                        Print("niftimal", new NiftimalPrinter(), number);
                        Print("seximal", SeximalPrinter.Create(), number);
                        break;
                    case null:
                        Console.WriteLine("Your string is not a dezimal number.");
                        break;
                }

                // Parse
                Parse("dozenal", new DozenalParser(config), line);
                Parse("niftimal", NiftimalParser.Create(), line);
                Parse("seximal", SeximalParser.Create(), line);
            }
        }
    }
}
