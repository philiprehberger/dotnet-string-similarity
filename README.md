# Philiprehberger.StringSimilarity

[![CI](https://github.com/philiprehberger/dotnet-string-similarity/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-string-similarity/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.StringSimilarity.svg)](https://www.nuget.org/packages/Philiprehberger.StringSimilarity)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-string-similarity)](https://github.com/philiprehberger/dotnet-string-similarity/commits/main)

String similarity and phonetic algorithms including Levenshtein, Damerau-Levenshtein, Jaro-Winkler, Dice, Soundex, Double Metaphone, trigrams, and fuzzy search.

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

### Damerau-Levenshtein

```csharp
using Philiprehberger.StringSimilarity;

int distance = Similarity.DamerauLevenshteinDistance("ab", "ba");      // 1 (transposition)
double score = Similarity.DamerauLevenshteinSimilarity("ab", "ba");    // 0.5
```

### Fuzzy Search

```csharp
using Philiprehberger.StringSimilarity;

var fuzzy = new FuzzySearch();
double score = fuzzy.Score("kitten", "sitting");

var candidates = new[] { "apple", "application", "ape", "banana" };
var results = fuzzy.Find("app", candidates, threshold: 0.3);
var topResults = fuzzy.FindTopN("app", candidates, 2);

// Custom weights
var custom = new FuzzySearch(new Dictionary<SimilarityAlgorithm, double>
{
    [SimilarityAlgorithm.JaroWinkler] = 2.0,
    [SimilarityAlgorithm.Dice] = 1.0
});
```

### Phonetic Matching

```csharp
using Philiprehberger.StringSimilarity;

bool similar = PhoneticMatcher.AreSimilar("Smith", "Smyth");  // true
double score = PhoneticMatcher.Score("Smith", "Smyth");       // 0.5-1.0

var candidates = new[] { "Smith", "Smyth", "Jones" };
var matches = PhoneticMatcher.FindMatches("Smith", candidates);
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
| `DamerauLevenshteinDistance(a, b)` | `int` | Damerau-Levenshtein distance with transpositions |
| `DamerauLevenshteinSimilarity(a, b)` | `double` | Normalized Damerau-Levenshtein similarity (0--1) |
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

### `DamerauLevenshtein`

| Method | Return | Description |
|--------|--------|-------------|
| `Distance(a, b)` | `int` | Damerau-Levenshtein distance with transpositions |
| `Normalize(a, b)` | `double` | Normalized similarity (0--1) |

### `FuzzySearch`

| Method | Return | Description |
|--------|--------|-------------|
| `Score(a, b)` | `double` | Weighted average similarity (0--1) |
| `Find(query, candidates, threshold)` | `List<MatchResult>` | Candidates above threshold sorted by score |
| `FindTopN(query, candidates, n)` | `List<MatchResult>` | Top N candidates sorted by score |

### `PhoneticMatcher`

| Method | Return | Description |
|--------|--------|-------------|
| `AreSimilar(a, b)` | `bool` | Whether strings sound alike (Soundex or Double Metaphone) |
| `FindMatches(query, candidates)` | `List<string>` | Candidates that sound similar to query |
| `Score(a, b)` | `double` | Phonetic similarity (0--1) |

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
| `DamerauLevenshtein` | Damerau-Levenshtein with transpositions |

## Development

```bash
dotnet build src/Philiprehberger.StringSimilarity.csproj --configuration Release
```

## Support

If you find this project useful:

⭐ [Star the repo](https://github.com/philiprehberger/dotnet-string-similarity)

🐛 [Report issues](https://github.com/philiprehberger/dotnet-string-similarity/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

💡 [Suggest features](https://github.com/philiprehberger/dotnet-string-similarity/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

❤️ [Sponsor development](https://github.com/sponsors/philiprehberger)

🌐 [All Open Source Projects](https://philiprehberger.com/open-source-packages)

💻 [GitHub Profile](https://github.com/philiprehberger)

🔗 [LinkedIn Profile](https://www.linkedin.com/in/philiprehberger)

## License

[MIT](LICENSE)
