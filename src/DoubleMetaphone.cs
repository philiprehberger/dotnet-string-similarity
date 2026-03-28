namespace Philiprehberger.StringSimilarity;

/// <summary>
/// Implements the Double Metaphone phonetic encoding algorithm. Produces both a primary
/// and an alternate encoding to handle words that may be pronounced in multiple ways.
/// </summary>
public static class DoubleMetaphone
{
    /// <summary>
    /// Encodes a string using the Double Metaphone algorithm.
    /// </summary>
    /// <param name="value">The string to encode.</param>
    /// <returns>A tuple containing the primary and alternate metaphone codes. Both codes are uppercase strings of up to 4 characters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    public static (string Primary, string Alternate) Encode(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0)
            return (string.Empty, string.Empty);

        var input = value.ToUpperInvariant();
        var primary = new System.Text.StringBuilder(4);
        var alternate = new System.Text.StringBuilder(4);
        var index = 0;
        var length = input.Length;
        var lastIndex = length - 1;

        // Skip initial silent letters.
        if (StartsWith(input, 0, "GN", "KN", "PN", "AE", "WR"))
            index++;

        // Handle initial X -> S.
        if (CharAt(input, 0) == 'X')
        {
            AddCodes(primary, alternate, "S", "S");
            index++;
        }

        while (index < length && primary.Length < 4)
        {
            var c = input[index];

            // Skip duplicate letters (except C).
            if (c != 'C' && index > 0 && input[index - 1] == c)
            {
                index++;
                continue;
            }

            switch (c)
            {
                case 'A':
                case 'E':
                case 'I':
                case 'O':
                case 'U':
                    // Vowels only coded at the start.
                    if (index == 0)
                        AddCodes(primary, alternate, "A", "A");
                    index++;
                    break;

                case 'B':
                    AddCodes(primary, alternate, "P", "P");
                    index += CharAt(input, index + 1) == 'B' ? 2 : 1;
                    break;

                case 'C':
                    index = HandleC(input, index, length, primary, alternate);
                    break;

                case 'D':
                    if (StartsWith(input, index, "DG"))
                    {
                        if (Contains("IEY", CharAt(input, index + 2)))
                        {
                            AddCodes(primary, alternate, "J", "J");
                            index += 3;
                        }
                        else
                        {
                            AddCodes(primary, alternate, "TK", "TK");
                            index += 2;
                        }
                    }
                    else
                    {
                        AddCodes(primary, alternate, "T", "T");
                        index += StartsWith(input, index, "DT", "DD") ? 2 : 1;
                    }
                    break;

                case 'F':
                    AddCodes(primary, alternate, "F", "F");
                    index += CharAt(input, index + 1) == 'F' ? 2 : 1;
                    break;

                case 'G':
                    index = HandleG(input, index, length, primary, alternate);
                    break;

                case 'H':
                    if (IsVowel(CharAt(input, index + 1)) && index > 0 && IsVowel(input[index - 1]))
                    {
                        index += 2;
                    }
                    else
                    {
                        if (index == 0 || IsVowel(CharAt(input, index - 1)))
                            AddCodes(primary, alternate, "H", "H");
                        index++;
                    }
                    break;

                case 'J':
                    AddCodes(primary, alternate, "J", "H");
                    index += CharAt(input, index + 1) == 'J' ? 2 : 1;
                    break;

                case 'K':
                    AddCodes(primary, alternate, "K", "K");
                    index += CharAt(input, index + 1) == 'K' ? 2 : 1;
                    break;

                case 'L':
                    AddCodes(primary, alternate, "L", "L");
                    index += CharAt(input, index + 1) == 'L' ? 2 : 1;
                    break;

                case 'M':
                    AddCodes(primary, alternate, "M", "M");
                    index += CharAt(input, index + 1) == 'M' ? 2 : 1;
                    break;

                case 'N':
                    AddCodes(primary, alternate, "N", "N");
                    index += CharAt(input, index + 1) == 'N' ? 2 : 1;
                    break;

                case 'P':
                    if (CharAt(input, index + 1) == 'H')
                    {
                        AddCodes(primary, alternate, "F", "F");
                        index += 2;
                    }
                    else
                    {
                        AddCodes(primary, alternate, "P", "P");
                        index += CharAt(input, index + 1) == 'P' ? 2 : 1;
                    }
                    break;

                case 'Q':
                    AddCodes(primary, alternate, "K", "K");
                    index += CharAt(input, index + 1) == 'Q' ? 2 : 1;
                    break;

                case 'R':
                    AddCodes(primary, alternate, "R", "R");
                    index += CharAt(input, index + 1) == 'R' ? 2 : 1;
                    break;

                case 'S':
                    index = HandleS(input, index, length, primary, alternate);
                    break;

                case 'T':
                    index = HandleT(input, index, length, primary, alternate);
                    break;

                case 'V':
                    AddCodes(primary, alternate, "F", "F");
                    index += CharAt(input, index + 1) == 'V' ? 2 : 1;
                    break;

                case 'W':
                    if (IsVowel(CharAt(input, index + 1)))
                    {
                        AddCodes(primary, alternate, "A", "F");
                        index += 2;
                    }
                    else
                    {
                        index++;
                    }
                    break;

                case 'X':
                    AddCodes(primary, alternate, "KS", "KS");
                    index += StartsWith(input, index, "XX") ? 2 : 1;
                    break;

                case 'Z':
                    AddCodes(primary, alternate, "S", "S");
                    index += CharAt(input, index + 1) == 'Z' ? 2 : 1;
                    break;

                default:
                    index++;
                    break;
            }
        }

        var p = primary.Length > 4 ? primary.ToString(0, 4) : primary.ToString();
        var a = alternate.Length > 4 ? alternate.ToString(0, 4) : alternate.ToString();
        return (p, a);
    }

    private static int HandleC(string input, int index, int length, System.Text.StringBuilder primary, System.Text.StringBuilder alternate)
    {
        if (StartsWith(input, index, "CH"))
        {
            AddCodes(primary, alternate, "X", "X");
            return index + 2;
        }

        if (StartsWith(input, index, "CI", "CE", "CY"))
        {
            AddCodes(primary, alternate, "S", "S");
            return index + 2;
        }

        if (StartsWith(input, index, "CK", "CC"))
        {
            AddCodes(primary, alternate, "K", "K");
            return index + 2;
        }

        AddCodes(primary, alternate, "K", "K");
        return index + 1;
    }

    private static int HandleG(string input, int index, int length, System.Text.StringBuilder primary, System.Text.StringBuilder alternate)
    {
        if (CharAt(input, index + 1) == 'H')
        {
            if (index > 0 && !IsVowel(CharAt(input, index - 1)))
            {
                AddCodes(primary, alternate, "K", "K");
                return index + 2;
            }

            if (index == 0)
            {
                AddCodes(primary, alternate, "J", "J");
                return index + 2;
            }

            AddCodes(primary, alternate, "K", "K");
            return index + 2;
        }

        if (CharAt(input, index + 1) == 'N')
        {
            if (index == 0)
            {
                AddCodes(primary, alternate, "KN", "N");
                return index + 2;
            }

            AddCodes(primary, alternate, "KN", "KN");
            return index + 2;
        }

        if (StartsWith(input, index, "GI", "GE", "GY"))
        {
            AddCodes(primary, alternate, "J", "K");
            return index + 2;
        }

        AddCodes(primary, alternate, "K", "K");
        return index + (CharAt(input, index + 1) == 'G' ? 2 : 1);
    }

    private static int HandleS(string input, int index, int length, System.Text.StringBuilder primary, System.Text.StringBuilder alternate)
    {
        if (StartsWith(input, index, "SH"))
        {
            AddCodes(primary, alternate, "X", "X");
            return index + 2;
        }

        if (StartsWith(input, index, "SI") && (CharAt(input, index + 2) == 'O' || CharAt(input, index + 2) == 'A'))
        {
            AddCodes(primary, alternate, "S", "X");
            return index + 3;
        }

        if (StartsWith(input, index, "SC"))
        {
            if (Contains("IEY", CharAt(input, index + 2)))
            {
                AddCodes(primary, alternate, "S", "S");
                return index + 3;
            }

            AddCodes(primary, alternate, "SK", "SK");
            return index + 3;
        }

        AddCodes(primary, alternate, "S", "S");
        return index + (StartsWith(input, index, "SS", "SZ") ? 2 : 1);
    }

    private static int HandleT(string input, int index, int length, System.Text.StringBuilder primary, System.Text.StringBuilder alternate)
    {
        if (StartsWith(input, index, "TH") || StartsWith(input, index, "TTH"))
        {
            AddCodes(primary, alternate, "0", "T");
            return index + (StartsWith(input, index, "TTH") ? 3 : 2);
        }

        if (StartsWith(input, index, "TION", "TIA", "TCH"))
        {
            AddCodes(primary, alternate, "X", "X");
            return index + (StartsWith(input, index, "TCH") ? 3 : 4);
        }

        AddCodes(primary, alternate, "T", "T");
        return index + (StartsWith(input, index, "TT", "TD") ? 2 : 1);
    }

    private static void AddCodes(System.Text.StringBuilder primary, System.Text.StringBuilder alternate, string p, string a)
    {
        if (primary.Length < 4)
            primary.Append(p);
        if (alternate.Length < 4)
            alternate.Append(a);
    }

    private static char CharAt(string s, int index)
    {
        return index >= 0 && index < s.Length ? s[index] : '\0';
    }

    private static bool IsVowel(char c)
    {
        return c is 'A' or 'E' or 'I' or 'O' or 'U';
    }

    private static bool Contains(string chars, char c)
    {
        return c != '\0' && chars.Contains(c);
    }

    private static bool StartsWith(string input, int index, params string[] prefixes)
    {
        foreach (var prefix in prefixes)
        {
            if (index + prefix.Length <= input.Length &&
                input.AsSpan(index, prefix.Length).SequenceEqual(prefix.AsSpan()))
                return true;
        }
        return false;
    }
}
