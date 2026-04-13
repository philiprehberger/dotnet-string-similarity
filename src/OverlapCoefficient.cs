namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes the Overlap Coefficient between two strings using character bigrams.
/// The overlap coefficient is |A ∩ B| / min(|A|, |B|), making it less biased
/// than the Dice coefficient when comparing strings of very different lengths.
/// </summary>
public static class OverlapCoefficient
{
    /// <summary>
    /// Computes the overlap coefficient between two strings using bigram overlap.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no shared bigrams) and 1 (complete overlap). Returns 1.0 if both strings are equal, including when both are empty or single characters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Similarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (string.Equals(a, b, StringComparison.Ordinal))
            return 1.0;

        var bigramsA = ExtractBigrams(a);
        var bigramsB = ExtractBigrams(b);

        if (bigramsA.Count == 0 && bigramsB.Count == 0)
            return 1.0;

        if (bigramsA.Count == 0 || bigramsB.Count == 0)
            return 0.0;

        var intersection = 0;

        var bigramCounts = new Dictionary<string, int>(bigramsB.Count);
        foreach (var bigram in bigramsB)
        {
            bigramCounts.TryGetValue(bigram, out var count);
            bigramCounts[bigram] = count + 1;
        }

        foreach (var bigram in bigramsA)
        {
            if (bigramCounts.TryGetValue(bigram, out var count) && count > 0)
            {
                intersection++;
                bigramCounts[bigram] = count - 1;
            }
        }

        var minSize = Math.Min(bigramsA.Count, bigramsB.Count);
        return (double)intersection / minSize;
    }

    /// <summary>
    /// Extracts all character bigrams (consecutive pairs) from a string.
    /// </summary>
    private static List<string> ExtractBigrams(string value)
    {
        if (value.Length < 2)
            return [];

        var bigrams = new List<string>(value.Length - 1);

        for (var i = 0; i < value.Length - 1; i++)
            bigrams.Add(value.Substring(i, 2));

        return bigrams;
    }
}
