using Version = CompareVersions.Models.Version;

namespace CompareVersions.Infrastructure;

/// <summary>
///     Interface that shapes the command line arguments to be processed
/// </summary>
public interface ICommandLine
{
    /// <summary>
    /// Gets the output file option.
    /// </summary>
    /// <value>
    /// The output file option.
    /// </value>
    public Option<FileInfo> OutputFileOption { get; }

    /// <summary>
    /// Gets the verbosity option.
    /// </summary>
    /// <value>
    /// The verbosity option.
    /// </value>
    public Option<Verbosity> VerbosityOption { get; }

    /// <summary>
    /// Gets the version1.
    /// </summary>
    /// <value>
    /// The version1.
    /// </value>
    public Option<IList<Version>> VersionsOption { get; }
}
