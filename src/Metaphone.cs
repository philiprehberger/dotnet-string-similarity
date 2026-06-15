using System.Text;

namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Implements the original Metaphone phonetic encoding algorithm by Lawrence Philips (1990).
/// </summary>
public static class Metaphone
{
    private const int DefaultMaxLength = 4;

    /// <summary>
    /// Encodes a string to its Metaphone phonetic representation.
    /// </summary>
    /// <param name="value">The input string. Non-letter characters are ignored.</param>
    /// <param name="maxLength">The maximum length of the returned code. Defaults to 4.</param>
    /// <returns>The Metaphone code, in uppercase. Empty if no letters are present.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxLength"/> is less than 1.</exception>
    public static string Encode(string value, int maxLength = DefaultMaxLength)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (maxLength < 1)
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Max length must be at least 1.");

        var normalized = Normalize(value);
        if (normalized.Length == 0)
            return string.Empty;

        // Drop initial silent letters
        var start = 0;
        if (normalized.Length >= 2)
        {
            var prefix = normalized[..2];
            if (prefix is "AE" or "GN" or "KN" or "PN" or "WR")
                start = 1;
            else if (prefix == "WH")
                normalized = "W" + normalized[2..];
            else if (normalized[0] == 'X')
                normalized = "S" + normalized[1..];
        }

        var result = new StringBuilder(maxLength);
        var length = normalized.Length;

        for (var i = start; i < length && result.Length < maxLength; i++)
        {
            var c = normalized[i];
            var prev = i > 0 ? normalized[i - 1] : '\0';
            var next = i + 1 < length ? normalized[i + 1] : '\0';
            var next2 = i + 2 < length ? normalized[i + 2] : '\0';

            // Skip duplicate consecutive letters (except C)
            if (c == prev && c != 'C')
                continue;

            switch (c)
            {
                case 'A' or 'E' or 'I' or 'O' or 'U':
                    if (i == start)
                        result.Append(c);
                    break;

                case 'B':
                    if (!(i == length - 1 && prev == 'M'))
                        result.Append('B');
                    break;

                case 'C':
                    if (next == 'I' && next2 == 'A')
                        result.Append('X');
                    else if (next == 'H')
                    {
                        result.Append('X');
                        i++;
                    }
                    else if (next is 'I' or 'E' or 'Y')
                        result.Append('S');
                    else
                        result.Append('K');
                    break;

                case 'D':
                    if (next == 'G' && next2 is 'E' or 'Y' or 'I')
                    {
                        result.Append('J');
                        i++;
                    }
                    else
                        result.Append('T');
                    break;

                case 'F':
                    result.Append('F');
                    break;

                case 'G':
                    if (next == 'H')
                    {
                        if (i + 2 < length || (i > 0 && !IsVowel(prev)))
                        {
                            // silent
                        }
                        else
                            result.Append('F');
                        i++;
                    }
                    else if (next == 'N')
                    {
                        // silent G
                    }
                    else if (next is 'E' or 'I' or 'Y')
                        result.Append('J');
                    else
                        result.Append('K');
                    break;

                case 'H':
                    if (IsVowel(prev) && !IsVowel(next))
                        break;
                    if (prev is 'C' or 'S' or 'P' or 'T' or 'G')
                        break;
                    result.Append('H');
                    break;

                case 'J':
                    result.Append('J');
                    break;

                case 'K':
                    if (prev != 'C')
                        result.Append('K');
                    break;

                case 'L':
                    result.Append('L');
                    break;

                case 'M':
                    result.Append('M');
                    break;

                case 'N':
                    result.Append('N');
                    break;

                case 'P':
                    if (next == 'H')
                    {
                        result.Append('F');
                        i++;
                    }
                    else
                        result.Append('P');
                    break;

                case 'Q':
                    result.Append('K');
                    break;

                case 'R':
                    result.Append('R');
                    break;

                case 'S':
                    if (next == 'H')
                    {
                        result.Append('X');
                        i++;
                    }
                    else if (next == 'I' && next2 is 'O' or 'A')
                        result.Append('X');
                    else
                        result.Append('S');
                    break;

                case 'T':
                    if (next == 'H')
                    {
                        result.Append('0');
                        i++;
                    }
                    else if (next == 'I' && next2 is 'O' or 'A')
                        result.Append('X');
                    else
                        result.Append('T');
                    break;

                case 'V':
                    result.Append('F');
                    break;

                case 'W':
                    if (IsVowel(next))
                        result.Append('W');
                    break;

                case 'X':
                    result.Append('K');
                    if (result.Length < maxLength)
                        result.Append('S');
                    break;

                case 'Y':
                    if (IsVowel(next))
                        result.Append('Y');
                    break;

                case 'Z':
                    result.Append('S');
                    break;
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Returns whether two strings produce the same Metaphone code.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="maxLength">The maximum encoding length used for comparison. Defaults to 4.</param>
    /// <returns><c>true</c> if both strings encode to the same Metaphone code; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
    public static bool AreSimilar(string a, string b, int maxLength = DefaultMaxLength)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var codeA = Encode(a, maxLength);
        var codeB = Encode(b, maxLength);

        return codeA.Length > 0 && codeA == codeB;
    }

    private static string Normalize(string value)
    {
        var sb = new StringBuilder(value.Length);
        foreach (var c in value)
        {
            if (char.IsLetter(c))
                sb.Append(char.ToUpperInvariant(c));
        }
        return sb.ToString();
    }

    private static bool IsVowel(char c) => c is 'A' or 'E' or 'I' or 'O' or 'U';
}
