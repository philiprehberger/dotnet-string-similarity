using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class FuzzySearchTests
{
    [Fact]
    public void Score_ReturnsBetweenZeroAndOne()
    {
        var fuzzy = new FuzzySearch();
        var score = fuzzy.Score("kitten", "sitting");
        Assert.InRange(score, 0.0, 1.0);
    }

    [Fact]
    public void Score_IdenticalStrings_ReturnsOne()
    {
        var fuzzy = new FuzzySearch();
        Assert.Equal(1.0, fuzzy.Score("hello", "hello"));
    }

    [Fact]
    public void Find_ReturnsResultsAboveThreshold()
    {
        var fuzzy = new FuzzySearch();
        var candidates = new[] { "apple", "application", "ape", "banana" };
        var results = fuzzy.Find("app", candidates, threshold: 0.3);

        foreach (var result in results)
            Assert.True(result.Score >= 0.3);
    }

    [Fact]
    public void Find_ResultsAreSortedByDescendingScore()
    {
        var fuzzy = new FuzzySearch();
        var candidates = new[] { "apple", "application", "ape", "apex", "banana" };
        var results = fuzzy.Find("app", candidates);

        for (var i = 1; i < results.Count; i++)
            Assert.True(results[i - 1].Score >= results[i].Score);
    }

    [Fact]
    public void Find_UseFuzzySearchAlgorithmName()
    {
        var fuzzy = new FuzzySearch();
        var results = fuzzy.Find("app", new[] { "apple" });

        Assert.Single(results);
        Assert.Equal("FuzzySearch", results[0].Algorithm);
    }

    [Fact]
    public void FindTopN_ReturnsCorrectCount()
    {
        var fuzzy = new FuzzySearch();
        var candidates = new[] { "apple", "application", "ape", "apex", "banana" };
        var results = fuzzy.FindTopN("app", candidates, 2);

        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void FindTopN_FewerCandidatesThanN_ReturnsAll()
    {
        var fuzzy = new FuzzySearch();
        var candidates = new[] { "apple", "ape" };
        var results = fuzzy.FindTopN("app", candidates, 10);

        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void CustomWeights_AffectsScore()
    {
        var defaultFuzzy = new FuzzySearch();
        var customFuzzy = new FuzzySearch(new Dictionary<SimilarityAlgorithm, double>
        {
            [SimilarityAlgorithm.JaroWinkler] = 5.0,
            [SimilarityAlgorithm.Levenshtein] = 0.1
        });

        var defaultScore = defaultFuzzy.Score("kitten", "sitting");
        var customScore = customFuzzy.Score("kitten", "sitting");

        Assert.NotEqual(defaultScore, customScore);
    }

    [Fact]
    public void FindTopN_NLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        var fuzzy = new FuzzySearch();
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            fuzzy.FindTopN("app", new[] { "apple" }, 0));
    }
}
