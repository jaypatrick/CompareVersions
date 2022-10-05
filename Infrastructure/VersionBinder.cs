using CompareVersions.Models;
using Version = CompareVersions.Models.Version;

namespace CompareVersions.Infrastructure;


/// <summary>
///     Class that assists with parsing binding options from the commandline
/// </summary>
/// <seealso cref="System.CommandLine.Binding.BinderBase{CompareVersions.Models.Version};" />
public class VersionBinder : BinderBase<Version>
{
    private readonly Option<Segment> major;
    private readonly Option<Segment> minor;
    private readonly Option<Segment> patch;
    private readonly Option<Segment> build;

    private readonly Option<Version> version;

    private readonly Option<IList<Version>> versions;
    private readonly Option<IList<string>> versionStrings;

    /// <summary>Initializes a new instance of the <see cref="VersionBinder" /> class.</summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="patch">The patch.</param>
    /// <param name="build">The build.</param>
    public VersionBinder(Option<Segment> major, Option<Segment> minor, Option<Segment> patch, Option<Segment> build)
    {
        this.major = major;
        this.minor = minor;
        this.patch = patch;
        this.build = build;
    }


    /// <summary>Initializes a new instance of the <see cref="VersionBinder" /> class.</summary>
    /// <param name="version">The version.</param>
    public VersionBinder(Option<Version> version)
    {
        this.version = version;
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="VersionBinder" /> class.
    /// </summary>
    /// <param name="versions">
    /// The versions.
    /// </param>
    public VersionBinder(Option<IList<Version>> versions)
    {
        this.versions = versions;
    }


    /// <summary>Initializes a new instance of the <see cref="VersionBinder" /> class.</summary>
    /// <param name="versionStrings">The version strings.</param>
    public VersionBinder(Option<IList<string>> versionStrings)
    {
        this.versionStrings = versionStrings;
    }


    /// <summary>
    /// Gets a value from the binding context.
    /// </summary>
    /// <param name="bindingContext"></param>
    /// <returns></returns>
    protected override Version GetBoundValue(BindingContext bindingContext) =>
        new()
        {
            // This is only needed if we parse out each SEGMENT from the commandline
            MajorSegment = bindingContext.ParseResult.GetValueForOption(major),
            MinorSegment = bindingContext.ParseResult.GetValueForOption(minor),
            PatchSegment = bindingContext.ParseResult.GetValueForOption(patch),
            BuildSegment = bindingContext.ParseResult.GetValueForOption(build)

        };
}
