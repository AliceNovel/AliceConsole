using AnovSyntax;

// input test that is written in Anov Syntax.
string filePath = @"sample.anov";

using (StreamReader sr = new(filePath))
{
    while (!sr.EndOfStream)
    {
        string? line = sr.ReadLine();
        if (line == "")
            Console.ReadLine();
        else if (line is not null)
            Console.WriteLine(Anov.Read(line));
    }
    Console.ReadLine();
}
