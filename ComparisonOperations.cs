namespace CompareVersions.UI;

/// <summary>
/// Class containing operations that compare version strings
/// </summary>
public class ComparisonOperations
{

    private readonly char separator = '.';

    /// <summary>Compares the versions.</summary>
    /// <param name="leftSideVersion">The left side version.</param>
    /// <param name="rightSideVersion">The right side version.</param>
    /// <returns>
    ///   An integer representing the result of the comparison.
    /// </returns>
    public int CompareVersions(string leftSideVersion, string rightSideVersion)
    {

        // given rightSideVersion verison numbers e.g. 2.5.1 and 2.4.1.6, compare them, return 0 if equal, return -1 if a > b, and 1 if a < b

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

        return leftSideSegments.CompareTo(rightSideSegments);
    }
}