using AnovSyntax;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
        {
            Console.WriteLine("Usage: ./AliceConsole <path>");
            Console.WriteLine("Example: ./AliceConsole sample.anov");
            return;
        }

        // input test that is written in Anov Syntax.
        string filePath = args[0];

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Error: File does not exist.");
            Console.WriteLine("Hint: Please check that the name is correct.");
            return;
        }

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
    }
}
