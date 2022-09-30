using Version = CompareVersions.Models.Version;

namespace CompareVersions.Infrastructure;

/// <summary>
///     Represents command line options
/// </summary>
/// <seealso cref="CompareVersions.Infrastructure.ICommandLine" />
public class CommandLineOptions : ICommandLine
{
    private readonly char _separator = Constants.VersionSeparators[0];
    private readonly IMessageWriter _console;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineOptions"/> class.
    /// </summary>
    /// <param name="console">The console.</param>
    public CommandLineOptions(IMessageWriter console)
    {
        this._console = console;
    }

    /// <summary>
    /// Gets the output file option.
    /// </summary>
    /// <value>
    /// The output file option.
    /// </value>
    public Option<FileInfo> OutputFileOption => new(
        name: "--output-file",
        getDefaultValue: () => new FileInfo("output.txt"),
        description: "The output file to write to");

    /// <summary>
    /// Gets the verbosity option.
    /// </summary>
    /// <value>
    /// The verbosity option.
    /// </value>
    public Option<Verbosity> VerbosityOption => new(
        aliases: new[] { "--verbosity", "-v" },
        description: "The verbosity of the output",
        getDefaultValue: () => Verbosity.Quiet);

    /// <summary>
    /// Gets the versions supplied on the command line.
    /// </summary>
    /// <value>
    /// The version1.
    /// </value>
    public Option<IList<Version>> VersionsOption => new(
        aliases: new[] { "--version-strings", "-vs" },
        parseArgument: result =>
        {
            var versions = new List<Version>();

            if (result.Tokens.Count == 0)
            {
                versions.Add(Version.CreateRandom(_separator));
                versions.Add(Version.CreateRandom(_separator));
            }
            else if (int.TryParse(result.Tokens.Single().Value, out int count))
            {
                if (count > 2) result.ErrorMessage = $"Argument count cannot be more than 2. {count} were supplied.";
            }
            else
            {
                versions.Add(Version.Default);
                versions.Add(Version.Default);
            }

            return versions;
        },
        isDefault: true,
        description: "The version strings to compare. "

        )
    {
        AllowMultipleArgumentsPerToken = true,
        Name = "versions",
        Arity = ArgumentArity.ZeroOrOne,
        Description = "The version strings to compare. "
    };

    public Option<Version> VersionStringOption => new(
        aliases: new[] { "--version", "-v" },
        parseArgument: result =>
        {
            result.OnlyTake(2);

            var version1 = new Version(result.Tokens[0].Value, _separator);
            return version1;
        })
    {
        AllowMultipleArgumentsPerToken = true,
        Arity = ArgumentArity.ZeroOrMore
    };

    /// <summary>
    /// The use equal versions
    /// </summary>
    public Argument<bool> UseEqualVersions = new(
        name: "equals",
        description: "Use equal version strings. ",
        getDefaultValue: () => true);
    /// <summary>
    /// The use default value
    /// </summary>
    public Argument<bool> UseDefaultVersion = new(
        name: "default",
        description: "Provide default values. ",
        getDefaultValue: () => true);
}
/// <summary>
/// Verbosity level for console output and logging
/// </summary>
public enum Verbosity
{
    Quiet,
    Normal,
    Detailed
}
