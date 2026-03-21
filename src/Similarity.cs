namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Provides string similarity algorithms with normalized 0-1 scoring.
/// Includes Levenshtein distance, Jaro-Winkler similarity, Dice coefficient,
/// and best-match search across a set of candidates.
/// </summary>
public static class Similarity
{
    /// <summary>
    /// Computes the normalized Levenshtein similarity between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (completely different) and 1 (identical).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Levenshtein(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return LevenshteinAlgorithm.Normalize(a, b);
    }

    /// <summary>
    /// Computes the Jaro-Winkler similarity between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no similarity) and 1 (exact match).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double JaroWinkler(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return JaroWinklerAlgorithm.Compute(a, b);
    }

    /// <summary>
    /// Computes the Sorensen-Dice coefficient between two strings using bigram overlap.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no shared bigrams) and 1 (identical).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Dice(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return DiceCoefficientAlgorithm.Compute(a, b);
    }

    /// <summary>
    /// Computes the raw Levenshtein edit distance between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The minimum number of single-character edits (insertions, deletions, substitutions) required to transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static int Distance(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return LevenshteinAlgorithm.ComputeDistance(a, b);
    }

    /// <summary>
    /// Finds the best matching candidate for the given input string by evaluating all
    /// three similarity algorithms and returning the highest-scoring result.
    /// </summary>
    /// <param name="input">The string to match against the candidates.</param>
    /// <param name="candidates">The collection of candidate strings to search.</param>
    /// <param name="threshold">The minimum similarity score (0-1) a candidate must meet. Defaults to 0.</param>
    /// <returns>A <see cref="MatchResult"/> containing the best match, or <c>null</c> if no candidate meets the threshold.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> or <paramref name="candidates"/> is <c>null</c>.</exception>
    public static MatchResult? BestMatch(string input, IEnumerable<string> candidates, double threshold = 0.0)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(candidates);

        MatchResult? best = null;

        foreach (var candidate in candidates)
        {
            if (candidate is null)
                continue;

            var (score, algorithm) = BestAlgorithmScore(input, candidate);

            if (score < threshold)
                continue;

            if (best is null || score > best.Score)
                best = new MatchResult(candidate, score, algorithm);
        }

        return best;
    }

    /// <summary>
    /// Evaluates all three algorithms and returns the highest score with its algorithm name.
    /// </summary>
    private static (double Score, string Algorithm) BestAlgorithmScore(string a, string b)
    {
        var levenshtein = LevenshteinAlgorithm.Normalize(a, b);
        var jaroWinkler = JaroWinklerAlgorithm.Compute(a, b);
        var dice = DiceCoefficientAlgorithm.Compute(a, b);

        if (levenshtein >= jaroWinkler && levenshtein >= dice)
            return (levenshtein, "Levenshtein");

        if (jaroWinkler >= dice)
            return (jaroWinkler, "JaroWinkler");

        return (dice, "Dice");
    }
}
