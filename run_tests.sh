#!/bin/bash
dotnet run --project tests/Dozenalize.UnitTests/Dozenalize.UnitTests.fsproj &
dotnet run --project tests/Seximalize.UnitTests/Seximalize.UnitTests.fsproj &
wait
