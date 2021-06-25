
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
