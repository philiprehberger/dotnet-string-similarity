using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class JaroTests
{
    [Fact]
    public void Jaro_WithIdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, Similarity.Jaro("hello", "hello"));
    }

    [Fact]
    public void Jaro_WithBothEmpty_ReturnsOne()
    {
        Assert.Equal(1.0, Similarity.Jaro(string.Empty, string.Empty));
    }

    [Fact]
    public void Jaro_WithOneEmpty_ReturnsZero()
    {
        Assert.Equal(0.0, Similarity.Jaro("abc", string.Empty));
        Assert.Equal(0.0, Similarity.Jaro(string.Empty, "abc"));
    }

    [Fact]
    public void Jaro_WithDisjointStrings_ReturnsZero()
    {
        Assert.Equal(0.0, Similarity.Jaro("abc", "xyz"));
    }

    [Theory]
    [InlineData("MARTHA", "MARHTA", 0.944)]
    [InlineData("DIXON", "DICKSONX", 0.767)]
    public void Jaro_ReturnsKnownValues(string a, string b, double expected)
    {
        Assert.Equal(expected, Similarity.Jaro(a, b), 3);
    }

    [Fact]
    public void Jaro_IsNotGreaterThanJaroWinkler_ForCommonPrefix()
    {
        // Jaro-Winkler adds a prefix boost, so Jaro must be <= Jaro-Winkler.
        var jaro = Similarity.Jaro("MARTHA", "MARHTA");
        var jaroWinkler = Similarity.JaroWinkler("MARTHA", "MARHTA");
        Assert.True(jaro < jaroWinkler);
    }

    [Fact]
    public void Jaro_ViaFindTopN_RanksCandidates()
    {
        var results = Similarity.FindTopN("MARTHA", new[] { "MARHTA", "zzzzzz" }, 2, SimilarityAlgorithm.Jaro);
        Assert.Equal("MARHTA", results[0].Value);
        Assert.Equal("Jaro", results[0].Algorithm);
    }

    [Fact]
    public void Jaro_WithNullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Similarity.Jaro(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => Similarity.Jaro("x", null!));
    }
}
