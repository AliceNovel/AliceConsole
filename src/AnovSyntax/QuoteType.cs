using System.Globalization;

namespace AnovSyntax;

public class QuoteType
{
    public string StartQuote { get; }
    public string EndQuote { get; }

    /// <summary>
    /// Provides quote-type.
    /// </summary>
    /// <param name="startQuote">Start Quote</param>
    /// <param name="endQuote">End Quote</param>

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
