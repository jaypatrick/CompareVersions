namespace CompareVersions.Infrastructure;

/// <summary>
/// Constants for the app
/// </summary>
public static class Constants
{
    /// <summary>
    /// Gets the version separators.
    /// </summary>
    /// <value>
    /// The version separators.
    /// </value>
    public static ReadOnlySpan<char> VersionSeparators
    {
        get
        {
            return new(new[] { '.' });
        }
    }

    /// <summary>
    /// Gets the version segment floor.
    /// </summary>
    /// <value>
    /// The version segment floor.
    /// </value>
    public static int VersionSegmentFloor
    {
        get { return 0; }
    }

    /// <summary>
    /// Gets the version segment ceiling.
    /// </summary>
    /// <value>
    /// The version segment ceiling.
    /// </value>
    public static int VersionSegmentCeiling
    {
        get { return 100; }
    }

    public static int MaxNumberOfSegments
    {
        get { return 4; }
    }
}