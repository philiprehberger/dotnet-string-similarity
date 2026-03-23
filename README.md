# Philiprehberger.StringSimilarity

[![CI](https://github.com/philiprehberger/dotnet-string-similarity/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-string-similarity/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.StringSimilarity.svg)](https://www.nuget.org/packages/Philiprehberger.StringSimilarity)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-string-similarity)](LICENSE)

String similarity algorithms — Levenshtein, Jaro-Winkler, Dice coefficient, and best-match search.

## Installation

```bash
dotnet add package Philiprehberger.StringSimilarity
```

## Usage

### Compare Strings

```csharp
using Philiprehberger.StringSimilarity;

double levenshtein = Similarity.Levenshtein("kitten", "sitting");  // 0.571
double jaroWinkler = Similarity.JaroWinkler("kitten", "sitting");  // 0.746
double dice = Similarity.Dice("night", "nacht");                   // 0.25
```

### Best Match Search

```csharp
using Philiprehberger.StringSimilarity;

var candidates = new[] { "apple", "application", "ape", "apex" };
var result = Similarity.BestMatch("app", candidates, threshold: 0.3);

if (result is not null)
{
    Console.WriteLine($"{result.Value} ({result.Score:F3}, {result.Algorithm})");
}
```

### Raw Edit Distance

```csharp
using Philiprehberger.StringSimilarity;

int distance = Similarity.Distance("kitten", "sitting");  // 3
```

## API

| Method | Return | Description |
|--------|--------|-------------|
| `Similarity.Levenshtein(a, b)` | `double` | Normalized Levenshtein similarity (0–1) |
| `Similarity.JaroWinkler(a, b)` | `double` | Jaro-Winkler similarity (0–1) |
| `Similarity.Dice(a, b)` | `double` | Sorensen-Dice coefficient (0–1) |
| `Similarity.Distance(a, b)` | `int` | Raw Levenshtein edit distance |
| `Similarity.BestMatch(input, candidates, threshold)` | `MatchResult?` | Best match across all algorithms |

## Development

```bash
dotnet build src/Philiprehberger.StringSimilarity.csproj --configuration Release
```

## License

MIT
