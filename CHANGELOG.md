# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [2.0.0] - 2024-03-09

### Changed

- Require .NET 6

### Fixed

- FSharp.Core dependency is now correctly set to >= 6.0.0, should have been something lower before for standard

## [1.0.0] - 2022-11-09

Initial stable version based on 0.3.0

## [0.3.0] - 2021-08-07

### Added
- Add C# interop layer (.CSharp namespace in every package)

### Changed
- Preconfigured configs for Dozenalize are functions now

## [0.2.0] - 2021-07-01

### Added
- Niftimalize as implementation for base thirtysix
- Parse.tryDigit functions to all implementations

### Changed
- Rearrange internal code, moved a bit more into Basealize

## [0.1.0] - 2021-06-25

### Added
- Initial implementation of a base conversion library
- Dozenalize as implementation for base twelve
- Seximalize as implementation for base six

[Unreleased]: https://github.com/NicoVIII/Basealize/compare/v2.0.0...HEAD
[2.0.0]: https://github.com/NicoVIII/Basealize/compare/v1.0.0..v2.0.0
[1.0.0]: https://github.com/NicoVIII/Basealize/compare/v0.3.0..v1.0.0
[0.3.0]: https://github.com/NicoVIII/Basealize/compare/v0.2.0..v0.3.0
[0.2.0]: https://github.com/NicoVIII/Basealize/compare/v0.1.0..v0.2.0
[0.1.0]: https://github.com/NicoVIII/Basealize/commits/v0.1.0
