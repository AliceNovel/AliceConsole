using AnovSyntax;

// input test that is written in Anov Syntax.
string filePath = @"sample.anov";

using (StreamReader sr = new(filePath))
{
    while (!sr.EndOfStream)
    {
        string? line = sr.ReadLine();
        if (line is not null)
            Anov.Read(line);
    }
}
