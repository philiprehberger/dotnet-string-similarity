namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes trigram similarity between strings using the Jaccard index of trigram sets.
/// </summary>
public static class Trigram
{
    /// <summary>
    /// Computes the trigram similarity between two strings using the Jaccard index.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0.0 (no shared trigrams) and 1.0 (identical trigram sets). Returns 1.0 if both strings are equal, including when both are empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Similarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (string.Equals(a, b, StringComparison.Ordinal))
            return 1.0;

        var trigramsA = ExtractTrigrams(a);
        var trigramsB = ExtractTrigrams(b);

        if (trigramsA.Count == 0 && trigramsB.Count == 0)
            return 1.0;

        if (trigramsA.Count == 0 || trigramsB.Count == 0)
            return 0.0;

        var intersection = new HashSet<string>(trigramsA);
        intersection.IntersectWith(trigramsB);

        var union = new HashSet<string>(trigramsA);
        union.UnionWith(trigramsB);

        return (double)intersection.Count / union.Count;
    }

    /// <summary>
    /// Extracts all character trigrams (consecutive triples) from a string.
    /// </summary>
    private static HashSet<string> ExtractTrigrams(string value)
    {
        if (value.Length < 3)
            return [];

        var trigrams = new HashSet<string>(value.Length - 2);

        for (var i = 0; i <= value.Length - 3; i++)
            trigrams.Add(value.Substring(i, 3));

        return trigrams;
    }
}
