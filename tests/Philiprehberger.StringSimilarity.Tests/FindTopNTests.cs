using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class FindTopNTests
{
    [Fact]
    public void FindTopN_ReturnsCorrectNumberOfResults()
    {
        var candidates = new[] { "apple", "application", "ape", "apex", "banana" };
        var results = Similarity.FindTopN("app", candidates, 3, SimilarityAlgorithm.Levenshtein);
        Assert.Equal(3, results.Count);
    }

    [Fact]
    public void FindTopN_ResultsAreSortedByDescendingScore()
    {
        var candidates = new[] { "apple", "application", "ape", "apex", "banana" };
        var results = Similarity.FindTopN("app", candidates, 5, SimilarityAlgorithm.Levenshtein);

        for (var i = 1; i < results.Count; i++)
            Assert.True(results[i - 1].Score >= results[i].Score);
    }

    [Fact]
    public void FindTopN_FewerCandidatesThanN_ReturnsAll()
    {
        var candidates = new[] { "apple", "ape" };
        var results = Similarity.FindTopN("app", candidates, 10, SimilarityAlgorithm.Dice);
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void FindTopN_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Similarity.FindTopN(null!, new[] { "a" }, 1, SimilarityAlgorithm.Levenshtein));
    }

    [Fact]
    public void FindTopN_NLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Similarity.FindTopN("app", new[] { "apple" }, 0, SimilarityAlgorithm.Levenshtein));
    }

    [Fact]
    public void FindTopN_TrigramAlgorithm_Works()
    {
        var candidates = new[] { "hello", "world", "help" };
        var results = Similarity.FindTopN("hell", candidates, 2, SimilarityAlgorithm.Trigram);
        Assert.Equal(2, results.Count);
        Assert.Equal("Trigram", results[0].Algorithm);
    }

    [Fact]
    public void FindTopN_JaroWinklerAlgorithm_Works()
    {
        var candidates = new[] { "kitten", "sitting", "kitchen" };
        var results = Similarity.FindTopN("kitten", candidates, 2, SimilarityAlgorithm.JaroWinkler);
        Assert.Equal(2, results.Count);
        Assert.Equal("kitten", results[0].Value);
        Assert.Equal(1.0, results[0].Score);
    }
}
