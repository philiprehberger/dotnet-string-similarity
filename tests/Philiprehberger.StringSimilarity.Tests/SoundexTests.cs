using Xunit;
using Philiprehberger.StringSimilarity;

namespace Philiprehberger.StringSimilarity.Tests;

public class SoundexTests
{
    [Theory]
    [InlineData("Robert", "R163")]
    [InlineData("Rupert", "R163")]
    [InlineData("Ashcraft", "A261")]
    [InlineData("Tymczak", "T522")]
    [InlineData("Pfister", "P236")]
    public void Encode_ReturnsCorrectSoundexCode(string input, string expected)
    {
        var result = Soundex.Encode(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Encode_EmptyString_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, Soundex.Encode(""));
    }

    [Fact]
    public void Encode_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Soundex.Encode(null!));
    }

    [Fact]
    public void Encode_SingleLetter_ReturnsPaddedCode()
    {
        Assert.Equal("A000", Soundex.Encode("A"));
    }

    [Theory]
    [InlineData("Robert", "Rupert", true)]
    [InlineData("Smith", "Smyth", true)]
    [InlineData("Robert", "Smith", false)]
    public void AreSimilar_ComparesCodesCorrectly(string a, string b, bool expected)
    {
        Assert.Equal(expected, Soundex.AreSimilar(a, b));
    }

    [Fact]
    public void AreSimilar_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Soundex.AreSimilar(null!, "test"));
        Assert.Throws<ArgumentNullException>(() => Soundex.AreSimilar("test", null!));
    }
}
