using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class CosineTests
{
    [Fact]
    public void Similarity_IdenticalStrings_ReturnsOne()
    {
        Assert.Equal(1.0, Cosine.Similarity("hello", "hello"));
    }

    [Fact]
    public void Similarity_NoSharedBigrams_ReturnsZero()
    {
        Assert.Equal(0.0, Cosine.Similarity("abc", "xyz"));
    }

    [Fact]
    public void Similarity_PartialOverlap_BetweenZeroAndOne()
    {
        var score = Cosine.Similarity("night", "nacht");
        Assert.InRange(score, 0.0, 1.0);
        Assert.True(score > 0.0 && score < 1.0);
    }

    [Fact]
    public void Similarity_TooShortForBigrams_ReturnsZero()
    {
        Assert.Equal(0.0, Cosine.Similarity("a", "b"));
    }

    [Fact]
    public void SimilarityFacade_RoutesToCosine()
    {
        var direct = Cosine.Similarity("night", "nacht");
        var facade = Similarity.CosineSimilarity("night", "nacht");
        Assert.Equal(direct, facade);
    }
}
