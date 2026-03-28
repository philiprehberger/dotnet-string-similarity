using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class DoubleMetaphoneTests
{
    [Fact]
    public void Encode_Smith_ReturnsSMTVariant()
    {
        var (primary, alternate) = DoubleMetaphone.Encode("Smith");
        // Both primary and alternate should start with "SM"
        Assert.StartsWith("SM", primary);
        Assert.NotEmpty(alternate);
    }

    [Fact]
    public void Encode_EmptyString_ReturnsEmptyTuple()
    {
        var (primary, alternate) = DoubleMetaphone.Encode("");
        Assert.Equal(string.Empty, primary);
        Assert.Equal(string.Empty, alternate);
    }

    [Fact]
    public void Encode_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => DoubleMetaphone.Encode(null!));
    }

    [Fact]
    public void Encode_Phone_ProducesFCode()
    {
        var (primary, _) = DoubleMetaphone.Encode("Phone");
        Assert.StartsWith("FN", primary);
    }

    [Fact]
    public void Encode_Knight_ProducesNInResult()
    {
        var (primary, _) = DoubleMetaphone.Encode("Knight");
        // Knight should produce a code containing N (the K may or may not be silent depending on impl)
        Assert.Contains("N", primary);
    }

    [Fact]
    public void Encode_ReturnsTupleWithBothValues()
    {
        var result = DoubleMetaphone.Encode("Schmidt");
        Assert.False(string.IsNullOrEmpty(result.Primary));
        Assert.False(string.IsNullOrEmpty(result.Alternate));
    }

    [Theory]
    [InlineData("A")]
    [InlineData("ABC")]
    [InlineData("XYZ")]
    public void Encode_ShortStrings_DoesNotThrow(string input)
    {
        var (primary, alternate) = DoubleMetaphone.Encode(input);
        Assert.NotNull(primary);
        Assert.NotNull(alternate);
    }
}
