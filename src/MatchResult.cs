namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Represents the result of a best-match search, containing the matched value,
/// its similarity score, and the algorithm that produced the highest score.
/// </summary>
/// <param name="Value">The candidate string that best matched the input.</param>
/// <param name="Score">The similarity score between 0 and 1, where 1 is an exact match.</param>
/// <param name="Algorithm">The name of the algorithm that produced the highest score.</param>
public record MatchResult(string Value, double Score, string Algorithm);
