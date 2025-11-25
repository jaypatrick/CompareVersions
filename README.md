# CompareVersions

[![Build and Test](https://github.com/jaypatrick/CompareVersions/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/jaypatrick/CompareVersions/actions/workflows/build-and-test.yml)
[![CodeQL](https://github.com/jaypatrick/CompareVersions/actions/workflows/codeql.yml/badge.svg)](https://github.com/jaypatrick/CompareVersions/actions/workflows/codeql.yml)

A comprehensive .NET library for semantic version comparison and manipulation, implementing the `major.minor.patch.build` format with full operator overloading support.

## Features

✨ **Comprehensive Version Support**
- Parse and create versions in `major.minor.patch.build` format
- Support for 2, 3, or 4 segment versions
- Type-safe version components with range validation (0-99)

🔢 **Rich Operator Support**
- **Comparison**: `==`, `!=`, `<`, `<=`, `>`, `>=`
- **Arithmetic**: `+` (addition), `-` (subtraction)
- **Increment/Decrement**: `++`, `--` on individual segments

🎯 **Modern .NET Interfaces**
- `IComparable<Version>`  & `IEquatable<Version>`
- `ISpanParsable<Version>` & `IParsable<Version>`
- `IAdditionOperators` & `ISubtractionOperators`
- `IEnumerable<Segment>` for iteration

⚡ **Performance Optimized**
- Span-based parsing and formatting
- Minimal allocations
- Efficient hash code generation

## Installation

### NuGet Package

```bash
dotnet add package CompareVersions
```

### From Source

```bash
git clone https://github.com/jaypatrick/CompareVersions.git
cd CompareVersions
dotnet build
```

## Quick Start

### Creating Versions

```csharp
using CompareVersions.Models;

// Default constructor (0.0.0.0)
var v1 = new Version();

// From integers
var v2 = new Version(1, 2, 3, 4);

// From string
var v3 = new Version("1.2.3.4", '.');

// Parse methods
var v4 = Version.Parse("2.5.1.0");
if (Version.TryParse("1.0.0", out var v5))
{
    Console.WriteLine($"Parsed: {v5}");
}

// Random version for testing
var random = Version.CreateRandom();
```

### Comparing Versions

```csharp
var version1 = new Version(1, 2, 3, 4);
var version2 = new Version(1, 2, 3, 5);

// Comparison operators
if (version1 < version2)
    Console.WriteLine($"{version1} is less than {version2}");

// Equality
bool areEqual = version1 == version2;  // false

// IComparable
int comparison = version1.CompareTo(version2);  // -1
```

### Arithmetic Operations

```csharp
var v1 = new Version(1, 2, 3, 4);
var v2 = new Version(1, 1, 1, 1);

// Addition
var sum = v1 + v2;  // 2.3.4.5

// Subtraction
var v3 = new Version(5, 5, 5, 5);
var diff = v3 - v1;  // 4.3.2.1
```

### Working with Segments

```csharp
var version = new Version(1, 2, 3, 4);

// Access by position
var major = version[0];  // Segment with value 1

// Access by type
var minor = version[SegmentType.Minor];  // Segment with value 2

// Properties
Console.WriteLine($"Major: {version.MajorSegment.Value}");
Console.WriteLine($"Minor: {version.MinorSegment.Value}");
Console.WriteLine($"Patch: {version.PatchSegment.Value}");
Console.WriteLine($"Build: {version.BuildSegment.Value}");

// Modify segments
version.ReplaceSegment(SegmentType.Major, new Segment(SegmentType.Major, 10));
version.RemoveSegment(SegmentType.Build);
```

### Segment Operations

```csharp
var seg1 = new Segment(SegmentType.Major, 5);
var seg2 = new Segment(SegmentType.Major, 3);

// Arithmetic
var sum = seg1 + seg2;      // Segment with value 8
var diff = seg1 - seg2;     // Segment with value 2

// Increment/Decrement
var incremented = ++seg1;   // Segment with value 6
var decremented = --seg2;   // Segment with value 2

// Comparison
bool isGreater = seg1 > seg2;  // true
```

### Enumeration

```csharp
var version = new Version(1, 2, 3, 4);

// Iterate through segments
foreach (var segment in version)
{
    Console.WriteLine($"{segment.SegmentType}: {segment.Value}");
}

// LINQ operations
var values = version.Select(s => s.Value).ToList();
```

### Cloning

```csharp
var original = new Version(1, 2, 3, 4);
var clone = original.Clone();

// Deep copy - modifying clone doesn't affect original
clone.ReplaceSegment(SegmentType.Major, new Segment(SegmentType.Major, 10));
Console.WriteLine(original);  // Still 1.2.3.4
Console.WriteLine(clone);     // Now 10.2.3.4
```

## Advanced Usage

### Custom Formatting

```csharp
var version = new Version(1, 2, 3, 4);

// Full version
Console.WriteLine(version.ToString());     // "1.2.3.4"

// Limited segments
Console.WriteLine(version.ToString(2));    // "1.2"
Console.WriteLine(version.ToString(3));    // "1.2.3"
```

### Span-Based Parsing

```csharp
ReadOnlySpan<char> versionText = "1.2.3.4";
var version = Version.Parse(versionText);

// Try parse with span
ReadOnlySpan<char> input = "2.0.1";
if (Version.TryParse(input, null, out var parsed))
{
    Console.WriteLine($"Parsed: {parsed}");
}
```

### Hash Code and Collections

```csharp
var versions = new HashSet<Version>
{
    new Version(1, 0, 0, 0),
    new Version(1, 1, 0, 0),
    new Version(2, 0, 0, 0)
};

var lookup = new Dictionary<Version, string>
{
    [new Version(1, 0, 0, 0)] = "Initial release",
    [new Version(2, 0, 0, 0)] = "Major update"
};
```

## API Reference

### Version Class

#### Constructors
- `Version()` - Creates version 0.0.0.0
- `Version(int major, int minor, int patch, int build)` - Creates version from integers
- `Version(string versionString, char separator)` - Parses version string
- `Version(List<Segment> segments)` - Creates from segment collection

#### Static Methods
- `Version Parse(string s)` - Parse string, throws on failure
- `bool TryParse(string s, out Version result)` - Safe parsing
- `Version CreateRandom()` - Generate random version for testing

#### Properties
- `Segment MajorSegment` - Major version segment
- `Segment MinorSegment` - Minor version segment
- `Segment PatchSegment` - Patch version segment
- `Segment BuildSegment` - Build version segment

#### Indexers
- `Segment this[int position]` - Access by position (0-3)
- `Segment this[SegmentType type]` - Access by segment type

#### Methods
- `string ToString()` - Format as string
- `string ToString(int fieldCount)` - Format with specific segment count
- `int CompareTo(Version other)` - Compare versions
- `bool Equals(Version other)` - Check equality
- `Version Clone()` - Create deep copy
- `void AddSegment(Segment segment)` - Add segment
- `void ReplaceSegment(SegmentType type, Segment segment)` - Replace segment
- `void RemoveSegment(SegmentType type)` - Remove segment

#### Operators
- Comparison: `==`, `!=`, `<`, `<=`, `>`, `>=`
- Arithmetic: `+`, `-`

### Segment Class

#### Constructors
- `Segment(SegmentType type, int value)` - Creates segment (value 0-99)

#### Properties
- `int Value` - Segment value (0-99)
- `SegmentType SegmentType` - Type (Major, Minor, Patch, Build)
- `static Segment Default` - Default segment (value 0)
- `static Segment AdditiveIdentity` - Additive identity segment

#### Methods
- `(SegmentType type, int value) GetSegment()` - Get as tuple
- `int CompareTo(Segment other)` - Compare segments
- `bool Equals(Segment other)` - Check equality
- `string ToString()` - Convert to string

#### Operators
- Comparison: `==`, `!=`, `<`, `<=`, `>`, `>=`
- Arithmetic: `+`, `-`
- Increment/Decrement: `++`, `--`

### SegmentType Enum

```csharp
public enum SegmentType
{
    Major = 0,
    Minor = 1,
    Patch = 2,
    Build = 3
}
```

## Requirements

- .NET 10.0 or later
- C# 12 or later

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Development Setup

```bash
# Clone the repository
git clone https://github.com/jaypatrick/CompareVersions.git
cd CompareVersions

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test
dotnet test --filter "FullyQualifiedName~VersionTests"
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

This project was created as an academic exercise to demonstrate:
- Advanced C# operator overloading
- Modern .NET interface implementations
- Span-based APIs for performance
- Comprehensive unit testing practices
- Clean architecture and SOLID principles

## Support

- **Issues**: [GitHub Issues](https://github.com/jaypatrick/CompareVersions/issues)
- **Documentation**: This README and XML documentation comments
- **Examples**: See `/examples` directory for more usage examples

## Changelog

### Version 1.0.0
- Initial release
- Support for .NET 10
- Full operator overloading (comparison, arithmetic, increment/decrement)
- Comprehensive parsing and formatting
- Subtraction operators for Version and Segment
- Complete test coverage
- CI/CD with GitHub Actions
- NuGet package support
