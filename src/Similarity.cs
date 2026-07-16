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
    /// Computes the Jaro similarity between two strings, without the Jaro-Winkler common-prefix boost.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no similarity) and 1 (exact match).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double Jaro(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return JaroWinklerAlgorithm.ComputeJaro(a, b);
    }

    /// <summary>
    /// Computes the Tversky index over the distinct character sets of two strings — an asymmetric
    /// generalization of Jaccard (<c>α = β = 1</c>) and Dice (<c>α = β = 0.5</c>).
    /// </summary>
    /// <param name="a">The first string (weighted by <paramref name="alpha"/>).</param>
    /// <param name="b">The second string (weighted by <paramref name="beta"/>).</param>
    /// <param name="alpha">Weight applied to characters present only in <paramref name="a"/>. Defaults to 0.5.</param>
    /// <param name="beta">Weight applied to characters present only in <paramref name="b"/>. Defaults to 0.5.</param>
    /// <returns>A value between 0 (no shared characters) and 1 (identical character sets).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="alpha"/> or <paramref name="beta"/> is negative.</exception>
    public static double Tversky(string a, string b, double alpha = 0.5, double beta = 0.5)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return StringSimilarity.Tversky.Similarity(a, b, alpha, beta);
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
    /// Computes a normalized Levenshtein similarity score between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0.0 (completely different) and 1.0 (identical), calculated as <c>1 - distance / max(len_a, len_b)</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double NormalizedLevenshtein(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return LevenshteinAlgorithm.Normalize(a, b);
    }

    /// <summary>
    /// Computes the Damerau-Levenshtein distance between two strings.
    /// Counts transpositions of adjacent characters as a single edit.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The minimum number of edit operations (insertions, deletions, substitutions, transpositions) required to transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static int DamerauLevenshteinDistance(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return DamerauLevenshtein.Distance(a, b);
    }

    /// <summary>
    /// Computes the normalized Damerau-Levenshtein similarity between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (completely different) and 1 (identical).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double DamerauLevenshteinSimilarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return DamerauLevenshtein.Normalize(a, b);
    }

    /// <summary>
    /// Computes the Hamming distance between two equal-length strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The number of positions at which the characters differ.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when the inputs differ in length.</exception>
    public static int HammingDistance(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return Hamming.Distance(a, b);
    }

    /// <summary>
    /// Computes the normalized Hamming similarity between two equal-length strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (completely different) and 1 (identical).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when the inputs differ in length.</exception>
    public static double HammingSimilarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return Hamming.Similarity(a, b);
    }

    /// <summary>
    /// Computes the cosine similarity between two strings using character bigrams.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no shared bigrams) and 1 (identical).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double CosineSimilarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return Cosine.Similarity(a, b);
    }

    /// <summary>
    /// Computes the Jaccard similarity over the distinct character sets of two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no shared characters) and 1 (identical character sets).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double JaccardSimilarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return Jaccard.Similarity(a, b);
    }

    /// <summary>
    /// Computes the Jaccard similarity over the whitespace-delimited token sets of two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A value between 0 (no shared tokens) and 1 (identical token sets).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static double JaccardTokenSimilarity(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return Jaccard.TokenSimilarity(a, b);
    }

    /// <summary>
    /// Finds the top N candidates ranked by similarity to the input string using the specified algorithm.
    /// </summary>
    /// <param name="input">The string to match against the candidates.</param>
    /// <param name="candidates">The collection of candidate strings to search.</param>
    /// <param name="n">The maximum number of results to return.</param>
    /// <param name="algorithm">The similarity algorithm to use for ranking.</param>
    /// <returns>A list of <see cref="MatchResult"/> objects sorted by descending similarity score, containing at most <paramref name="n"/> results.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> or <paramref name="candidates"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="n"/> is less than 1.</exception>
    public static List<MatchResult> FindTopN(string input, IEnumerable<string> candidates, int n, SimilarityAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(candidates);

        if (n < 1)
            throw new ArgumentOutOfRangeException(nameof(n), "n must be at least 1.");

        var results = new List<MatchResult>();
        var algorithmName = algorithm.ToString();

        foreach (var candidate in candidates)
        {
            if (candidate is null)
                continue;

            var score = ComputeScore(input, candidate, algorithm);
            results.Add(new MatchResult(candidate, score, algorithmName));
        }

        results.Sort((x, y) => y.Score.CompareTo(x.Score));

        return results.Count <= n ? results : results.GetRange(0, n);
    }

    /// <summary>
    /// Computes a similarity score using the specified algorithm.
    /// </summary>
    private static double ComputeScore(string a, string b, SimilarityAlgorithm algorithm)
    {
        return algorithm switch
        {
            SimilarityAlgorithm.Levenshtein => LevenshteinAlgorithm.Normalize(a, b),
            SimilarityAlgorithm.JaroWinkler => JaroWinklerAlgorithm.Compute(a, b),
            SimilarityAlgorithm.Dice => DiceCoefficientAlgorithm.Compute(a, b),
            SimilarityAlgorithm.Trigram => Trigram.Similarity(a, b),
            SimilarityAlgorithm.NormalizedLevenshtein => LevenshteinAlgorithm.Normalize(a, b),
            SimilarityAlgorithm.DamerauLevenshtein => DamerauLevenshtein.Normalize(a, b),
            SimilarityAlgorithm.LongestCommonSubsequence => LongestCommonSubsequence.Similarity(a, b),
            SimilarityAlgorithm.OverlapCoefficient => OverlapCoefficient.Similarity(a, b),
            SimilarityAlgorithm.Hamming => a.Length == b.Length ? Hamming.Similarity(a, b) : 0.0,
            SimilarityAlgorithm.Cosine => Cosine.Similarity(a, b),
            SimilarityAlgorithm.Jaccard => Jaccard.Similarity(a, b),
            SimilarityAlgorithm.Jaro => JaroWinklerAlgorithm.ComputeJaro(a, b),
            SimilarityAlgorithm.Tversky => StringSimilarity.Tversky.Similarity(a, b),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm))
        };
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
