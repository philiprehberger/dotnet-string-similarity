using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class TverskyTests
{
    [Fact]
    public void Similarity_WithIdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, Tversky.Similarity("hello", "hello"));
    }

    [Fact]
    public void Similarity_WithBothEmpty_ReturnsOne()
    {
        Assert.Equal(1.0, Tversky.Similarity(string.Empty, string.Empty));
    }

    [Fact]
    public void Similarity_WithOneEmpty_ReturnsZero()
    {
        Assert.Equal(0.0, Tversky.Similarity("abc", string.Empty));
        Assert.Equal(0.0, Tversky.Similarity(string.Empty, "abc"));
    }

    [Fact]
    public void Similarity_WithDisjointSets_ReturnsZero()
    {
        Assert.Equal(0.0, Tversky.Similarity("abc", "xyz"));
    }

    [Fact]
    public void Similarity_WithSymmetricWeights_EqualsDiceOnSets()
    {
        // {a,b} vs {a,b,c}: inter=2, onlyInA=0, onlyInB=1 ⇒ 2 / (2 + 0.5*0 + 0.5*1) = 0.8
        Assert.Equal(0.8, Tversky.Similarity("ab", "abc", 0.5, 0.5), 6);
    }

    [Fact]
    public void Similarity_WithAlphaBetaOne_EqualsJaccard()
    {
        Assert.Equal(Jaccard.Similarity("ab", "abc"), Tversky.Similarity("ab", "abc", 1.0, 1.0), 6);
        Assert.Equal(Jaccard.Similarity("night", "nacht"), Tversky.Similarity("night", "nacht", 1.0, 1.0), 6);
    }

    [Fact]
    public void Similarity_IsAsymmetric_ForContainment()
    {
        // A={a,b,c}, B={a,b}. B is a subset of A.
        // alpha=0 (ignore chars only in A) ⇒ containment ⇒ 1.0
        Assert.Equal(1.0, Tversky.Similarity("abc", "ab", 0.0, 1.0), 6);
        // alpha=1, beta=0 ⇒ inter=2 / (2 + 1*1 + 0) = 2/3
        Assert.Equal(2.0 / 3.0, Tversky.Similarity("abc", "ab", 1.0, 0.0), 6);
    }

    [Fact]
    public void Similarity_ViaFindTopN_UsesSymmetricWeights()
    {
        var results = Similarity.FindTopN("abc", new[] { "abcd", "xyz" }, 2, SimilarityAlgorithm.Tversky);
        Assert.Equal("abcd", results[0].Value);
        Assert.Equal("Tversky", results[0].Algorithm);
    }

    [Fact]
    public void Similarity_WithNegativeWeight_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Tversky.Similarity("a", "b", -0.1, 0.5));
        Assert.Throws<ArgumentOutOfRangeException>(() => Tversky.Similarity("a", "b", 0.5, -0.1));
    }

    [Fact]
    public void Similarity_WithNullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Tversky.Similarity(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => Tversky.Similarity("x", null!));
    }
}
