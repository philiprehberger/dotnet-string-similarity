namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes Levenshtein edit distance using the Wagner-Fischer dynamic programming algorithm.
/// </summary>
internal static class LevenshteinAlgorithm
{
    /// <summary>
    /// Computes the raw Levenshtein edit distance between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The minimum number of single-character edits (insertions, deletions, substitutions) required to transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    internal static int ComputeDistance(string a, string b)
    {
        if (string.Equals(a, b, StringComparison.Ordinal))
            return 0;

        if (a.Length == 0)
            return b.Length;

        if (b.Length == 0)
            return a.Length;

        // Use a single-row optimization to reduce memory from O(m*n) to O(min(m,n)).
        if (a.Length > b.Length)
            (a, b) = (b, a);

        var previousRow = new int[a.Length + 1];
        var currentRow = new int[a.Length + 1];

        for (var i = 0; i <= a.Length; i++)
            previousRow[i] = i;

        for (var j = 1; j <= b.Length; j++)
        {
            currentRow[0] = j;

            for (var i = 1; i <= a.Length; i++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;

                currentRow[i] = Math.Min(
                    Math.Min(currentRow[i - 1] + 1, previousRow[i] + 1),
                    previousRow[i - 1] + cost);
            }

            (previousRow, currentRow) = (currentRow, previousRow);
        }

        return previousRow[a.Length];
    }

    /// <summary>
    /// Computes a normalized Levenshtein similarity score between 0 and 1.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (completely different) and 1 (identical), calculated as <c>1 - (distance / max(len_a, len_b))</c>.</returns>
    internal static double Normalize(string a, string b)
    {
        var maxLength = Math.Max(a.Length, b.Length);

        if (maxLength == 0)
            return 1.0;

        var distance = ComputeDistance(a, b);
        return 1.0 - ((double)distance / maxLength);
    }
}
