namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Hamming distance and similarity for equal-length strings.
/// </summary>
public static class Hamming
{
    /// <summary>
    /// Computes the Hamming distance between two equal-length strings,
    /// defined as the number of positions at which the characters differ.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The Hamming distance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when the inputs differ in length.</exception>
    public static int Distance(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (a.Length != b.Length)
            throw new ArgumentException("Hamming distance requires strings of equal length.");

        var distance = 0;
        for (var i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                distance++;
        }

        return distance;
    }

    /// <summary>
    /// Computes the normalized Hamming similarity between two equal-length strings,
    /// returning a value in the range [0, 1] where 1 is identical.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A similarity score between 0 and 1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when the inputs differ in length.</exception>
    public static double Similarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (a.Length == 0)
            return 1.0;

        return 1.0 - (double)Distance(a, b) / a.Length;
    }
}
