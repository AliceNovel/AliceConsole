using System.Text.RegularExpressions;

namespace AnovSyntax;

public class Anov
{
    public static void Read(string str)
    {
        Match match;
        // "- "から始まる"人物"を読み込み
        match = Regex.Match(str, @"- (.*)");
        if (match.Success)
            Console.Write(match.Groups[1].Value);

        // "["と"]"で囲む"会話"を読み込み
        match = Regex.Match(str, @"\[(.*?)\]");
        if (match.Success)
            Console.WriteLine("「" + match.Groups[1].Value + "」");
    }
}
