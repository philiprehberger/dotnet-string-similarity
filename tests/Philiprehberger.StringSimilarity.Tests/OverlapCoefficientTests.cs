using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class OverlapCoefficientTests
{
    [Fact]
    public void Similarity_IdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, OverlapCoefficient.Similarity("night", "night"));
    }

    [Fact]
    public void Similarity_CompletelyDifferent_ReturnsZero()
    {
        Assert.Equal(0.0, OverlapCoefficient.Similarity("abc", "xyz"));
    }

    [Fact]
    public void Similarity_BothEmpty_ReturnsOne()
    {
        Assert.Equal(1.0, OverlapCoefficient.Similarity("", ""));
    }

    [Fact]
    public void Similarity_OneEmpty_ReturnsZero()
    {
        Assert.Equal(0.0, OverlapCoefficient.Similarity("abc", ""));
        Assert.Equal(0.0, OverlapCoefficient.Similarity("", "abc"));
    }

    [Fact]
    public void Similarity_SingleCharacters_ReturnsOne()
    {
        // Both have zero bigrams, equal strings → 1.0
        Assert.Equal(1.0, OverlapCoefficient.Similarity("a", "a"));
    }

    [Fact]
    public void Similarity_SubstringBigrams_ReturnsHighScore()
    {
        // "app" bigrams: ["ap", "pp"]
        // "application" bigrams: ["ap", "pp", "pl", "li", "ic", "ca", "at", "ti", "io", "on"]
        // Intersection: 2, min size: 2 → 2/2 = 1.0
        Assert.Equal(1.0, OverlapCoefficient.Similarity("app", "application"));
    }

    [Fact]
    public void Similarity_PartialOverlap_ReturnsFraction()
    {
        var score = OverlapCoefficient.Similarity("night", "nacht");
        Assert.True(score > 0.0 && score < 1.0);
    }

    [Fact]
    public void Similarity_HigherThanDiceForSubstrings()
    {
        // Overlap coefficient should be >= Dice coefficient for substring-like pairs
        var overlap = OverlapCoefficient.Similarity("app", "application");
        var dice = Similarity.Dice("app", "application");
        Assert.True(overlap >= dice);
    }

    [Fact]
    public void Similarity_NullFirstArg_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OverlapCoefficient.Similarity(null!, "test"));
    }

    [Fact]
    public void Similarity_NullSecondArg_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OverlapCoefficient.Similarity("test", null!));
    }
}
