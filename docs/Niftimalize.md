
## Seximalize

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
