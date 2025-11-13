using System.Text;

namespace Infrastructure.Levels;

internal static class LevelCategoryParser
{
    internal static readonly string[] separator = new[] { "\r\n", "\n", "\r" };

    /// <summary>
    /// Parse a semicolon-delimited CSV (with quotes and "" escapes) where:
    /// - Row 0 is the header (categories)
    /// - Subsequent rows contain words for those categories
    /// Returns a map: Category -> List of words (down the column).
    /// </summary>
    public static Dictionary<string, List<string>> ParseColumns(string csvText)
    {
        var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(csvText))
            return result;

        var lines = csvText.Split(separator, StringSplitOptions.None);
        if (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]))
            return result;

        // Header
        var headers = SplitSemicolonCsv(lines[0]);
        if (headers.Count == 0)
            return result;

        // Initialize buckets for each category
        foreach (var h in headers)
        {
            var key = h.Trim();
            if (key.Length == 0)
                throw new FormatException("Empty header/category name is not allowed.");
            if (!result.ContainsKey(key))
                result[key] = new List<string>();
            else
                // If duplicate header names exist, we still aggregate into the same bucket
                _ = 0;
        }

        // Data rows
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var cols = SplitSemicolonCsv(line);

            if (cols.Count != headers.Count)
                throw new FormatException(
                    $"Row {i + 1} has {cols.Count} columns but header has {headers.Count}.");

            for (int c = 0; c < headers.Count; c++)
            {
                var value = cols[c].Trim();
                if (value.Length == 0)
                    continue; // skip blanks

                var header = headers[c].Trim();
                result[header].Add(value);
            }
        }

        return result;
    }

    /// <summary>
    /// Helper to get words for a category (case-insensitive).
    /// Returns empty list if the category doesn't exist or has no values.
    /// </summary>
    public static IReadOnlyList<string> GetWordsForCategory(
        Dictionary<string, List<string>> map, string category)
    {
        if (map.TryGetValue(category, out var list)) return list;
        // Try exact-trim match if not already
        var key = category.Trim();
        if (map.TryGetValue(key, out list)) return list;
        return Array.Empty<string>();
    }

    // --- CSV splitting (semicolon, with quotes & "" escapes) ---

    private static List<string> SplitSemicolonCsv(string input)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < input.Length; i++)
        {
            char ch = input[i];

            if (inQuotes)
            {
                if (ch == '"')
                {
                    // doubled quote => literal "
                    if (i + 1 < input.Length && input[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = false; // end quoted field
                    }
                }
                else
                {
                    current.Append(ch);
                }
            }
            else
            {
                if (ch == ';')
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else if (ch == '"')
                {
                    inQuotes = true;
                }
                else
                {
                    current.Append(ch);
                }
            }
        }

        result.Add(current.ToString());
        return result;
    }
}