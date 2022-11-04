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
            return new ReadOnlySpan<char>(new[] { '.' });
        }
    }

    /// <summary>
    /// Gets the version segment floor.
    /// </summary>
    /// <value>
    /// The version segment floor.
    /// </value>
    public static int VersionSegmentFloor => 0;

    /// <summary>
    /// Gets the version segment ceiling.
    /// </summary>
    /// <value>
    /// The version segment ceiling.
    /// </value>
    public static int VersionSegmentCeiling => 100;

    /// <summary>
    /// Gets the maximum number of segments.
    /// </summary>
    /// <value>
    /// The maximum number of segments.
    /// </value>
    public static int MaxNumberOfSegments => 4;

    /// <summary>
    /// Gets the is less than.
    /// </summary>
    /// <value>
    /// The is less than.
    /// </value>
    public static string IsLessThan => "is less than";

    /// <summary>
    /// Gets the is greater than.
    /// </summary>
    /// <value>
    /// The is greater than.
    /// </value>
    public static string IsGreaterThan => "is greater than";

    /// <summary>
    /// Gets the is equal to.
    /// </summary>
    /// <value>
    /// The is equal to.
    /// </value>
    public static string IsEqualTo => "is equal to";

    /// <summary>
    /// Gets the result was.
    /// </summary>
    /// <value>
    /// The result was.
    /// </value>
    public static string TheResultWas => "The result was";
}