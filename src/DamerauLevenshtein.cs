namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes the Damerau-Levenshtein distance which extends Levenshtein by treating
/// transpositions of two adjacent characters as a single edit operation.
/// Uses the Optimal String Alignment (OSA) variant.
/// </summary>
public static class DamerauLevenshtein
{
    /// <summary>
    /// Computes the Damerau-Levenshtein distance between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The minimum number of edit operations (insertions, deletions, substitutions, transpositions) required to transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static int Distance(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (string.Equals(a, b, StringComparison.Ordinal))
            return 0;

        if (a.Length == 0)
            return b.Length;

        if (b.Length == 0)
            return a.Length;

        var matrix = new int[a.Length + 1, b.Length + 1];

        for (var i = 0; i <= a.Length; i++)
            matrix[i, 0] = i;

        for (var j = 0; j <= b.Length; j++)
            matrix[0, j] = j;

        for (var i = 1; i <= a.Length; i++)
        {
            for (var j = 1; j <= b.Length; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;

                matrix[i, j] = Math.Min(
                    Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);

                if (i > 1 && j > 1 && a[i - 1] == b[j - 2] && a[i - 2] == b[j - 1])
                    matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
            }
        }

        return matrix[a.Length, b.Length];
    }

    /// <summary>
    /// Computes a normalized Damerau-Levenshtein similarity score between 0 and 1.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (completely different) and 1 (identical), calculated as <c>1 - (distance / max(len_a, len_b))</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Normalize(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var maxLength = Math.Max(a.Length, b.Length);

        if (maxLength == 0)
            return 1.0;

        var distance = Distance(a, b);
        return 1.0 - ((double)distance / maxLength);
    }
}
