namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Combines Soundex and Double Metaphone phonetic algorithms for matching words that sound alike.
/// </summary>
public static class PhoneticMatcher
{
    /// <summary>
    /// Determines whether two strings sound alike using both Soundex and Double Metaphone.
    /// Returns true if either algorithm considers them similar.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns><c>true</c> if either Soundex or Double Metaphone considers the strings similar; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static bool AreSimilar(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (Soundex.AreSimilar(a, b))
            return true;

        var encodingA = DoubleMetaphone.Encode(a);
        var encodingB = DoubleMetaphone.Encode(b);

        return HasMetaphoneMatch(encodingA, encodingB);
    }

    /// <summary>
    /// Finds all candidates that sound similar to the query.
    /// </summary>
    /// <param name="query">The string to match against the candidates.</param>
    /// <param name="candidates">The collection of candidate strings to search.</param>
    /// <returns>A list of candidates that sound similar to the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> or <paramref name="candidates"/> is <c>null</c>.</exception>
    public static List<string> FindMatches(string query, IEnumerable<string> candidates)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(candidates);

        var matches = new List<string>();

        foreach (var candidate in candidates)
        {
            if (candidate is null)
                continue;

            if (AreSimilar(query, candidate))
                matches.Add(candidate);
        }

        return matches;
    }

    /// <summary>
    /// Computes a phonetic similarity score between 0 and 1.
    /// Soundex match = 0.5, Double Metaphone primary match = 0.5.
    /// Both matching = 1.0.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no phonetic similarity) and 1 (both algorithms match).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Score(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var score = 0.0;

        if (Soundex.AreSimilar(a, b))
            score += 0.5;

        var encodingA = DoubleMetaphone.Encode(a);
        var encodingB = DoubleMetaphone.Encode(b);

        if (HasMetaphoneMatch(encodingA, encodingB))
            score += 0.5;

        return score;
    }

    /// <summary>
    /// Determines whether any of the Double Metaphone codes match between two encodings.
    /// </summary>
    private static bool HasMetaphoneMatch(
        (string Primary, string Alternate) encodingA,
        (string Primary, string Alternate) encodingB)
    {
        if (encodingA.Primary.Length == 0 || encodingB.Primary.Length == 0)
            return false;

        if (string.Equals(encodingA.Primary, encodingB.Primary, StringComparison.Ordinal))
            return true;

        if (encodingA.Alternate.Length > 0 &&
            string.Equals(encodingA.Alternate, encodingB.Primary, StringComparison.Ordinal))
            return true;

        if (encodingB.Alternate.Length > 0 &&
            string.Equals(encodingA.Primary, encodingB.Alternate, StringComparison.Ordinal))
            return true;

        if (encodingA.Alternate.Length > 0 && encodingB.Alternate.Length > 0 &&
            string.Equals(encodingA.Alternate, encodingB.Alternate, StringComparison.Ordinal))
            return true;

        return false;
    }
}
