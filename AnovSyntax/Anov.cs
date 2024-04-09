using System.Text.RegularExpressions;

namespace AnovSyntax;

public class Anov
{
    /// <summary>
    /// Read the text in anov syntax.
    /// </summary>
    /// <param name="str">The text in anov syntax</param>
    public static void Read(string str)
    {
        Match match;

        // Read "- people-name / emotion"
        match = Regex.Match(str, @"- (.*?)/");
        if (match.Success)
            Console.Write(match.Groups[1].Value.Trim());
        else
        {
            // Read "- people-name"
            match = Regex.Match(str, @"- (.*)");
            if (match.Success)
                Console.Write(match.Groups[1].Value.Trim());
        }

        // Read "/ emotion"
        match = Regex.Match(str, @"/ (.*)");
        if (match.Success)
            Console.Write(" (" + match.Groups[1].Value.Trim() + ")");

        // Read "[conversatioc-content]"
        match = Regex.Match(str, @"\[(.*?)\]");
        if (match.Success)
            Console.WriteLine(" \"" + match.Groups[1].Value.Trim() + "\"");
    }
}
