using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class LongestCommonSubsequenceTests
{
    [Fact]
    public void Length_IdenticalStrings_ReturnsStringLength()
    {
        Assert.Equal(6, LongestCommonSubsequence.Length("kitten", "kitten"));
    }

    [Fact]
    public void Length_CompletelyDifferent_ReturnsZero()
    {
        Assert.Equal(0, LongestCommonSubsequence.Length("abc", "xyz"));
    }

    [Fact]
    public void Length_EmptyStrings_ReturnsZero()
    {
        Assert.Equal(0, LongestCommonSubsequence.Length("", ""));
    }

    [Fact]
    public void Length_OneEmpty_ReturnsZero()
    {
        Assert.Equal(0, LongestCommonSubsequence.Length("abc", ""));
        Assert.Equal(0, LongestCommonSubsequence.Length("", "abc"));
    }

    [Fact]
    public void Length_KnownValue_ReturnsExpected()
    {
        // LCS of "ABCBDAB" and "BDCAB" is "BCAB" (length 4)
        Assert.Equal(4, LongestCommonSubsequence.Length("ABCBDAB", "BDCAB"));
    }

    [Fact]
    public void Length_SubstringMatch_ReturnsSubstringLength()
    {
        // "ace" is a subsequence of "abcde"
        Assert.Equal(3, LongestCommonSubsequence.Length("ace", "abcde"));
    }

    [Fact]
    public void Similarity_IdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, LongestCommonSubsequence.Similarity("hello", "hello"));
    }

    [Fact]
    public void Similarity_CompletelyDifferent_ReturnsZero()
    {
        Assert.Equal(0.0, LongestCommonSubsequence.Similarity("abc", "xyz"));
    }

    [Fact]
    public void Similarity_BothEmpty_ReturnsOne()
    {
        Assert.Equal(1.0, LongestCommonSubsequence.Similarity("", ""));
    }

    [Fact]
    public void Similarity_ReturnsNormalizedValue()
    {
        // LCS("kitten", "sitting") = "ittn" (length 4), max length = 7
        var score = LongestCommonSubsequence.Similarity("kitten", "sitting");
        Assert.True(score > 0.0 && score < 1.0);
    }

    [Fact]
    public void Length_NullFirstArg_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LongestCommonSubsequence.Length(null!, "test"));
    }

    [Fact]
    public void Similarity_NullSecondArg_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LongestCommonSubsequence.Similarity("test", null!));
    }
}
