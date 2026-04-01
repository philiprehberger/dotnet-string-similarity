using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class DamerauLevenshteinTests
{
    [Fact]
    public void Distance_CaToAbc_ReturnsExpected()
    {
        var distance = DamerauLevenshtein.Distance("ca", "abc");
        Assert.Equal(3, distance);
    }

    [Fact]
    public void Distance_Transposition_ReturnsSingleEdit()
    {
        var distance = DamerauLevenshtein.Distance("ab", "ba");
        Assert.Equal(1, distance);
    }

    [Fact]
    public void Distance_IdenticalStrings_ReturnsZero()
    {
        Assert.Equal(0, DamerauLevenshtein.Distance("hello", "hello"));
    }

    [Fact]
    public void Distance_EmptyStrings_ReturnsZero()
    {
        Assert.Equal(0, DamerauLevenshtein.Distance("", ""));
    }

    [Fact]
    public void Distance_OneEmptyString_ReturnsLengthOfOther()
    {
        Assert.Equal(3, DamerauLevenshtein.Distance("", "abc"));
        Assert.Equal(3, DamerauLevenshtein.Distance("abc", ""));
    }

    [Fact]
    public void Distance_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => DamerauLevenshtein.Distance(null!, "test"));
        Assert.Throws<ArgumentNullException>(() => DamerauLevenshtein.Distance("test", null!));
    }

    [Fact]
    public void Normalize_IdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, DamerauLevenshtein.Normalize("hello", "hello"));
    }

    [Fact]
    public void Normalize_CompletelyDifferent_ReturnsBetweenZeroAndOne()
    {
        var score = DamerauLevenshtein.Normalize("abc", "xyz");
        Assert.InRange(score, 0.0, 1.0);
    }

    [Fact]
    public void Normalize_EmptyStrings_ReturnsOne()
    {
        Assert.Equal(1.0, DamerauLevenshtein.Normalize("", ""));
    }

    [Fact]
    public void Normalize_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => DamerauLevenshtein.Normalize(null!, "test"));
        Assert.Throws<ArgumentNullException>(() => DamerauLevenshtein.Normalize("test", null!));
    }

    [Fact]
    public void DamerauLevenshteinDistance_ViaFacade_Works()
    {
        Assert.Equal(1, Similarity.DamerauLevenshteinDistance("ab", "ba"));
    }

    [Fact]
    public void DamerauLevenshteinSimilarity_ViaFacade_Works()
    {
        var score = Similarity.DamerauLevenshteinSimilarity("ab", "ba");
        Assert.InRange(score, 0.0, 1.0);
    }
}
