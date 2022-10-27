using Version = CompareVersions.Models.Version;

namespace CompareVersions.Extensions;

/// <summary>
///     Class housing extension methods for the <see cref="Version"/> type.
/// </summary>
public static class VersionExtensions
{
    /// <summary>
    /// Creates the random.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="randomizerAlgorithm"></param>
    /// <param name="separator">The _separator.</param>
    /// <returns>A fully constructed <see cref="Version"/>type.</returns>
    public static Version CreateRandom(this Version version, Func<int, int, int>? randomizerAlgorithm, char? separator)
    {
        char sep = separator ??= Constants.VersionSeparators[0];

        int maxSegments = Constants.MaxNumberOfSegments;
        StringBuilder versionString = new();

        for (int i = 1; i <= maxSegments; i++)
        {
            var quad = RandomNumberGenerator.GetInt32(Constants.VersionSegmentFloor, Constants.VersionSegmentCeiling);

            versionString.Append(quad);
            versionString.Append(separator);
        }

        versionString.Length--;
        return new Version(versionString.ToString(), sep);
    }
}
