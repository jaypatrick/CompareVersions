using Version = CompareVersions.Models.Version;

namespace CompareVersions;

/// <summary>
///  Sets up object container and service lifetimes
/// </summary>
/// <typeparam name="T"></typeparam>
public class Application<T>
    where T : Version
{
    /// <summary>
    /// Gets the comparison operations.
    /// </summary>
    /// <value>
    /// The comparison operations.
    /// </value>
    public required IComparisonOperations<T> ComparisonOperations { get; init; }

    /// <summary>
    /// Gets or sets the service provider.
    /// </summary>
    /// <value>
    /// The service provider.
    /// </value>
    public required IServiceProvider ServiceProvider { get; init; }

    /// <summary>
    /// Gets or sets the message writers.
    /// </summary>
    /// <value>
    /// The message writers.
    /// </value>
    public required IEnumerable<IMessageWriter> MessageWriters { get; set; }

    /// <summary>
    /// Gets the command line.
    /// </summary>
    /// <value>
    /// The command line.
    /// </value>
    public required ICommandLine CommandLine { get; init; }

    /// <summary>
    /// Gets the active console.
    /// </summary>
    /// <value>
    /// The active console.
    /// </value>
    public IMessageWriter ConsoleWriter { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Application{T}"/> class.
    /// </summary>
    /// <param name="comparisonOperations">The comparison operations.</param>
    /// <param name="messageWriters">The message writers.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="commandLine"></param>
    /// <param name="consoleWriter"></param>
    /// <exception cref="ArgumentNullException">
    /// nameof(comparisonOperations)
    /// or
    /// nameof(serviceProvider)
    /// </exception>
    [SetsRequiredMembers]
    public Application(IComparisonOperations<T> comparisonOperations,
        IEnumerable<IMessageWriter> messageWriters,
        IServiceProvider serviceProvider,
        ICommandLine commandLine,
        IMessageWriter consoleWriter)
    {
        this.ComparisonOperations = comparisonOperations
            ?? throw new ArgumentNullException(nameof(comparisonOperations));
        this.ServiceProvider = serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.MessageWriters = messageWriters
            ?? throw new ArgumentNullException(nameof(messageWriters));
        this.CommandLine = commandLine
            ?? throw new ArgumentNullException(nameof(commandLine));
        this.ConsoleWriter = consoleWriter
            ?? throw new ArgumentNullException(nameof(consoleWriter));
    }

    /// <summary>
    /// Runs the asynchronous.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>A <see cref="System.Int64"/>representing the result of the compare operation. -1 means the left side was greater, 0 means they are equal, and 1 means the right side is larger</returns>
    public async Task<int> RunAsync(IList<string> args)
    {
        // standard console, TODO: need to also set up logging writer
        var writer = this.ConsoleWriter;
        char separator = Constants.VersionSeparators[0];

        // TODO: Finish setting up the command line options here
        var rootCommand = new RootCommand("This is the root command. ")
        {
            Description = "A simple app to compare version strings.",
            //rootCommand.AddOption(this.CommandLine.VersionsOption);

            // TODO: Finish setting up binder and validation
            //rootCommand.SetHandler((versionOptions) =>
            //    versionOptions.Console = this.ConsoleWriter
            //);

            Handler = CommandHandler.Create<IMessageWriter>((writer) => HandleVersions(writer, args))
        }; //.WithHandler(nameof(HandleVersions));

        var result = await rootCommand.InvokeAsync(args.ToArray(), writer);

        writer.Write("Have another go? y/n");
        string again = writer.ReadLine() ?? "n";

        if (again != "N" || again != "n")
        {
            writer.Write("Firing it up...");
            result = await this.RunAsync(args);
        }
        else
        {
            Environment.Exit(result);
        }

        //Console.ReadLine();
        writer.ReadLine();
        return result;
    }

    /// <summary>
    /// Handles the versions.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="args">The arguments.</param>
    /// <returns></returns>
    public void HandleVersions(IMessageWriter writer, IList<string> args)
    {
        char separator = Constants.VersionSeparators[0];

        string version1 = args.Count > 0 ? args[0] : string.Empty;
        string version2 = args.Count > 0 ? args[1] : string.Empty;

        string inputVersion = "Input version string in <xx>.<xx>.<xx>.<xx> format";

        writer.Write($"Hello World from writer of type {writer.GetType()}");
        writer.Write("Welcome to CompareVersions. Would you like random version strings? y/n ");

        string createRandomVersion = writer.ReadLine() ?? "n";

        if (createRandomVersion == "y" || createRandomVersion == "Y")
        {
            writer.Write("Would you like to use equal version strings? y/n ");
            string useEqual = Console.ReadLine() ?? "n";

            if (useEqual == "y" || useEqual == "Y")
            {
                version1 = version2 = Version.CreateRandom(separator).ToString();
            }
            else
            {
                version1 = Version.CreateRandom(separator).ToString();
                version2 = Version.CreateRandom(separator).ToString();
            }

            // TODO: Work on the TryFormat methods
            // TODO: Work on TryParse methods
        }
        else
        {
            writer.Write(inputVersion);
            version1 = writer.ReadLine() ?? Version.CreateRandom(separator).ToString();
            // version1 = Console.ReadLine() ?? Version.CreateRandom(_separator).ToString();

            writer.Write(inputVersion);
            version2 = writer.ReadLine() ?? Version.CreateRandom(separator).ToString();
            // version2 = Console.ReadLine() ?? Version.CreateRandom(_separator).ToString();
        }

        writer.Write($"The version strings are {version1} and {version2}");

        //var result = comparisonOperations.CompareVersions(version1, version2);
        var result = this.ComparisonOperations.Compare(version1, version2);
        writeToConsole_1();

        void writeToConsole_1()
        {
            string isLessThan = Constants.IsLessThan;
            string isGreaterThan = Constants.IsGreaterThan;
            string isEqualTo = Constants.IsEqualTo;
            string theResultWas = Constants.TheResultWas;

            if (result < 0) writer.Write($"{version1} {isLessThan} {version2}: {theResultWas} {result}");
            if (result == 0) writer.Write($"{version1} {isEqualTo} {version2}: {theResultWas} {result}");
            if (result > 0) writer.Write($"{version1} {isGreaterThan} {version2}: {theResultWas} {result}");
        }
    }

    // THIS CONTAINS THE COMMENTED OUT MAIN METHOD OLD VERSION
    // private void oldMainMethod()
    // {
    //string version1 = args.Count > 0 ? args[0] : string.Empty;
    //string version2 = args.Count > 0 ? args[1] : string.Empty;

    //string inputVersion = "Input version string in <xx>.<xx>.<xx>.<xx> format";

    //writer.Write($"Hello World from writer of type {writer.GetType()}");
    //writer.Write("Welcome to CompareVersions. Would you like random version strings? y/n ");

    //string createRandomVersion = writer.ReadLine() ?? "n";

    //if (createRandomVersion == "y" || createRandomVersion == "Y")
    //{
    //    writer.Write("Would you like to use equal version strings? y/n ");
    //    string useEqual = Console.ReadLine() ?? "n";

    //    if (useEqual == "y" || useEqual == "Y")
    //    {
    //        version1 = version2 = Version.CreateRandom(separator).ToString();
    //    }
    //    else
    //    {
    //        version1 = Version.CreateRandom(separator).ToString();
    //        version2 = Version.CreateRandom(separator).ToString();
    //    }

    //    // TODO: Work on the TryFormat methods
    //    // TODO: Work on TryParse methods
    //}
    //else
    //{
    //    writer.Write(inputVersion);
    //    version1 = writer.ReadLine() ?? Version.CreateRandom(separator).ToString();
    //    // version1 = Console.ReadLine() ?? Version.CreateRandom(_separator).ToString();

    //    writer.Write(inputVersion);
    //    version2 = writer.ReadLine() ?? Version.CreateRandom(separator).ToString();
    //    // version2 = Console.ReadLine() ?? Version.CreateRandom(_separator).ToString();
    //}

    //writer.Write($"The version strings are {version1} and {version2}");

    ////var result = comparisonOperations.CompareVersions(version1, version2);
    //var result = this.ComparisonOperations.Compare(version1, version2);
    //writeToConsole_1();

    //void writeToConsole_1()
    //{
    //    string isLessThan = Constants.IsLessThan;
    //    string isGreaterThan = Constants.IsGreaterThan;
    //    string isEqualTo = Constants.IsEqualTo;
    //    string theResultWas = Constants.TheResultWas;

    //    if (result < 0) writer.Write($"{version1} {isLessThan} {version2}: {theResultWas} {result}");
    //    if (result == 0) writer.Write($"{version1} {isEqualTo} {version2}: {theResultWas} {result}");
    //    if (result > 0) writer.Write($"{version1} {isGreaterThan} {version2}: {theResultWas} {result}");
    //}

    //writer.Write("Have another go? y/n");
    ////string again = Console.ReadLine() ?? "n";
    //string again = writer.ReadLine() ?? "n";

    //if (again != "N" || again != "n")
    //{
    //    writer.Write("Firing it up...");
    //    await this.RunAsync(args);
    //}
    //else
    //{
    //    Environment.Exit(result);
    //}

    ////Console.ReadLine();
    //writer.ReadLine();
    //return result;
    // }
}