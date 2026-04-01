using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class PhoneticMatcherTests
{
    [Fact]
    public void AreSimilar_SmithSmyth_ReturnsTrue()
    {
        Assert.True(PhoneticMatcher.AreSimilar("Smith", "Smyth"));
    }

    [Fact]
    public void AreSimilar_CompletelyDifferent_ReturnsFalse()
    {
        Assert.False(PhoneticMatcher.AreSimilar("Apple", "Zebra"));
    }

    [Fact]
    public void AreSimilar_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PhoneticMatcher.AreSimilar(null!, "test"));
        Assert.Throws<ArgumentNullException>(() => PhoneticMatcher.AreSimilar("test", null!));
    }

    [Fact]
    public void FindMatches_FindsPhoneticallySimialarCandidates()
    {
        var candidates = new[] { "Smith", "Smyth", "Jones", "Schmidt" };
        var matches = PhoneticMatcher.FindMatches("Smith", candidates);

        Assert.Contains("Smith", matches);
        Assert.Contains("Smyth", matches);
    }

    [Fact]
    public void FindMatches_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PhoneticMatcher.FindMatches(null!, new[] { "a" }));
    }

    [Fact]
    public void Score_ReturnsBetweenZeroAndOne()
    {
        var score = PhoneticMatcher.Score("Smith", "Smyth");
        Assert.InRange(score, 0.0, 1.0);
    }

    [Fact]
    public void Score_IdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, PhoneticMatcher.Score("Robert", "Robert"));
    }

    [Fact]
    public void Score_CompletelyDifferent_ReturnsZero()
    {
        Assert.Equal(0.0, PhoneticMatcher.Score("Apple", "Zebra"));
    }

    [Fact]
    public void Score_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PhoneticMatcher.Score(null!, "test"));
        Assert.Throws<ArgumentNullException>(() => PhoneticMatcher.Score("test", null!));
    }
}
