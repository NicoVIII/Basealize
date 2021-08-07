
## Seximalize

Base six is also interesting like jan Misali points out here:
https://www.seximal.net/

### Usage

Simply call it like this with your wanted precision and number:

```fsharp
open Seximalize

Display.number config 0uy 13.16 // 21
Display.number config 2uy 13.16 // 21.10
Display.number config 5uy 13.16 // 21.05432
```

## Usage (C#)

There is a Printer and a Parser. Because of F# limitations  the relevant methods are explicitly implemented on an interface, therefore you have to downcast the classes before use:

```csharp
using Basealize.CSharp;
using Seximalize.CSharp;

var printer = (BasePrinter) new SeximalPrinter();
var parser = (BaseParser) new SeximalParser();
```

Or you can use the static Create methods:

```csharp
using Seximalize.CSharp;

var printer = SeximalPrinter.Create();
var parser = SeximalParser.Create();
```

### Printer

You can handle the precision for the printer in two ways.
You can set a default precision on the printer itself:

```csharp
using Seximalize.CSharp;

var printer = SeximalPrinter.Create(1);
Console.WriteLine(printer.PrintNumber(1.3)); // 1.2
Console.WriteLine(printer.PrintNumber(1.7)); // 1.4
Console.WriteLine(printer.PrintNumber(3.3)); // 3.2
```

But you can also define the precision per function call:

```csharp
using Seximalize.CSharp;

var printer = SeximalPrinter.Create();
Console.WriteLine(printer.PrintNumber(1.3, 1)); // 1.2
Console.WriteLine(printer.PrintNumber(1.7, 2)); // 1.41
Console.WriteLine(printer.PrintNumber(3.3, 3)); // 3.145
```

If you mix both methods, the argument on the function call beat the default precision on the printer:

```csharp
using Seximalize.CSharp;

var printer = SeximalPrinter.Create(1);
Console.WriteLine(printer.PrintNumber(1.3)); // 1.2
Console.WriteLine(printer.PrintNumber(1.7, 2)); // 1.41
Console.WriteLine(printer.PrintNumber(3.3, 3)); // 3.145
```

### Parser

To parse a number, create a parser and pass the string to parse.
There are two ways errors are handled:

* Try-prefixed functions return null, if the string is not parsable
* Non-prefixed functions throws exceptions, if the string is not parsable

```csharp
using Seximalize.CSharp;

var parser = SeximalParser.Create();
var number1 = parser.TryParseNumber("24"); // 16
var number2 = parser.TryParseNumber("66"); // null
var number3 = parser.ParseNumber("66"); // !! throws Exception
```
