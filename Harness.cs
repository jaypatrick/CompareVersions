using Microsoft.Extensions.DependencyInjection;

namespace CompareVersions;

/// <summary>
///  Sets up object container and service lifetimes
/// </summary>
/// <typeparam name="T"></typeparam>
public class Harness<T> where T : UI.Version
{
    private readonly int floor = Constants.VersionSegmentFloor;
    private readonly int ceiling = Constants.VersionSegmentCeiling;

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
    public required IServiceProvider serviceProvider { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Harness{T}"/> class.
    /// </summary>
    /// <param name="comparisonOperations">The comparison operations.</param>
    /// <param name="messageWriters">The message writers.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <exception cref="ArgumentNullException">
    /// nameof(comparisonOperations)
    /// or
    /// nameof(serviceProvider)
    /// </exception>
    [SetsRequiredMembers]
    public Harness(IComparisonOperations<T> comparisonOperations,
        IEnumerable<IMessageWriter> messageWriters,
        IServiceProvider serviceProvider)
    {
        if (comparisonOperations is null)
        {
            throw new ArgumentNullException(nameof(comparisonOperations));
        }
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        this.ComparisonOperations = comparisonOperations;
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the asynchronous.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns></returns>
    public async Task RunAsync(IList<string> args)
    {
        var writer = this.serviceProvider.GetRequiredService<IMessageWriter>();

        writer.Write($"Hello World from writer of type {writer.GetType()}");

        writer.Write("Hello, World!");
        string version1 = args.Count > 0 ? args[0] : string.Empty;
        string version2 = args.Count > 0 ? args[1] : string.Empty;

        string inputVersion = "Input version string in <xx>.<xx>.<xx>.<xx> format";

        writer.Write("Welcome to CompareVersions. Would you like random version strings? y/n ");
        string createRandomVersion = Console.ReadLine();

        if (createRandomVersion == "y" || createRandomVersion == "Y")
        {
            writer.Write("Would you like to use equal version strings? y/n ");
            string useEqual = Console.ReadLine();

            if (useEqual == "y" || useEqual == "Y")
            {
                version1 = version2 = createRandomVersionString();
            }
            else
            {
                version1 = createRandomVersionString();
                version2 = createRandomVersionString();
            }

            // TODO: Work on the TryFormat methods
            // TODO: Work on TryParse methods
        }
        else
        {
            writer.Write(inputVersion);
            version1 = Console.ReadLine();

            writer.Write(inputVersion);
            version2 = Console.ReadLine();
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

        writer.Write("Have another go? y/n");
        string again = Console.ReadLine();

        if (again != "N" || again != "n")
        {
            writer.Write("Firing it up...");
            await this.RunAsync(args);
        }
        else
        {
            Environment.Exit(result);
        }

        Console.ReadLine();
    }

    /// <summary>
    /// Creates the random version string.
    /// </summary>
    /// <returns>A <see cref="String"/>value of the version.</returns>
    private string createRandomVersionString()
    {
        char separator = Constants.VersionSeparators[0];
        int maxSegments = Constants.MaxNumberOfSegments;
        StringBuilder versionString = new();

        for (int i = 1; i <= maxSegments; i++)
        {
            var quad = RandomNumberGenerator.GetInt32(floor, ceiling);

            versionString.Append(quad);
            versionString.Append(separator);
        }

        versionString.Length--;
        return versionString.ToString();
    }
}