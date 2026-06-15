using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class JaccardTests
{
    [Fact]
    public void Similarity_WithIdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, Jaccard.Similarity("hello", "hello"));
    }

    [Fact]
    public void Similarity_WithBothEmpty_ReturnsOne()
    {
        Assert.Equal(1.0, Jaccard.Similarity(string.Empty, string.Empty));
    }

    [Fact]
    public void Similarity_WithOneEmpty_ReturnsZero()
    {
        Assert.Equal(0.0, Jaccard.Similarity("abc", string.Empty));
        Assert.Equal(0.0, Jaccard.Similarity(string.Empty, "abc"));
    }

    [Fact]
    public void Similarity_WithDisjointSets_ReturnsZero()
    {
        Assert.Equal(0.0, Jaccard.Similarity("abc", "xyz"));
    }

    [Theory]
    [InlineData("ab", "abc", 2.0 / 3.0)]
    [InlineData("abcd", "abef", 2.0 / 6.0)]
    public void Similarity_WithPartialOverlap_ReturnsExpectedRatio(string a, string b, double expected)
    {
        Assert.Equal(expected, Jaccard.Similarity(a, b), 6);
    }

    [Fact]
    public void Similarity_TreatsDuplicateCharactersAsSet()
    {
        // "aaaa" has set {a}, "ab" has set {a, b} ⇒ intersection 1, union 2
        Assert.Equal(0.5, Jaccard.Similarity("aaaa", "ab"));
    }

    [Fact]
    public void TokenSimilarity_WithIdenticalSentences_ReturnsOne()
    {
        Assert.Equal(1.0, Jaccard.TokenSimilarity("the quick fox", "the quick fox"));
    }

    [Fact]
    public void TokenSimilarity_WithPartialOverlap_ReturnsExpectedRatio()
    {
        // {the, quick, fox} vs {the, lazy, fox} ⇒ intersection {the, fox}=2, union=4
        Assert.Equal(0.5, Jaccard.TokenSimilarity("the quick fox", "the lazy fox"));
    }

    [Fact]
    public void TokenSimilarity_WithCustomSeparator_SplitsCorrectly()
    {
        Assert.Equal(1.0, Jaccard.TokenSimilarity("a,b,c", "c,b,a", ','));
    }

    [Fact]
    public void TokenSimilarity_WithDisjointTokens_ReturnsZero()
    {
        Assert.Equal(0.0, Jaccard.TokenSimilarity("alpha beta", "gamma delta"));
    }

    [Fact]
    public void Similarity_WithNullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Jaccard.Similarity(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => Jaccard.Similarity("x", null!));
    }
}
