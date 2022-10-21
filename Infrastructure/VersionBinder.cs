using CompareVersions.Models;
using Version = CompareVersions.Models.Version;

namespace CompareVersions.Infrastructure;


/// <summary>
///     Class that assists with parsing binding options from the commandline
/// </summary>
/// <seealso cref="Version" />
public class VersionBinder : BinderBase<Version>
{
    private readonly Option<Segment> _major;
    private readonly Option<Segment> _minor;
    private readonly Option<Segment> _patch;
    private readonly Option<Segment> _build;

    private readonly Option<Version> _version;

    private readonly Option<IList<Version>> _versions;

    /// <summary>Initializes a new instance of the <see cref="VersionBinder" /> class.</summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="patch">The patch.</param>
    /// <param name="build">The build.</param>
    public VersionBinder(Option<Segment> major, Option<Segment> minor, Option<Segment> patch, Option<Segment> build)
    {
        this._major = major;
        this._minor = minor;
        this._patch = patch;
        this._build = build;
    }


    /// <summary>Initializes a new instance of the <see cref="VersionBinder" /> class.</summary>
    /// <param name="version">The version.</param>
    public VersionBinder(Option<Version> version)
    {
        this._version = version;
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="VersionBinder" /> class.
    /// </summary>
    /// <param name="versions">
    /// The versions.
    /// </param>
    public VersionBinder(Option<IList<Version>> versions)
    {
        this._versions = versions;
    }

    /// <summary>Initializes a new instance of the <see cref="VersionBinder" /> class.</summary>
    /// <param name="versionStrings">The version strings.</param>
    public VersionBinder(Option<IList<string>> versionStrings)
    {
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
            MajorSegment = bindingContext.ParseResult.GetValueForOption(_major),
            MinorSegment = bindingContext.ParseResult.GetValueForOption(_minor),
            PatchSegment = bindingContext.ParseResult.GetValueForOption(_patch),
            BuildSegment = bindingContext.ParseResult.GetValueForOption(_build)

        };
}
