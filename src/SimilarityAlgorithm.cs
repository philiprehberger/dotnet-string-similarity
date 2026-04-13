namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Specifies a string similarity algorithm for use in ranked search operations.
/// </summary>
public enum SimilarityAlgorithm
{
    /// <summary>Normalized Levenshtein similarity (0-1).</summary>
    Levenshtein,

    /// <summary>Jaro-Winkler similarity (0-1).</summary>
    JaroWinkler,

    /// <summary>Sorensen-Dice coefficient using bigrams (0-1).</summary>
    Dice,

    /// <summary>Trigram similarity using Jaccard index (0-1).</summary>
    Trigram,

    /// <summary>Normalized Levenshtein distance as a similarity score (0-1).</summary>
    NormalizedLevenshtein,

    /// <summary>Damerau-Levenshtein distance with transpositions (0-1).</summary>
    DamerauLevenshtein,

    /// <summary>Longest Common Subsequence similarity (0-1).</summary>
    LongestCommonSubsequence,

    /// <summary>Overlap Coefficient using bigrams (0-1).</summary>
    OverlapCoefficient
}
