namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes Jaro and Jaro-Winkler similarity between two strings.
/// </summary>
internal static class JaroWinklerAlgorithm
{
    /// <summary>
    /// The scaling factor for the common prefix adjustment in Jaro-Winkler.
    /// </summary>
    private const double PrefixScale = 0.1;

    /// <summary>
    /// The maximum prefix length considered for the Jaro-Winkler adjustment.
    /// </summary>
    private const int MaxPrefixLength = 4;

    /// <summary>
    /// Computes the Jaro-Winkler similarity between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no similarity) and 1 (exact match).</returns>
    internal static double Compute(string a, string b)
    {
        var jaroScore = ComputeJaro(a, b);

        if (jaroScore == 0.0)
            return 0.0;

        var prefixLength = CommonPrefixLength(a, b);
        return jaroScore + (prefixLength * PrefixScale * (1.0 - jaroScore));
    }

    /// <summary>
    /// Computes the Jaro similarity between two strings.
    /// </summary>
    private static double ComputeJaro(string a, string b)
    {
        if (string.Equals(a, b, StringComparison.Ordinal))
            return 1.0;

        if (a.Length == 0 || b.Length == 0)
            return 0.0;

        var matchWindow = Math.Max(0, (Math.Max(a.Length, b.Length) / 2) - 1);

        var aMatched = new bool[a.Length];
        var bMatched = new bool[b.Length];

        var matches = 0;
        var transpositions = 0;

        // Find matching characters within the match window.
        for (var i = 0; i < a.Length; i++)
        {
            var start = Math.Max(0, i - matchWindow);
            var end = Math.Min(i + matchWindow + 1, b.Length);

            for (var j = start; j < end; j++)
            {
                if (bMatched[j] || a[i] != b[j])
                    continue;

                aMatched[i] = true;
                bMatched[j] = true;
                matches++;
                break;
            }
        }

        if (matches == 0)
            return 0.0;

        // Count transpositions by comparing matched characters in order.
        var k = 0;
        for (var i = 0; i < a.Length; i++)
        {
            if (!aMatched[i])
                continue;

            while (!bMatched[k])
                k++;

            if (a[i] != b[k])
                transpositions++;

            k++;
        }

        var m = (double)matches;
        return ((m / a.Length) + (m / b.Length) + ((m - (transpositions / 2.0)) / m)) / 3.0;
    }

    /// <summary>
    /// Returns the length of the common prefix, up to <see cref="MaxPrefixLength"/> characters.
    /// </summary>
    private static int CommonPrefixLength(string a, string b)
    {
        var limit = Math.Min(Math.Min(a.Length, b.Length), MaxPrefixLength);
        var length = 0;

        for (var i = 0; i < limit; i++)
        {
            if (a[i] != b[i])
                break;

            length++;
        }

        return length;
    }
}
