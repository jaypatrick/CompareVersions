using CompareVersions.Interfaces;

namespace CompareVersions.UI;

/// <summary>
/// Class containing operations that compare version strings
/// </summary>
public class VersionComparer<TVersion> : IComparisonOperations<TVersion>
    where TVersion : Version
{
    private readonly char separator = Constants.VersionSeparators[0];

    public int Compare([AllowNull] string leftSideVersion, [AllowNull] string rightSideVersion)
    {
        return CompareVersions(leftSideVersion, rightSideVersion);
    }

    public int Compare([AllowNull] TVersion leftSideVersion, [AllowNull] TVersion rightSideVersion)
    {
        if (leftSideVersion == null) return -1;
        if (rightSideVersion == null) return 1;

        return leftSideVersion.CompareTo(rightSideVersion);
    }

    public int Compare([AllowNull] object leftSideVersion, [AllowNull] object rightSideVersion)
    {
        if (leftSideVersion == null) return -1;
        if (rightSideVersion == null) return 1;

        Version version1 = leftSideVersion as Version;
        if (version1 == null)
        {
            throw new ArgumentException(nameof(leftSideVersion), "Argument must be of type 'Version'.");
        }

        Version version2 = rightSideVersion as Version;
        if (version2 == null)
        {
            throw new ArgumentException(nameof(rightSideVersion), "Argument must be of type 'Version'.");
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

        // given rightSideVersion verison numbers e.g. 2.5.1 and 2.4.1.6,
        // compare them, return 0 if equal, return -1 if a > b, and 1 if a < b

        // Insert your code here
        // major.minor.patch.build
        // assume any quad can be < 100 (so what happens when quad > 100?
        // create a custom type?
        // 2 == 2.0 == 2.0.0 == 2.0.0.0
        // return -1 when version1 < version2

        // return 0 when version1 == version2

        // return 1 when version1 > version2

        var leftSideSegments = new Version(leftSideVersion, separator);
        var rightSideSegments = new Version(rightSideVersion, separator);

        return Compare(leftSideSegments, rightSideSegments);
    }

    public int CompareVersions(TVersion leftSideVersion, TVersion rightSideVersion)
    {
        return Compare(leftSideVersion, rightSideVersion);
    }

    public bool Equals(TVersion? leftSideVersion, TVersion? rightSideVersion) => leftSideVersion.Equals(rightSideVersion);

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

    public int GetHashCode([DisallowNull] TVersion obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return obj.GetHashCode();
    }

    public int GetHashCode(object obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return obj?.GetHashCode() ?? GetHashCode();
    }

    //private (Version version1, Version version2) checkVersionParameters(object leftSideVersion, object rightSideVersion)
    //{
    //    string parameterTypeError = $"Argument {1} is not of type {typeof(Version).ToString()}";
    //    string parameterNullError = $"Argument {1} is null";

    //    if (leftSideVersion is not Version version1)
    //    {
    //        throw new ArgumentException(string.Format(parameterTypeError, rightSideVersion), nameof(rightSideVersion));
    //    }
    //    if (rightSideVersion is not Version version2)
    //    {
    //        throw new ArgumentException(string.Format(parameterTypeError, leftSideVersion), nameof(leftSideVersion));
    //    }
    //    if (leftSideVersion == null || rightSideVersion == null) throw new ArgumentNullException(string.Format(parameterNullError));

    //    return (version1, version2);
    //}
}