# Dozenalize

Base twelve is an interesting one because it needs only 2 characters more and the base twelve is highly
composable.

https://en.wikipedia.org/wiki/Duodecimal

Here is a nice video from Numberphile about the Dozenal system:
https://www.youtube.com/watch?v=U6xJfP7-HCc

## Usage

Because we need two additional digits for dozenal, you have to provide a config for these two characters.

```fsharp
open Dozenalize

let config = Config.create "A" "B"
```

There are also [prepared configs](src/Dozenalize/Types.fs) for commonly used ones:
```fsharp
open Dozenalize

let config = Config.createAb ()
```

### Printer

You can use this config together with your wanted precision and number to create a string in dozenal:

```fsharp
open Dozenalize

Display.number config 0uy 13.16 // "11"
Display.number config 2uy 13.16 // "11.1B"
Display.number config 5uy 13.16 // "11.1B059"
```

### Parser

You can use the config and a string representing a dozenal number to create a float from it:

```fsharp
open Dozenalize

Parse.number config "AB" // 131
Parse.number config "11.1B" // 13.159722
```

## Usage (C#)

Because we need two additional digits for dozenal, you have to provide a config for these two characters.

```csharp
using Dozenalize.CSharp;

var config = Config.Create("A", "B");
```

There are also [prepared configs](src/Dozenalize/Api.fs) for commonly used ones:
```csharp
using Dozenalize.CSharp;

var config = Config.CreateAb();
```

Now you can use this config together with the Printer and Parser. Because of F# limitations
 the relevant methods are explicitly implemented on an interface, therefore you have
 to downcast the classes before use:

```csharp
using Basealize.CSharp;
using Dozenalize.CSharp;

var config = Config.CreateAb();
var printer = (BasePrinter) new DozenalPrinter(config);
var parser = (BaseParser) new DozenalParser(config);
```

Or you can use the static Create methods:

```csharp
using Dozenalize.CSharp;

var config = Config.CreateAb();
var printer = DozenalPrinter.Create(config);
var parser = DozenalParser.Create(config);
```

### Printer

You can handle the precision for the printer in two ways.
You can set a default precision on the printer itself:

```csharp
using Dozenalize.CSharp;

var printer = DozenalPrinter.Create(Config.CreateAb(), 1);
Console.WriteLine(printer.PrintNumber(1.3)); // "1.4"
Console.WriteLine(printer.PrintNumber(1.7)); // "1.8"
Console.WriteLine(printer.PrintNumber(3.3)); // "3.4"
```

But you can also define the precision per function call:

```csharp
using Dozenalize.CSharp;

var printer = DozenalPrinter.Create(config);
Console.WriteLine(printer.PrintNumber(1.3, 1)); // "1.4"
Console.WriteLine(printer.PrintNumber(1.7, 2)); // "1.85"
Console.WriteLine(printer.PrintNumber(3.3, 3)); // "3.372"
```

If you mix both methods, the argument on the function call beat the default precision on the printer:

```csharp
using Dozenalize.CSharp;

var printer = DozenalPrinter.Create(Config.CreateAb(), 1);
Console.WriteLine(printer.PrintNumber(1.3)); // "1.4"
Console.WriteLine(printer.PrintNumber(1.7, 2)); // "1.85"
Console.WriteLine(printer.PrintNumber(3.3, 3)); // "3.372"
```

### Parser

To parse a number, create a parser and pass the string to parse.
There are two ways errors are handled:

* Try-prefixed functions return null, if the string is not parsable
* Non-prefixed functions throws exceptions, if the string is not parsable

```csharp
using Dozenalize.CSharp;

var parser = DozenalParser.Create(Config.CreateAb());
var number1 = parser.TryParseNumber("2A"); // 34
var number2 = parser.TryParseNumber("ZZ"); // null
var number3 = parser.ParseNumber("ZZ"); // !! throws Exception
```
