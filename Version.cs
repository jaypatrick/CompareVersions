using CompareVersions.Extensions;

namespace CompareVersions.UI;

/// <summary>
///    Class that represents a Version object, consisting of major.minor.patch.build <see cref="Segment"/> objects
/// </summary>
/// <seealso cref="System.IComparable{CompareVersions.UI.Version}>
/// <seealso cref="System.IEquatable{CompareVersions.UI.Version}
/// <seealso cref="System.IComparable" />
[TypeConverter(typeof(VersionConverter))]
public class Version : IComparable<Version>, IEquatable<Version>, IComparable, System.ICloneable
{
    private static readonly string tooManySegments = "Version string can only have at most 4 segments.";
    private static readonly char _separator = Constants.VersionSeparators[0];
    private static readonly int _floor = Constants.VersionSegmentFloor;
    private static readonly int _ceiling = Constants.VersionSegmentCeiling;

    /// <summary>
    /// Gets the <see cref="Segment"/> with the specified position.
    /// </summary>
    /// <value>
    /// The <see cref="Segment"/>.
    /// </value>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    public Segment this[int position]
    {
        get { return this.Segments[position]; }
        set { this.Segments[position] = value; }
    }

    /// <summary>
    /// Gets the <see cref="Segment"/> with the specified segment type.
    /// </summary>
    /// <value>
    /// The <see cref="Segment"/>.
    /// </value>
    /// <param name="segmentType">Type of the segment.</param>
    /// <returns></returns>
    public Segment this[SegmentType segmentType]
    {
        get { return this.GetSegment(segmentType); }
        set { this.Segments[(int)segmentType] = value; }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    [Obsolete("Use conversion methods implemented by IConvertible, or the methods exposed by the TypeConverter", true)]
    [SetsRequiredMembers()]
    public Version(System.Version version)
        : this
            (
              new List<Segment>
              {
                  new Segment(SegmentType.Major, version.Major),
                  new Segment(SegmentType.Minor, version.Minor),
                  new Segment(SegmentType.Patch, version.Build),
                  new Segment(SegmentType.Build, version.Revision)
              }
            )
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="versionString">The version string.</param>
    /// <param name="separator"></param>
    [SetsRequiredMembers()]
    public Version(string versionString, char separator)

    {
        var segmentedString = versionString.Split(separator);
        List<Segment> segments = new();

        for (int i = 0; i < segmentedString.Length; i++)
        {
            var segment = new Segment((SegmentType)i, int.Parse(segmentedString[i]));

            segments.Add(segment);
        }

        this.Segments = segments;
        SetVersionSegmentProperties(segments);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="majorSegment">The major segment.</param>
    /// <param name="minorSegment">The minor segment.</param>
    /// <param name="patchSegment">The patch segment.</param>
    /// <param name="buildSegment">The build segment.</param>
    [SetsRequiredMembers()]
    public Version(Segment majorSegment, Segment minorSegment, Segment patchSegment, Segment buildSegment)
        : this
              (
                new List<Segment>
                {
                    majorSegment, minorSegment, patchSegment, buildSegment
                }
              )
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="patch">The patch.</param>
    /// <param name="build">The build.</param>
    [SetsRequiredMembers()]
    public Version(int major, int minor, int patch = 0, int build = 0)
        : this
        (
              new List<Segment>
              {
                  new Segment(SegmentType.Major, major),
                  new Segment(SegmentType.Minor, minor),
                  new Segment(SegmentType.Patch, patch),
                  new Segment(SegmentType.Build, build)
              }
        )
    { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="segments">The segments.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    [SetsRequiredMembers()]
    public Version(params Segment[] segments)
        : this(segments.ToList()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="segments">The segments.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    [SetsRequiredMembers()]
    public Version(List<Segment> segments)
    {
        if (segments.Count > 4) throw new ArgumentOutOfRangeException(tooManySegments);
        this.Segments = segments;
        SetVersionSegmentProperties(segments);
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <returns></returns>
    public (Segment major, Segment minor, Segment? patch, Segment? build) GetVersion()
        => (this.MajorSegment, this.MinorSegment, this.PatchSegment, this.BuildSegment);

    private void SetVersionSegmentProperties(List<Segment> segments)
    {
        this.MajorSegment = segments[0];
        this.MinorSegment = segments[1];
        this.PatchSegment = segments[2] ?? Segment.Default;
        this.BuildSegment = segments[3] ?? Segment.Default;
    }

    /// <summary>
    /// Creates a random Version object
    /// </summary>
    /// <param name="separator"></param>
    /// <returns>A full constructed <see cref="Version"/>object for consumption.</returns>
    public static Version CreateRandom(char? separator) => new Version().CreateRandom(separator ?? _separator);

    /// <summary>
    /// Adds the segment.
    /// </summary>
    /// <param name="segment">The segment.</param>
    public bool AddSegment(Segment segment, bool replaceIfFound)
    {
        if (!this.Segments.Contains(segment))
        {
            // simply add the segment
            this.Segments.Add(segment);

        }
        else if (replaceIfFound && this.Segments.Contains(segment))
        {
            this.Segments.RemoveAt((int)segment.SegmentType);
            this.Segments.Add(segment);

        }
        else return false;

        return Segments.Contains(segment);
    }

    /// <summary>
    /// Adds the segment.
    /// </summary>
    /// <param name="segmentType">Type of the segment.</param>
    /// <param name="value">The value.</param>
    /// <param name="replaceIfFound">if set to <c>true</c> [replace if found].</param>
    /// <returns></returns>
    public bool AddSegment(SegmentType segmentType, int value, bool replaceIfFound)
    {
        var segment = new Segment(segmentType, value);
        return this.AddSegment(segment, replaceIfFound);
    }

    /// <summary>
    /// Gets the segment.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public Segment GetSegment(int index)
        => this.Segments[index];

    /// <summary>
    /// Gets the segment.
    /// </summary>
    /// <param name="segmentType">Type of the segment.</param>
    /// <returns></returns>
    public Segment GetSegment(SegmentType segmentType)
        => this.Segments[(int)segmentType];

    /// <summary>
    /// Replaces the segment.
    /// </summary>
    /// <param name="segment">The segment.</param>
    /// <returns></returns>
    public bool ReplaceSegment(Segment segment)
        => this.AddSegment(segment, true);

    /// <summary>
    /// Replaces the segment.
    /// </summary>
    /// <param name="segmentType">Type of the segment.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool ReplaceSegment(SegmentType segmentType, int value)
        => this.AddSegment(segmentType, value, true);

    /// <summary>
    /// Removes the segment.
    /// </summary>
    /// <param name="segment">The segment.</param>
    /// <returns></returns>
    public bool RemoveSegment(Segment segment)
    {
        if (this.Segments.Contains(segment))
            return this.Segments.Remove(segment);
        else return false;
    }

    /// <summary>
    /// Removes the segment.
    /// </summary>
    /// <param name="segmentType">Type of the segment.</param>
    /// <returns></returns>
    public bool RemoveSegment(SegmentType segmentType)
    {
        if (this.Segments.Exists(s => s.SegmentType == segmentType))
        {
            var segment = this.Segments.Find(s => s.SegmentType == segmentType);
            return RemoveSegment(segment!);
        }
        return false;
    }

    /// <summary>
    /// Gets or sets the segments.
    /// </summary>
    /// <value>
    /// The segments.
    /// </value>
    public List<Segment> Segments { get; init; }

    /// <summary>
    /// Gets or sets the major segment.
    /// </summary>
    /// <value>
    /// The major segment.
    /// </value>
    public required Segment MajorSegment
    {
        get { return this.Segments[(int)SegmentType.Major]; }
        set { this.Segments[(int)SegmentType.Major] = value; }
    }

    /// <summary>
    /// Gets or sets the minor segment.
    /// </summary>
    /// <value>
    /// The minor segment.
    /// </value>
    public required Segment MinorSegment
    {
        get { return this.Segments[(int)SegmentType.Minor]; }
        set { this.Segments[(int)SegmentType.Minor] = value; }
    }
    /// <summary>
    /// Gets or sets the patch segment.
    /// </summary>
    /// <value>
    /// The patch segment.
    /// </value>
    public required Segment PatchSegment
    {
        get { return this.Segments[(int)SegmentType.Patch]; }
        set { this.Segments[(int)SegmentType.Patch] = value ?? Segment.Default; }
    }
    /// <summary>
    /// Gets or sets the build segment.
    /// </summary>
    /// <value>
    /// The build segment.
    /// </value>
    public required Segment BuildSegment
    {
        get { return this.Segments[(int)SegmentType.Build]; }
        set { this.Segments[(int)SegmentType.Build] = value ?? Segment.Default; }
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// <returns>A <see cref="System.String"/>string representation of this object, e.g major.minor.patch.build</returns>
    public override string ToString()
    {
        StringBuilder segments = new StringBuilder();
        foreach (var segment in this.Segments)
        {
            segments.Append(segment);
            segments.Append(_separator);
        }

        segments.Length--;
        return segments.ToString();
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals([AllowNull] Version other)
    {
        if (other == null)
            return false;

        // check that major, minor, build & revision numbers match
        if ((this.MajorSegment != other.MajorSegment) ||
            (this.MinorSegment != other.MinorSegment) ||
            (this.PatchSegment != other.PatchSegment) ||
            (this.BuildSegment != other.BuildSegment))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals([AllowNull] object obj)
    {
        if (obj is not Version v)
            return false;

        return Equals(v);
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="value">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="value" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo([AllowNull] Version value)
    {
        if (value == null)
            return 1;

        if (this.MajorSegment != value.MajorSegment)
            if (this.MajorSegment > value.MajorSegment)
                return 1;
            else
                return -1;

        if (this.MinorSegment != value.MinorSegment)
            if (this.MinorSegment > value.MinorSegment)
                return 1;
            else
                return -1;

        if (this.PatchSegment != value.PatchSegment)
            if (this.PatchSegment > value.PatchSegment)
                return 1;
            else
                return -1;

        if (this.BuildSegment != value.BuildSegment)
            if (this.BuildSegment > value.BuildSegment)
                return 1;
            else
                return -1;

        return 0;
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="version">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="version" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="version" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="version" /> in the sort order.</description></item></list>
    /// </returns>
    /// <exception cref="System.ArgumentException">Object is not a Segment</exception>
    public int CompareTo([AllowNull] object version)
    {
        if (version == null) return 1;

        Version v = version as Version;
        if (v == null)
        {
            throw new ArgumentException(nameof(version), "Argument must be of type 'Version'.");
        }

        return this.CompareTo(v);

        //if (version is Version otherVersion)
        //    return this.CompareTo(otherVersion);
        //else
        //    throw new ArgumentException("Object is not a Segment");

    }
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode() => this.GetHashCode();



    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator ==(Version left, Version right)
    {
        if (Object.ReferenceEquals(left, null))
        {
            return Object.ReferenceEquals(right, null);
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator !=(Version left, Version right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <(Version left, Version right)
    {
        if ((Object)left == null)
            throw new ArgumentNullException(nameof(left));
        return (left.CompareTo(right) < 0);
    }

    /// <summary>
    /// Implements the operator &lt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <=(Version left, Version right)
    {
        if ((Object)left == null)
            throw new ArgumentNullException(nameof(left));
        return (left.CompareTo(right) <= 0);
    }

    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >(Version left, Version right)
    {
        return (right < left);
    }

    /// <summary>
    /// Implements the operator &gt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >=(Version left, Version right)
    {
        return (right <= left);
    }

    /// <summary>
    /// Clones this instance.
    /// </summary>
    /// <returns>The cloned <see cref="Version"/>object.</returns>
    public Version Clone()
    {
        return new Version(majorSegment: new Segment(segmentType: SegmentType.Major, value: MajorSegment.Value),
            minorSegment: new Segment(segmentType: MinorSegment.SegmentType, value: MinorSegment.Value),
            patchSegment: new Segment(segmentType: PatchSegment.SegmentType, value: PatchSegment.Value),
            buildSegment: new Segment(segmentType: BuildSegment.SegmentType, value: BuildSegment.Value))
        {
            //_separator = _separator,
            Segments = Segments.ConvertAll(thisSegment => new Segment(segmentType: thisSegment.SegmentType, value: thisSegment.Value))
        };
    }

    /// <summary>
    /// Clones this instance.
    /// </summary>
    /// <returns></returns>
    object ICloneable.Clone()
    {
        return Clone();
    }
}