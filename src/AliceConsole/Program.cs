using AnovSyntax;
using System.CommandLine;
using System.IO.Compression;
using System.Text.Encodings.Web;
using System.Text.Json;
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
        RootCommand rootCommand = new("Alice Console - Create and run a story right there and then, just for you!");

        // Init command
        Command initCommand = new("init", "Construct template files and directories for Alice Novel.");
        initCommand.SetAction(parseResult =>
        {
            return Init();
        });
        rootCommand.Subcommands.Add(initCommand);

        // Packing command
        Command packCommand = new("pack", "Pack the directory into an .anproj file.");
        Argument<DirectoryInfo> PackDirectoryArgument = new("target-directory");
        packCommand.Arguments.Add(PackDirectoryArgument);
        packCommand.SetAction(parseResult =>
        {
            DirectoryInfo? packTargetDirectory = parseResult.CommandResult.GetRequiredValue(PackDirectoryArgument);
            return Pack(packTargetDirectory);
        });
        rootCommand.Subcommands.Add(packCommand);

        // Run command
        Command runCommand = new("run", "The path to an .anov file to execute.");
        Argument<FileInfo> DefaultFileArgument = new("target-file");
        runCommand.Arguments.Add(DefaultFileArgument);
        runCommand.SetAction(parseResult =>
        {
            FileInfo? fileInfo = parseResult.CommandResult.GetRequiredValue(DefaultFileArgument);
            return Run(fileInfo);
        });
        rootCommand.Subcommands.Add(runCommand);

        return rootCommand.Parse(args).Invoke();
    }

    static int Init()
    {
        Console.WriteLine("This utility will walk you through creating a package.json file.");
        Console.WriteLine("It only covers the most common items, and tries to guess sensible defaults.");
        Console.WriteLine("");
        Console.WriteLine("Press ^C at any time to quit.");

        Console.Write("package name: (anproj-template) ");
        string? input = Console.ReadLine();
        string packageName = string.IsNullOrWhiteSpace(input) ? "anproj-template" : input;

        Console.Write("version: (1.0.0) ");
        input = Console.ReadLine();
        string version = string.IsNullOrWhiteSpace(input) ? "1.0.0" : input;

        Console.Write("description: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("entry point: (story/main.anov) ");
        input = Console.ReadLine();
        string entryPoint = string.IsNullOrWhiteSpace(input) ? "story/main.anov" : input;

        // Console.Write("git repository: ");
        // string gitRepository = Console.ReadLine() ?? "";

        // Console.Write("keywords: ");
        // string keywords = Console.ReadLine() ?? "";

        Console.Write("author: ");
        string author = Console.ReadLine() ?? "";

        Console.Write("license: ");
        string license = Console.ReadLine() ?? "";

        string outputDirectoryName = packageName.Replace(@" ", ""); // Remove spaces.
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

    static int Pack(DirectoryInfo? dirInfo)
    {
        if (dirInfo is null)
            return 1;

        if (!dirInfo.Exists)
        {
            Console.WriteLine("Error: Directory does not exist.");
            Console.WriteLine("Hint: Please check that the name is correct.");
            return 1;
        }

        // Remove the last slash.
        string directoryPath = dirInfo.FullName.TrimEnd('/');

        ZipFile.CreateFromDirectory(directoryPath, $"{directoryPath}.anproj");
        return 0;
    }

    static int Run(FileInfo? fileInfo)
    {
        if (fileInfo is null)
            return 1;

        if (!fileInfo.Exists)
        {
            Console.WriteLine("Error: File does not exist.");
            Console.WriteLine("Hint: Please check that the name is correct.");
            return 1;
        }

        using (StreamReader sr = new(fileInfo.FullName))
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
