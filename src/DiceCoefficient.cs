namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes the Sorensen-Dice coefficient using character bigrams.
/// </summary>
internal static class DiceCoefficientAlgorithm
{
    /// <summary>
    /// Computes the Sorensen-Dice coefficient between two strings using bigram overlap.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no shared bigrams) and 1 (identical bigram sets). Returns 1 if both strings are equal, including when both are empty or single characters.</returns>
    internal static double Compute(string a, string b)
    {
        if (string.Equals(a, b, StringComparison.Ordinal))
            return 1.0;

        var bigramsA = ExtractBigrams(a);
        var bigramsB = ExtractBigrams(b);

        if (bigramsA.Count == 0 && bigramsB.Count == 0)
            return 1.0;

        if (bigramsA.Count == 0 || bigramsB.Count == 0)
            return 0.0;

        var intersection = 0;

        // Count shared bigrams, handling duplicates correctly by decrementing counts.
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

        return (2.0 * intersection) / (bigramsA.Count + bigramsB.Count);
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
