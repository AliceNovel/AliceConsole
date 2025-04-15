using System.Globalization;
using System.Text.RegularExpressions;

namespace AnovSyntax;

public class Anov
{
    /// <summary>
    /// Read the text in anov syntax.
    /// </summary>
    /// <param name="str">The text in anov syntax</param>
    /// <param name="quoteType">QuoteType in Conversations (Default = DoubleQuote)</param>
    public static string Read(string str, QuoteType? quoteType = null)
    {
        quoteType ??= QuoteType.DoubleQuote;

        Match match;
        string _return = "";

        // Read "[conversation-content]"
        match = Regex.Match(str, @"\[(.*?)\]");
        if (match.Success)
            return quoteType.StartQuote + match.Groups[1].Value.Trim() + quoteType.EndQuote;

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

    /// <summary>
    /// Quotation Type
    /// </summary>
    /// <param name="startQuote">Start Quotation</param>
    /// <param name="endQuote">End Quotation</param>

    public QuoteType(string startQuote, string endQuote)
    {
        StartQuote = startQuote;
        EndQuote = endQuote;
    }

    public string Wrap(string input) => $"{StartQuote}{input}{EndQuote}";

    /// <summary>Input</summary>
    public static readonly QuoteType None = new("", "");
    /// <summary>'Input'</summary>
    public static readonly QuoteType SingleQuote = new("\'", "\'");
    /// <summary>"Input"</summary>
    public static readonly QuoteType DoubleQuote = new("\"", "\""); // Default
    /// <summary>‘Input’</summary>
    public static readonly QuoteType NovelSingleQuote = new("‘", "’");
    /// <summary>“Input”</summary>
    public static readonly QuoteType NovelDoubleQuote = new("“", "”");
    /// <summary>‚Input‘</summary>
    public static readonly QuoteType ReversedNovelSingleQuote = new("‚", "‘");
    /// <summary>„Input“</summary>
    public static readonly QuoteType ReversedNovelDoubleQuote = new("„", "“");
    /// <summary>‚Input’</summary>
    public static readonly QuoteType ReversedNovelLowSingleQuote = new("‚", "’");
    /// <summary>„Input”</summary>
    public static readonly QuoteType ReversedNovelLowDoubleQuote = new("„", "”");
    /// <summary>「Input」</summary>
    public static readonly QuoteType CJKSingleQuote = new("「", "」");
    /// <summary>『Input』</summary>
    public static readonly QuoteType CJKDoubleQuote = new("『", "』");
    /// <summary>〈Input〉</summary>
    public static readonly QuoteType CJKSingleBracket = new("〈", "〉");
    /// <summary>《Input》</summary>
    public static readonly QuoteType CJKDoubleBracket = new("《", "》");
    /// <summary>‹Input›</summary>
    public static readonly QuoteType SingleGuillemets = new("‹", "›");
    /// <summary>«Input»</summary>
    public static readonly QuoteType DoubleGuillemets = new("«", "»");
    /// <summary>›Input‹</summary>
    public static readonly QuoteType ReversedSingleGuillemets = new("›", "‹");
    /// <summary>»Input«</summary>
    public static readonly QuoteType ReversedDoubleGuillemets = new("»", "«");
    /// <summary>〝Input〞</summary>
    public static readonly QuoteType JapaneseDoublePrime = new("〝", "〞");
    /// <summary>〝Input〟</summary>
    public static readonly QuoteType JapaneseLowDoublePrime = new("〝", "〟");
    /// <summary>― Input</summary>
    public static readonly QuoteType Dash = new("― ", "");
    /// <summary>_Input_</summary>
    public static readonly QuoteType UnderBar = new("_", "_");

    public static QuoteType GetQuoteForCulture(CultureInfo culture)
    {
        return culture.Name switch
        {
            "en-US" => NovelDoubleQuote,
            "en-GB" => NovelSingleQuote,
            "ja-JP" => CJKSingleQuote,
            "da-DK" => ReversedDoubleGuillemets,
            "fr-FR" => DoubleGuillemets,
            "de-DE" => ReversedNovelDoubleQuote,
            _ => DoubleQuote
        };
    }

    /// <summary>
    /// Converts the quote-type value of this instance to its equivalent string representation.
    /// </summary>
    /// <returns>String representation.</returns>
    public override string ToString() => $"QuoteType({StartQuote}, {EndQuote})";
}
