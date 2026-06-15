namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes Jaccard similarity over character sets or whitespace-delimited token sets.
/// </summary>
public static class Jaccard
{
    /// <summary>
    /// Computes the Jaccard similarity over the distinct character sets of two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0.0 (no shared characters) and 1.0 (identical character sets). Returns 1.0 if both strings are empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Similarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (a.Length == 0 && b.Length == 0)
            return 1.0;

        if (a.Length == 0 || b.Length == 0)
            return 0.0;

        var setA = new HashSet<char>(a);
        var setB = new HashSet<char>(b);

        return ComputeIndex(setA, setB);
    }

    /// <summary>
    /// Computes the Jaccard similarity over the distinct token sets of two strings,
    /// splitting on the given separator character.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="separator">The character used to split each string into tokens. Defaults to space.</param>
    /// <returns>A value between 0.0 (no shared tokens) and 1.0 (identical token sets). Returns 1.0 if both strings are empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double TokenSimilarity(string a, string b, char separator = ' ')
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var tokensA = a.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        var tokensB = b.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        if (tokensA.Length == 0 && tokensB.Length == 0)
            return 1.0;

        if (tokensA.Length == 0 || tokensB.Length == 0)
            return 0.0;

        var setA = new HashSet<string>(tokensA, StringComparer.Ordinal);
        var setB = new HashSet<string>(tokensB, StringComparer.Ordinal);

        return ComputeIndex(setA, setB);
    }

    private static double ComputeIndex<T>(HashSet<T> a, HashSet<T> b)
    {
        var intersection = new HashSet<T>(a);
        intersection.IntersectWith(b);

        var union = new HashSet<T>(a);
        union.UnionWith(b);

        return union.Count == 0 ? 0.0 : (double)intersection.Count / union.Count;
    }
}
