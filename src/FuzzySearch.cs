namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Combines multiple similarity algorithms with configurable weights to perform fuzzy search.
/// </summary>
public sealed class FuzzySearch
{
    private readonly Dictionary<SimilarityAlgorithm, double> _weights;

    /// <summary>
    /// Creates a FuzzySearch with default equal weights for Levenshtein, JaroWinkler, and Dice.
    /// </summary>
    public FuzzySearch()
    {
        _weights = new Dictionary<SimilarityAlgorithm, double>
        {
            [SimilarityAlgorithm.Levenshtein] = 1.0,
            [SimilarityAlgorithm.JaroWinkler] = 1.0,
            [SimilarityAlgorithm.Dice] = 1.0
        };
    }

    /// <summary>
    /// Creates a FuzzySearch with the specified algorithm weights.
    /// </summary>
    /// <param name="weights">A dictionary mapping similarity algorithms to their weights.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="weights"/> is <c>null</c>.</exception>
    public FuzzySearch(Dictionary<SimilarityAlgorithm, double> weights)
    {
        ArgumentNullException.ThrowIfNull(weights);

        _weights = new Dictionary<SimilarityAlgorithm, double>(weights);
    }

    /// <summary>
    /// Computes a weighted similarity score between two strings.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>A weighted average similarity score between 0 and 1.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public double Score(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var totalWeight = 0.0;
        var totalScore = 0.0;

        foreach (var (algorithm, weight) in _weights)
        {
            var score = ComputeScore(a, b, algorithm);
            totalScore += weight * score;
            totalWeight += weight;
        }

        if (totalWeight == 0.0)
            return 0.0;

        return totalScore / totalWeight;
    }

    /// <summary>
    /// Finds candidates matching the query above the given threshold, sorted by score descending.
    /// </summary>
    /// <param name="query">The string to match against the candidates.</param>
    /// <param name="candidates">The collection of candidate strings to search.</param>
    /// <param name="threshold">The minimum similarity score (0-1) a candidate must meet. Defaults to 0.</param>
    /// <returns>A list of <see cref="MatchResult"/> objects sorted by descending similarity score.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> or <paramref name="candidates"/> is <c>null</c>.</exception>
    public List<MatchResult> Find(string query, IEnumerable<string> candidates, double threshold = 0.0)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(candidates);

        var results = new List<MatchResult>();

        foreach (var candidate in candidates)
        {
            if (candidate is null)
                continue;

            var score = Score(query, candidate);

            if (score >= threshold)
                results.Add(new MatchResult(candidate, score, "FuzzySearch"));
        }

        results.Sort((x, y) => y.Score.CompareTo(x.Score));

        return results;
    }

    /// <summary>
    /// Finds the top N candidates matching the query, sorted by score descending.
    /// </summary>
    /// <param name="query">The string to match against the candidates.</param>
    /// <param name="candidates">The collection of candidate strings to search.</param>
    /// <param name="n">The maximum number of results to return.</param>
    /// <returns>A list of <see cref="MatchResult"/> objects sorted by descending similarity score, containing at most <paramref name="n"/> results.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> or <paramref name="candidates"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="n"/> is less than 1.</exception>
    public List<MatchResult> FindTopN(string query, IEnumerable<string> candidates, int n)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(candidates);

        if (n < 1)
            throw new ArgumentOutOfRangeException(nameof(n), "n must be at least 1.");

        var results = Find(query, candidates);

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
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm))
        };
    }
}
