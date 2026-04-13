namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes the Longest Common Subsequence (LCS) between two strings.
/// The LCS is the longest sequence of characters that appear in the same
/// order in both strings, but not necessarily contiguously.
/// </summary>
public static class LongestCommonSubsequence
{
    /// <summary>
    /// Computes the length of the longest common subsequence between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The number of characters in the longest common subsequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static int Length(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (a.Length == 0 || b.Length == 0)
            return 0;

        if (string.Equals(a, b, StringComparison.Ordinal))
            return a.Length;

        // Use a single-row optimization to reduce memory from O(m*n) to O(min(m,n)).
        if (a.Length > b.Length)
            (a, b) = (b, a);

        var previousRow = new int[a.Length + 1];
        var currentRow = new int[a.Length + 1];

        for (var j = 1; j <= b.Length; j++)
        {
            for (var i = 1; i <= a.Length; i++)
            {
                if (a[i - 1] == b[j - 1])
                {
                    currentRow[i] = previousRow[i - 1] + 1;
                }
                else
                {
                    currentRow[i] = Math.Max(previousRow[i], currentRow[i - 1]);
                }
            }

            (previousRow, currentRow) = (currentRow, previousRow);
            Array.Clear(currentRow);
        }

        return previousRow[a.Length];
    }

    /// <summary>
    /// Computes a normalized similarity score between 0 and 1 based on the longest common subsequence.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no common subsequence) and 1 (identical), calculated as <c>LCS length / max(len_a, len_b)</c>. Returns 1.0 if both strings are empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Similarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var maxLength = Math.Max(a.Length, b.Length);

        if (maxLength == 0)
            return 1.0;

        var lcsLength = Length(a, b);
        return (double)lcsLength / maxLength;
    }
}
