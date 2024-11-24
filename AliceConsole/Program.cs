using AnovSyntax;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Alice Console");
            return;
        }

        if (args[0] == "-h" || args[0] == "--help")
        {
            Console.WriteLine("# Read Anov Syntax #");
            Console.WriteLine("Usage: ./AliceConsole <path>");
            Console.WriteLine("Example: ./AliceConsole sample.anov");
            Console.WriteLine();
            Console.WriteLine("# Init anproj file #");
            Console.WriteLine("Usage: ./AliceConsole init");
            return;
        }

        if  (args[0] == "init")
        {
            string outputDirectoryName = @"AnprojTemplate";

            try
            {
                if (Directory.Exists(outputDirectoryName))
                {
                    Console.WriteLine("The path exists already. Please remove \"AnprojTemplate\" directory.");
                    return;
                }
                Directory.CreateDirectory(outputDirectoryName);

                // Create "image", "image/background", "story" and "movie" directories.
                Directory.CreateDirectory(Path.Combine(outputDirectoryName, "image"));
                Directory.CreateDirectory(Path.Combine(outputDirectoryName, "image", "background"));
                Directory.CreateDirectory(Path.Combine(outputDirectoryName, "story"));
                Directory.CreateDirectory(Path.Combine(outputDirectoryName, "movie"));

                // Create "story/main.anov" and "package.json" files.
                string contentsMainAnov = "- Alice" + Environment.NewLine
                                        + "[Welcome to Alice Novel!]" + Environment.NewLine;
                string contentsPackageJson = "{" + Environment.NewLine
                                           + "  \"game-name\": \"Test Game\"," + Environment.NewLine
                                           + "  \"first-read\": \"story/main.anov\"" + Environment.NewLine
                                           + "}";
                File.WriteAllText(Path.Combine(outputDirectoryName, "story", "main.anov"), contentsMainAnov);
                File.WriteAllText(Path.Combine(outputDirectoryName, "package.json"), contentsPackageJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

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
