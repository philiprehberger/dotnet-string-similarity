# Philiprehberger.StringSimilarity

[![CI](https://github.com/philiprehberger/dotnet-string-similarity/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-string-similarity/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.StringSimilarity.svg)](https://www.nuget.org/packages/Philiprehberger.StringSimilarity)
[![GitHub release](https://img.shields.io/github/v/release/philiprehberger/dotnet-string-similarity)](https://github.com/philiprehberger/dotnet-string-similarity/releases)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-string-similarity)](https://github.com/philiprehberger/dotnet-string-similarity/commits/main)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-string-similarity)](LICENSE)
[![Bug Reports](https://img.shields.io/github/issues/philiprehberger/dotnet-string-similarity/bug)](https://github.com/philiprehberger/dotnet-string-similarity/issues?q=is%3Aissue+is%3Aopen+label%3Abug)
[![Feature Requests](https://img.shields.io/github/issues/philiprehberger/dotnet-string-similarity/enhancement)](https://github.com/philiprehberger/dotnet-string-similarity/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)
[![Sponsor](https://img.shields.io/badge/sponsor-GitHub%20Sponsors-ec6cb9)](https://github.com/sponsors/philiprehberger)

String similarity and phonetic algorithms including Levenshtein, Jaro-Winkler, Dice, Soundex, Double Metaphone, and trigrams.

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

### Normalized Levenshtein

```csharp
using Philiprehberger.StringSimilarity;

double score = Similarity.NormalizedLevenshtein("kitten", "sitting");  // 0.571
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

### Find Top N Matches

```csharp
using Philiprehberger.StringSimilarity;

var candidates = new[] { "apple", "application", "ape", "apex", "banana" };
var topMatches = Similarity.FindTopN("app", candidates, 3, SimilarityAlgorithm.Levenshtein);

foreach (var match in topMatches)
{
    Console.WriteLine($"{match.Value}: {match.Score:F3}");
}
```

### Soundex

```csharp
using Philiprehberger.StringSimilarity;

string code = Soundex.Encode("Robert");           // "R163"
bool similar = Soundex.AreSimilar("Robert", "Rupert");  // true
```

### Double Metaphone

```csharp
using Philiprehberger.StringSimilarity;

var (primary, alternate) = DoubleMetaphone.Encode("Schmidt");
Console.WriteLine($"Primary: {primary}, Alternate: {alternate}");
```

### Trigram Similarity

```csharp
using Philiprehberger.StringSimilarity;

double score = Trigram.Similarity("night", "nacht");  // Jaccard index of trigram sets
```

### Raw Edit Distance

```csharp
using Philiprehberger.StringSimilarity;

int distance = Similarity.Distance("kitten", "sitting");  // 3
```

## API

### `Similarity`

| Method | Return | Description |
|--------|--------|-------------|
| `Levenshtein(a, b)` | `double` | Normalized Levenshtein similarity (0--1) |
| `JaroWinkler(a, b)` | `double` | Jaro-Winkler similarity (0--1) |
| `Dice(a, b)` | `double` | Sorensen-Dice coefficient (0--1) |
| `Distance(a, b)` | `int` | Raw Levenshtein edit distance |
| `NormalizedLevenshtein(a, b)` | `double` | Normalized Levenshtein similarity (0--1) |
| `BestMatch(input, candidates, threshold)` | `MatchResult?` | Best match across all algorithms |
| `FindTopN(input, candidates, n, algorithm)` | `List<MatchResult>` | Top N matches ranked by score |

### `Soundex`

| Method | Return | Description |
|--------|--------|-------------|
| `Encode(value)` | `string` | 4-character Soundex code |
| `AreSimilar(a, b)` | `bool` | Whether two strings share the same Soundex code |

### `DoubleMetaphone`

| Method | Return | Description |
|--------|--------|-------------|
| `Encode(value)` | `(string Primary, string Alternate)` | Primary and alternate metaphone codes |

### `Trigram`

| Method | Return | Description |
|--------|--------|-------------|
| `Similarity(a, b)` | `double` | Jaccard index of trigram sets (0--1) |

### `SimilarityAlgorithm`

| Value | Description |
|-------|-------------|
| `Levenshtein` | Normalized Levenshtein similarity |
| `JaroWinkler` | Jaro-Winkler similarity |
| `Dice` | Sorensen-Dice coefficient |
| `Trigram` | Trigram similarity (Jaccard index) |
| `NormalizedLevenshtein` | Normalized Levenshtein distance |

## Development

```bash
dotnet build src/Philiprehberger.StringSimilarity.csproj --configuration Release
```

## Support

If you find this package useful, consider giving it a star on GitHub — it helps motivate continued maintenance and development.

[![LinkedIn](https://img.shields.io/badge/Philip%20Rehberger-LinkedIn-0A66C2?logo=linkedin)](https://www.linkedin.com/in/philiprehberger)
[![More packages](https://img.shields.io/badge/more-open%20source%20packages-blue)](https://philiprehberger.com/open-source-packages)

## License

[MIT](LICENSE)
