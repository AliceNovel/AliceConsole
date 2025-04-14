using System.Globalization;
using System.Text.RegularExpressions;

namespace AnovSyntax;

public class Anov
{
    /// <summary>
    /// Read the text in anov syntax.
    /// </summary>
    /// <param name="str">The text in anov syntax</param>
    public static string Read(string str)
    {
        Match match;
        string _return = "";

        // Read "[conversation-content]"
        match = Regex.Match(str, @"\[(.*?)\]");
        if (match.Success)
            return " \"" + match.Groups[1].Value.Trim() + "\"";

        // Unsupported
        // Read "> place"
        match = Regex.Match(str, @"> (.*)");
        if (match.Success)
            _return += "/ " + match.Groups[1].Value.Trim() + " /";

        // Unsupported
        // Read "bgm: background-music"
        match = Regex.Match(str, @"bgm: (.*)");
        if (match.Success)
            _return += "<bgm>";

        // Unsupported
        // Read "movie: movie"
        match = Regex.Match(str, @"movie: (.*)");
        if (match.Success)
            _return += "<movie>";

        // Read "- people-name / emotion"
        match = Regex.Match(str, @"- (.*?)/");
        if (match.Success)
            _return += match.Groups[1].Value.Trim();
        else
        {
            // Read "- people-name"
            match = Regex.Match(str, @"- (.*)");
            if (match.Success)
                _return += match.Groups[1].Value.Trim();
        }

        // Read "/ emotion"
        match = Regex.Match(str, @"/ (.*)");
        if (match.Success)
            _return += " (" + match.Groups[1].Value.Trim() + ")";

        return _return;
    }
}

public class QuoteType
{
    public string StartQuote { get; }
    public string EndQuote { get; }

    public QuoteType(string startQuote, string endQuote)
    {
        StartQuote = startQuote;
        EndQuote = endQuote;
    }

    private static readonly Dictionary<string, QuoteType> _quotes = new()
    {
        { "None", new QuoteType("", "") },
        { "SingleQuote", new QuoteType("\'", "\'") },
        { "DoubleQuote", new QuoteType("\"", "\"") }, // Default
        { "NovelSingleQuote", new QuoteType("‘", "’") },
        { "NovelDoubleQuote", new QuoteType("“", "”") },
        { "ReversedNovelSingleQuote", new QuoteType("‚", "‘") },
        { "ReversedNovelDoubleQuote", new QuoteType("„", "“") },
        { "ReversedNovelLowSingleQuote", new QuoteType("‚", "’") },
        { "ReversedNovelLowDoubleQuote", new QuoteType("„", "”") },
        { "CJKSingleQuote", new QuoteType("「", "」") },
        { "CJKDoubleQuote", new QuoteType("『", "』") },
        { "CJKSingleBracket", new QuoteType("〈", "〉") },
        { "CJKDoubleBracket", new QuoteType("《", "》") },
        { "SingleGuillemets", new QuoteType("‹", "›") },
        { "DoubleGuillemets", new QuoteType("«", "»") },
        { "ReversedSingleGuillemets", new QuoteType("›", "‹") },
        { "ReversedDoubleGuillemets", new QuoteType("»", "«") },
        { "JapaneseDoublePrime", new QuoteType("〝", "〞") },
        { "JapaneseLowDoublePrime", new QuoteType("〝", "〟") },
        { "Dash", new QuoteType("― ", "") },
        { "UnderBar", new QuoteType("_", "_") },
    };

    public static QuoteType Get(string name)
    {
        if (_quotes.TryGetValue(name, out var pattern))
            return pattern;
        throw new KeyNotFoundException($"Pattern '{name}' not found.");
    }

    public static QuoteType GetQuoteForCulture(CultureInfo culture)
    {
        string cultureName = culture.Name;
        return cultureName switch
        {
            "en-US" => _quotes["NovelDoubleQuote"],
            "en-GB" => _quotes["NovelSingleQuote"],
            "ja-JP" => _quotes["CJKSingleQuote"],
            "da-DK" => _quotes["ReversedDoubleGuillemets"],
            "fr-FR" => _quotes["DoubleGuillemets"],
            "de-DE" => _quotes["ReversedNovelDoubleQuote"],
            _ => _quotes["DoubleQuote"]
        };
    }

    public static IEnumerable<QuoteType> Available => _quotes.Values;
}
