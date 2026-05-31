namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Cosine similarity over character-bigram term-frequency vectors.
/// </summary>
public static class Cosine
{
    /// <summary>
    /// Computes the cosine similarity between two strings using character bigrams.
    /// Returns a value in [0, 1] where 1 means identical bigram distributions.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A similarity score between 0 and 1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Similarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (a == b)
            return 1.0;

        var bigramsA = BigramCounts(a);
        var bigramsB = BigramCounts(b);

        if (bigramsA.Count == 0 || bigramsB.Count == 0)
            return 0.0;

        var dotProduct = 0.0;
        foreach (var (bigram, countA) in bigramsA)
        {
            if (bigramsB.TryGetValue(bigram, out var countB))
                dotProduct += countA * countB;
        }

        var magnitudeA = 0.0;
        foreach (var count in bigramsA.Values)
            magnitudeA += count * count;

        var magnitudeB = 0.0;
        foreach (var count in bigramsB.Values)
            magnitudeB += count * count;

        return dotProduct / (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
    }

    private static Dictionary<string, int> BigramCounts(string s)
    {
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        for (var i = 0; i < s.Length - 1; i++)
        {
            var bigram = s.Substring(i, 2);
            counts[bigram] = counts.TryGetValue(bigram, out var existing) ? existing + 1 : 1;
        }
        return counts;
    }
}
