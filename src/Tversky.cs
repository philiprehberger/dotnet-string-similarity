namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Computes the Tversky index — an asymmetric generalization of the Jaccard and Dice set
/// similarity measures — over the distinct character sets of two strings.
/// </summary>
/// <remarks>
/// The index is defined as <c>|A∩B| / (|A∩B| + α·|A∖B| + β·|B∖A|)</c>, where <c>A</c> and <c>B</c>
/// are the character sets of the two inputs. With <c>α = β = 1</c> it equals the Jaccard index;
/// with <c>α = β = 0.5</c> it equals the Sørensen–Dice coefficient. Larger <c>α</c> penalizes
/// characters present only in the first string; larger <c>β</c> penalizes those only in the second,
/// which is useful for containment/subset-style matching.
/// </remarks>
public static class Tversky
{
    /// <summary>
    /// Computes the Tversky index over the distinct character sets of two strings.
    /// </summary>
    /// <param name="a">The first string (the "prototype", weighted by <paramref name="alpha"/>).</param>
    /// <param name="b">The second string (the "variant", weighted by <paramref name="beta"/>).</param>
    /// <param name="alpha">Weight applied to characters present only in <paramref name="a"/>. Defaults to 0.5. Must be non-negative.</param>
    /// <param name="beta">Weight applied to characters present only in <paramref name="b"/>. Defaults to 0.5. Must be non-negative.</param>
    /// <returns>A value between 0.0 (no shared characters) and 1.0 (identical character sets). Returns 1.0 if both strings are empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="alpha"/> or <paramref name="beta"/> is negative.</exception>
    public static double Similarity(string a, string b, double alpha = 0.5, double beta = 0.5)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (alpha < 0)
            throw new ArgumentOutOfRangeException(nameof(alpha), "Alpha must be non-negative.");
        if (beta < 0)
            throw new ArgumentOutOfRangeException(nameof(beta), "Beta must be non-negative.");

        if (a.Length == 0 && b.Length == 0)
            return 1.0;

        if (a.Length == 0 || b.Length == 0)
            return 0.0;

        var setA = new HashSet<char>(a);
        var setB = new HashSet<char>(b);

        var intersection = new HashSet<char>(setA);
        intersection.IntersectWith(setB);

        var intersectionCount = intersection.Count;
        var onlyInA = setA.Count - intersectionCount;
        var onlyInB = setB.Count - intersectionCount;

        var denominator = intersectionCount + (alpha * onlyInA) + (beta * onlyInB);

        return denominator == 0.0 ? 0.0 : intersectionCount / denominator;
    }
}
