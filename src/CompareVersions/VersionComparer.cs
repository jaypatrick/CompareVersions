using Version = CompareVersions.Models.Version;

namespace CompareVersions.UI;

/// <summary>
/// Class containing operations that compare version strings
/// </summary>
public class VersionComparer<TVersion> : IComparisonOperations<TVersion>
    where TVersion : Version
{
    private readonly char _separator = Constants.VersionSeparators[0];

    /// <summary>
    /// Compares the specified left side version.
    /// </summary>
    /// <param name="leftSideVersion">The left side version.</param>
    /// <param name="rightSideVersion">The right side version.</param>
    /// <returns></returns>
    public int Compare([AllowNull] string leftSideVersion, [AllowNull] string rightSideVersion)
    {
        if (leftSideVersion == null) throw new ArgumentNullException(nameof(leftSideVersion));
        if (rightSideVersion == null) throw new ArgumentNullException(nameof(rightSideVersion));

        return CompareVersions(leftSideVersion, rightSideVersion);
    }

    /// <summary>
    /// Compares the specified left side version.
    /// </summary>
    /// <param name="leftSideVersion">The left side version.</param>
    /// <param name="rightSideVersion">The right side version.</param>
    /// <returns></returns>
    public int Compare([AllowNull] TVersion leftSideVersion, [AllowNull] TVersion rightSideVersion)
    {
        if (leftSideVersion == null) return -1;
        return rightSideVersion == null ? 1 : leftSideVersion.CompareTo(rightSideVersion);
    }

    /// <summary>
    /// Compares the specified left side version.
    /// </summary>
    /// <param name="leftSideVersion">The left side version.</param>
    /// <param name="rightSideVersion">The right side version.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">
    /// leftSideVersion - Argument must be of type 'Version'.
    /// or
    /// rightSideVersion - Argument must be of type 'Version'.
    /// </exception>
    public int Compare([AllowNull] object leftSideVersion, [AllowNull] object rightSideVersion)
    {
        if (leftSideVersion == null) return -1;
        if (rightSideVersion == null) return 1;

        Version version1 = leftSideVersion as Version;
        if (version1 == null)
        {
            throw new ArgumentException("Argument must be of type 'Version'.", nameof(leftSideVersion));
        }

        Version version2 = rightSideVersion as Version;
        if (version2 == null)
        {
            throw new ArgumentException("Argument must be of type 'Version'.", nameof(rightSideVersion));
        }

        return version1.CompareTo(version2);
    }

    /// <summary>Compares the versions.</summary>
    /// <param name="leftSideVersion">The left side version.</param>
    /// <param name="rightSideVersion">The right side version.</param>
    /// <returns>
    ///   An integer representing the result of the comparison.
    /// </returns>
    public int CompareVersions(string leftSideVersion, string rightSideVersion)
    {

        // given rightSideVersion version numbers e.g. 2.5.1 and 2.4.1.6,
        // compare them, return 0 if equal, return -1 if a > b, and 1 if a < b

        // Insert your code here
        // major.minor.patch.build
        // assume any quad can be < 100 (so what happens when quad > 100?
        // create a custom type?
        // 2 == 2.0 == 2.0.0 == 2.0.0.0
        // return -1 when version1 < version2

        // return 0 when version1 == version2

        // return 1 when version1 > version2

        var leftSideSegments = new Version(leftSideVersion, _separator);
        var rightSideSegments = new Version(rightSideVersion, _separator);

        return Compare(leftSideSegments, rightSideSegments);
    }

    /// <summary>
    /// Compares the versions.
    /// </summary>
    /// <param name="leftSideVersion">The left side version.</param>
    /// <param name="rightSideVersion">The right side version.</param>
    /// <returns></returns>
    public int CompareVersions(TVersion leftSideVersion, TVersion rightSideVersion)
    {
        return Compare(leftSideVersion, rightSideVersion);
    }

    /// <summary>
    /// Equals the specified left side.
    /// </summary>
    /// <param name="leftSideVersion"></param>
    /// <param name="rightSideVersion"></param>
    /// <returns></returns>
    public bool Equals(TVersion? leftSideVersion, TVersion? rightSideVersion) => leftSideVersion.Equals(rightSideVersion);

    /// <summary>
    /// Equals the specified left side version.
    /// </summary>
    /// <param name="leftSideVersion">The left side version.</param>
    /// <param name="rightSideVersion">The right side version.</param>
    /// <returns></returns>
    public new bool Equals(object? leftSideVersion, object? rightSideVersion)
    {
        if (leftSideVersion is not Version version1)
        {
            return false;
        }
        if (rightSideVersion is not Version version2)
        {
            return false;
        }

        return version1.Equals(version2);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">obj</exception>
    public int GetHashCode([DisallowNull] TVersion obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return obj.GetHashCode();
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">obj</exception>
    public int GetHashCode(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return obj.GetHashCode();
    }
}