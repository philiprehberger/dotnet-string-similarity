using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class TrigramTests
{
    [Fact]
    public void Similarity_IdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, Trigram.Similarity("hello", "hello"));
    }

    [Fact]
    public void Similarity_CompletelyDifferent_ReturnsZero()
    {
        Assert.Equal(0.0, Trigram.Similarity("abc", "xyz"));
    }

    [Fact]
    public void Similarity_BothEmpty_ReturnsOne()
    {
        Assert.Equal(1.0, Trigram.Similarity("", ""));
    }

    [Fact]
    public void Similarity_OneEmpty_ReturnsZero()
    {
        Assert.Equal(0.0, Trigram.Similarity("hello", ""));
        Assert.Equal(0.0, Trigram.Similarity("", "hello"));
    }

    [Fact]
    public void Similarity_SimilarStrings_ReturnsBetweenZeroAndOne()
    {
        var score = Trigram.Similarity("testing", "tester");
        Assert.True(score > 0.0 && score < 1.0);
    }

    [Fact]
    public void Similarity_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Trigram.Similarity(null!, "test"));
        Assert.Throws<ArgumentNullException>(() => Trigram.Similarity("test", null!));
    }

    [Fact]
    public void Similarity_ShortStrings_ReturnsZeroOrOne()
    {
        // Strings shorter than 3 chars have no trigrams.
        // Equal strings always return 1.0 (string equality short-circuit).
        Assert.Equal(1.0, Trigram.Similarity("ab", "ab"));
        // Both have no trigrams and are not equal, but both empty trigram sets → 1.0.
        Assert.Equal(1.0, Trigram.Similarity("ab", "cd"));
    }
}
