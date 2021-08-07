
## Niftimalize

Base thirtysix is an interesting one to compress seximal like jan Misali points out here:
https://www.seximal.net/hexaseximal

### Usage

Simply call it like this with your wanted precision and number:

```fsharp
open Niftimalize

Display.number config 0uy 13.16 // D
Display.number config 2uy 13.16 // D.5R
Display.number config 5uy 13.16 // D.5RCYK
```

## Usage (C#)

There is a Printer and a Parser. Because of F# limitations  the relevant methods are explicitly implemented on an interface, therefore you have to downcast the classes before use:

```csharp
using Basealize.CSharp;
using Niftimalize.CSharp;

var printer = (BasePrinter) new NiftimalPrinter();
var parser = (BaseParser) new NiftimalParser();
```

Or you can use the static Create methods:

```csharp
using Niftimalize.CSharp;

var printer = NiftimalPrinter.Create();
var parser = NiftimalParser.Create();
```

### Printer

You can handle the precision for the printer in two ways.
You can set a default precision on the printer itself:

```csharp
using Niftimalize.CSharp;

var printer = NiftimalPrinter.Create(1);
Console.WriteLine(printer.PrintNumber(1.3)); // 1.B
Console.WriteLine(printer.PrintNumber(1.7)); // 1.P
Console.WriteLine(printer.PrintNumber(3.3)); // 3.B
```

But you can also define the precision per function call:

```csharp
using Niftimalize.CSharp;

var printer = NiftimalPrinter.Create();
Console.WriteLine(printer.PrintNumber(1.3, 1)); // 1.B
Console.WriteLine(printer.PrintNumber(1.7, 2)); // 1.P7
Console.WriteLine(printer.PrintNumber(3.3, 3)); // 3.AST
```

If you mix both methods, the argument on the function call beat the default precision on the printer:

```csharp
using Niftimalize.CSharp;

var printer = NiftimalPrinter.Create(1);
Console.WriteLine(printer.PrintNumber(1.3)); // 1.B
Console.WriteLine(printer.PrintNumber(1.7, 2)); // 1.P7
Console.WriteLine(printer.PrintNumber(3.3, 3)); // 3.AST
```

### Parser

To parse a number, create a parser and pass the string to parse.
There are two ways errors are handled:

* Try-prefixed functions return null, if the string is not parsable
* Non-prefixed functions throws exceptions, if the string is not parsable

```csharp
using Niftimalize.CSharp;

var parser = NiftimalParser.Create();
var number1 = parser.TryParseNumber("2A"); // 82
var number2 = parser.TryParseNumber("ÄÄ"); // null
var number3 = parser.ParseNumber("ÄÄ"); // !! throws Exception
```
