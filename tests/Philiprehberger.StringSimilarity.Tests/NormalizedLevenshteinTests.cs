using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class NormalizedLevenshteinTests
{
    [Fact]
    public void NormalizedLevenshtein_IdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, Similarity.NormalizedLevenshtein("hello", "hello"));
    }

    [Fact]
    public void NormalizedLevenshtein_CompletelyDifferent_ReturnsLowScore()
    {
        var score = Similarity.NormalizedLevenshtein("abc", "xyz");
        Assert.True(score < 0.5);
    }

    [Fact]
    public void NormalizedLevenshtein_BothEmpty_ReturnsOne()
    {
        Assert.Equal(1.0, Similarity.NormalizedLevenshtein("", ""));
    }

    [Fact]
    public void NormalizedLevenshtein_OneEmpty_ReturnsZero()
    {
        Assert.Equal(0.0, Similarity.NormalizedLevenshtein("hello", ""));
        Assert.Equal(0.0, Similarity.NormalizedLevenshtein("", "hello"));
    }

    [Fact]
    public void NormalizedLevenshtein_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Similarity.NormalizedLevenshtein(null!, "test"));
        Assert.Throws<ArgumentNullException>(() => Similarity.NormalizedLevenshtein("test", null!));
    }

    [Fact]
    public void NormalizedLevenshtein_KnownValue()
    {
        // "kitten" -> "sitting" = distance 3, max length 7 => 1 - 3/7 = 0.5714...
        var score = Similarity.NormalizedLevenshtein("kitten", "sitting");
        Assert.Equal(1.0 - (3.0 / 7.0), score, 3);
    }
}
