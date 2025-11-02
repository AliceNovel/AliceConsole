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
