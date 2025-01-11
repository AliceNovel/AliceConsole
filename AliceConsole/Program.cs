using AnovSyntax;
using System.IO.Compression;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace AliceConsole;

internal class Program
{
    static readonly JsonSerializerOptions jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // for Japanese Encoding
        WriteIndented = true, // for Indent
    };

    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("");
            Console.WriteLine("Usage: ./AliceConsole [options]");
            Console.WriteLine("Usage: ./AliceConsole [path-to-anov-file]");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("  -h|--help         Display help.");
            Console.WriteLine("  -v|--version      Display the version of your Alice Console.");
            Console.WriteLine("");
            Console.WriteLine("path-to-anov-file:");
            Console.WriteLine("  The path to an .anov file to execute.");
            return 1;
        }

        if (args[0] == "-h" || args[0] == "--help")
        {
            Console.WriteLine("Usage: ./AliceConsole [path-to-anov-file]");
            Console.WriteLine("");
            Console.WriteLine("Run Anov Syntax on Alice Console.");
            Console.WriteLine("");
            Console.WriteLine("path-to-anov-file:");
            Console.WriteLine("  The path to an .anov file to execute.");
            Console.WriteLine("");

            Console.WriteLine("Usage: ./AliceNovel [sdk-options] [command]");
            Console.WriteLine("");
            Console.WriteLine("Run Anov Syntax on Alive Console.");
            Console.WriteLine("");
            Console.WriteLine("sdk-options:");
            Console.WriteLine("  -h|--help         Display help.");
            Console.WriteLine("  -v|--version      Display the version of your Alice Console.");
            Console.WriteLine("");
            Console.WriteLine("SDK command:");
            Console.WriteLine("  init              Construct template files and directories for Alice Novel.");
            Console.WriteLine("  pack <directory>  Pack the directory into an .anproj file.");
            Console.WriteLine("");
            return 0;
        }

        if (args[0] == "-v" || args[0] == "--version")
        {
            var versionString = Assembly.GetEntryAssembly()?
                                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                        .InformationalVersion
                                        .ToString();

            Console.WriteLine($"Alice Console v{versionString}");
            return 0;
        }

        if (args[0] == "init")
        {
            Console.WriteLine("This utility will walk you through creating a package.json file.");
            Console.WriteLine("It only covers the most common items, and tries to guess sensible defaults.");
            Console.WriteLine("");
            Console.WriteLine("Press ^C at any time to quit.");
            
            Console.Write("package name: (anproj-template) ");
            string? packageName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(packageName))
                packageName = "anproj-template";

            Console.Write("version: (1.0.0) ");
            string? version = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(version))
                version = "1.0.0";

            Console.Write("description: ");
            string description = Console.ReadLine() ?? "";

            Console.Write("entry point: (story/main.anov) ");
            string? entryPoint = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(entryPoint))
                entryPoint = "story/main.anov";

            // Console.Write("git repository: ");
            // string gitRepository = Console.ReadLine() ?? "";

            // Console.Write("keywords: ");
            // string keywords = Console.ReadLine() ?? "";

            Console.Write("author: ");
            string author = Console.ReadLine() ?? "";

            Console.Write("license: ");
            string license = Console.ReadLine() ?? "";

            string outputDirectoryName = Regex.Replace(packageName, @"\s", "");
            Console.WriteLine($"About to write to ./{outputDirectoryName}/package.json:" + Environment.NewLine);

            Dictionary<string, string> dictPackageJson = new()
            {
                { "game-name", packageName },
                // { "name", packageName },
                { "version", version },
                { "first-read", entryPoint },
                // { "main", entryPoint },
                // { "repository", $"{{ \"type\": \"git\", \"url\": \"{gitRepository}\" }}" },
                // { "keywords", keywords },
                { "author", author },
                { "license", license },
                { "description", description }
            };
            string contentsPackageJson = JsonSerializer.Serialize(dictPackageJson, jsonOptions) + Environment.NewLine;

            Console.WriteLine(contentsPackageJson);

            Console.WriteLine("Is this OK? (yes) ");
            string? answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer))
                answer = "yes";
            if (answer != "yes" && answer != "y")
            {
                Console.WriteLine("Aborted.");
                return 0;
            }

            try
            {
                if (Directory.Exists(outputDirectoryName))
                {
                    Console.WriteLine($"The path exists already. Please remove \"{outputDirectoryName}\" directory.");
                    return 1;
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

                if (entryPoint == "story/main.anov")
                    File.WriteAllText(Path.Combine(outputDirectoryName, "story", "main.anov"), contentsMainAnov);
                else
                    File.WriteAllText(Path.Combine(outputDirectoryName, "main.anov"), contentsMainAnov);
                File.WriteAllText(Path.Combine(outputDirectoryName, "package.json"), contentsPackageJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 1;
            }

            return 0;
        }

        if (args[0] == "pack")
        {
            string directoryPath = args[1];

            // Remove the last slash.
            directoryPath = directoryPath.Trim('/');

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Error: Directory does not exist.");
                Console.WriteLine("Hint: Please check that the name is correct.");
                return 1;
            }

            ZipFile.CreateFromDirectory(directoryPath, $"./{directoryPath}.anproj");

            return 0;
        }

        // input test that is written in Anov Syntax.
        string filePath = args[0];

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Error: File does not exist.");
            Console.WriteLine("Hint: Please check that the name is correct.");
            return 1;
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
        return 0;
    }
}
