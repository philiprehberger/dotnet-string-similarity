using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class MetaphoneTests
{
    [Theory]
    [InlineData("Smith", "SM0")]
    [InlineData("Smyth", "SM0")]
    [InlineData("Knight", "NT")]
    [InlineData("Phone", "FN")]
    [InlineData("Quick", "KK")]
    [InlineData("Thompson", "0MPS")]
    public void Encode_KnownInputs_ReturnsExpectedCode(string input, string expected)
    {
        Assert.Equal(expected, Metaphone.Encode(input));
    }

    [Fact]
    public void Encode_WithEmptyString_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, Metaphone.Encode(string.Empty));
    }

    [Fact]
    public void Encode_StripsNonLetterCharacters()
    {
        Assert.Equal(Metaphone.Encode("Smith"), Metaphone.Encode("Sm1th!"));
    }

    [Fact]
    public void Encode_IsCaseInsensitive()
    {
        Assert.Equal(Metaphone.Encode("smith"), Metaphone.Encode("SMITH"));
    }

    [Fact]
    public void Encode_RespectsMaxLength()
    {
        var longCode = Metaphone.Encode("Constantinople", maxLength: 10);
        var shortCode = Metaphone.Encode("Constantinople", maxLength: 4);
        Assert.True(longCode.Length >= shortCode.Length);
        Assert.Equal(4, shortCode.Length);
    }

    [Fact]
    public void Encode_WithInvalidMaxLength_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Metaphone.Encode("test", maxLength: 0));
    }

    [Fact]
    public void AreSimilar_WithHomophones_ReturnsTrue()
    {
        Assert.True(Metaphone.AreSimilar("Smith", "Smyth"));
    }

    [Fact]
    public void AreSimilar_WithDifferentWords_ReturnsFalse()
    {
        Assert.False(Metaphone.AreSimilar("Smith", "Jones"));
    }

    [Fact]
    public void AreSimilar_WithEmptyInputs_ReturnsFalse()
    {
        Assert.False(Metaphone.AreSimilar(string.Empty, string.Empty));
    }

    [Fact]
    public void Encode_WithNullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Metaphone.Encode(null!));
    }

    [Fact]
    public void AreSimilar_WithNullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Metaphone.AreSimilar(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => Metaphone.AreSimilar("x", null!));
    }
}
