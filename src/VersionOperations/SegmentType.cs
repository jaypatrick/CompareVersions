using CompareVersions.Models;

namespace CompareVersions.UI;

/// <summary>
///     Enum representing segments types for the <see cref="Version" /> object
/// </summary>
public enum SegmentType
{
    /// <summary>
    /// The major
    /// </summary>
    Major = 0,
    /// <summary>
    /// The minor
    /// </summary>
    Minor = 1,
    /// <summary>
    /// The patch
    /// </summary>
    Patch = 2,
    /// <summary>
    /// The build
    /// </summary>
    Build = 3
}