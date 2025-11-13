using Domain.Levels;
using System.Globalization;
using System.Text;

namespace Infrastructure.Levels;
internal sealed class LevelParser
{
    internal static readonly string[] separator = new[] { "\r\n", "\n", "\r" };

    public static List<Level> Parse(string csvText)
    {
        var levels = new List<Level>();

        if (string.IsNullOrWhiteSpace(csvText))
            return levels;

        var lines = csvText.Split(
            separator,
            StringSplitOptions.None);

        if (lines.Length <= 1)
            return levels;

        // skip header (line 0)
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            levels.Add(ParseLine(line));
        }

        return levels;
    }

    public static Level ParseLine(string line)
    {
        ArgumentNullException.ThrowIfNull(line);

        var cols = SplitSemicolonCsv(line);

        if (cols.Count < 9)
            throw new FormatException($"Expected at least 9 columns (ignoring RemovalWord), got {cols.Count}.");

        // Date
        int year = int.Parse(cols[0], CultureInfo.InvariantCulture);
        int month = int.Parse(cols[1], CultureInfo.InvariantCulture);
        int day = int.Parse(cols[2], CultureInfo.InvariantCulture);
        var date = new DateTime(year, month, day);

        // Categories
        var categories = SplitToList(cols[3], '|');

        // Word columns → Word objects
        var words = new List<Word>();
        for (int i = 4; i <= 8; i++) // 5 "Word" columns
        {
            var coords = SplitToList(cols[i], ',');
            if (coords.Count > 0)
            {
                words.Add(new Word(coords, coords.Count));
            }
        }

        return new Level
        {
            Date = date,
            Categories = categories,
            Words = words
        };
    }

    // CSV splitting helpers
    private static List<string> SplitSemicolonCsv(string input)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < input.Length && input[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    current.Append(c);
                }
            }
            else
            {
                if (c == ';')
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else if (c == '"')
                {
                    inQuotes = true;
                }
                else
                {
                    current.Append(c);
                }
            }
        }

        result.Add(current.ToString());
        return result;
    }

    private static List<string> SplitToList(string s, char sep)
    {
        var list = new List<string>();
        if (string.IsNullOrEmpty(s)) return list;

        foreach (var part in s.Split(sep))
        {
            var trimmed = part.Trim();
            if (trimmed.Length > 0)
                list.Add(trimmed);
        }
        return list;
    }
}
