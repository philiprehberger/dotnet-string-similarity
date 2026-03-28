namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Implements the American Soundex phonetic encoding algorithm. Soundex encodes
/// names by sound so that similarly pronounced names produce the same code.
/// </summary>
public static class Soundex
{
    /// <summary>
    /// Encodes a string into its 4-character Soundex code.
    /// </summary>
    /// <param name="value">The string to encode.</param>
    /// <returns>A 4-character Soundex code (letter followed by three digits), or an empty string if the input contains no letters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    public static string Encode(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0)
            return string.Empty;

        // Find the first letter.
        var firstLetterIndex = -1;
        for (var i = 0; i < value.Length; i++)
        {
            if (char.IsLetter(value[i]))
            {
                firstLetterIndex = i;
                break;
            }
        }

        if (firstLetterIndex < 0)
            return string.Empty;

        var result = new char[4];
        result[0] = char.ToUpperInvariant(value[firstLetterIndex]);
        var count = 1;
        var lastCode = MapToDigit(value[firstLetterIndex]);

        for (var i = firstLetterIndex + 1; i < value.Length && count < 4; i++)
        {
            if (!char.IsLetter(value[i]))
                continue;

            var code = MapToDigit(value[i]);

            if (code == '0' || code == lastCode)
            {
                // H and W do not separate identical codes, but other non-coded letters do.
                var upper = char.ToUpperInvariant(value[i]);
                if (upper != 'H' && upper != 'W')
                    lastCode = code;
                continue;
            }

            result[count] = code;
            count++;
            lastCode = code;
        }

        // Pad with zeros.
        while (count < 4)
        {
            result[count] = '0';
            count++;
        }

        return new string(result);
    }

    /// <summary>
    /// Determines whether two strings have the same Soundex code.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns><c>true</c> if both strings produce the same non-empty Soundex code; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static bool AreSimilar(string a, string b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var codeA = Encode(a);
        var codeB = Encode(b);

        if (codeA.Length == 0 || codeB.Length == 0)
            return false;

        return string.Equals(codeA, codeB, StringComparison.Ordinal);
    }

    /// <summary>
    /// Maps a letter to its Soundex digit. Returns '0' for vowels and H/W/Y.
    /// </summary>
    private static char MapToDigit(char c)
    {
        return char.ToUpperInvariant(c) switch
        {
            'B' or 'F' or 'P' or 'V' => '1',
            'C' or 'G' or 'J' or 'K' or 'Q' or 'S' or 'X' or 'Z' => '2',
            'D' or 'T' => '3',
            'L' => '4',
            'M' or 'N' => '5',
            'R' => '6',
            _ => '0'
        };
    }
}
