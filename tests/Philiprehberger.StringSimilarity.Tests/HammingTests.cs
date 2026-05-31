using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class HammingTests
{
    [Theory]
    [InlineData("karolin", "kathrin", 3)]
    [InlineData("karolin", "kerstin", 3)]
    [InlineData("1011101", "1001001", 2)]
    [InlineData("abc", "abc", 0)]
    public void Distance_ReturnsCorrectCount(string a, string b, int expected)
    {
        Assert.Equal(expected, Hamming.Distance(a, b));
    }

    [Fact]
    public void Distance_DifferentLengths_Throws()
    {
        Assert.Throws<ArgumentException>(() => Hamming.Distance("abc", "abcd"));
    }

    [Fact]
    public void Similarity_Identical_ReturnsOne()
    {
        Assert.Equal(1.0, Hamming.Similarity("hello", "hello"));
    }

    [Fact]
    public void Similarity_EmptyStrings_ReturnsOne()
    {
        Assert.Equal(1.0, Hamming.Similarity("", ""));
    }

    [Fact]
    public void Similarity_AllDifferent_ReturnsZero()
    {
        Assert.Equal(0.0, Hamming.Similarity("abc", "xyz"));
    }

    [Fact]
    public void SimilarityFacade_RoutesToHamming()
    {
        var direct = Hamming.Similarity("karolin", "kathrin");
        var facade = Similarity.HammingSimilarity("karolin", "kathrin");
        Assert.Equal(direct, facade);
    }
}
