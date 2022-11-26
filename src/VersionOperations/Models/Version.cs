namespace CompareVersions.Models;

/// <summary>
/// Class that represents a Version object, consisting of major.minor.patch.build <see cref="Segment" /> objects
/// </summary>
/// <seealso cref="ICloneable" />
/// <seealso cref="System.Numerics.IAdditionOperators{TSelf, TOther, TResult}" />
/// <seealso cref="IComparable{Version}" />
/// <seealso cref="IEquatable{Version}" />
/// <seealso cref="IComparable" />
/// <seealso cref="IAdditiveIdentity{TSelf,TResult}"/>
/// <seealso cref="ISpanFormattable"/>
/// <seealso cref="ISpanParsable{Version}"/>
/// <seealso cref="IParsable{Version}"/>
[TypeConverter(typeof(UI.VersionConverter))]
[Serializable()]
public class Version : IEnumerable<Segment>,
    IComparable<Version>,
    IEquatable<Version>,
    IComparable,
    ICloneable,
    IAdditionOperators<Version, Version, Version>,
    IAdditiveIdentity<Version, Version>,
    ISpanFormattable,
    ISpanParsable<Version>,
    IParsable<Version>
{
    private static readonly string TooManySegments = "Version string can only have at most 4 segments.";
    private static readonly char Separator = Constants.VersionSeparators[0];
    private static readonly int Floor = Constants.VersionSegmentFloor;
    private static readonly int Ceiling = Constants.VersionSegmentCeiling;

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
        get => Segments[position];
        set => Segments[position] = value;
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
        get => Segments[(int)segmentType];
        set => Segments[(int)segmentType] = value;
    }

    /// <summary>
    ///     Represents an uninitialized <see cref="Version"/> instance
    /// </summary>
    /// <remarks>
    ///     This initializes all <see cref="Segment"/>values to 0 (zero)
    /// </remarks>
    [SetsRequiredMembers]
    public Version() : this(0, 0, 0, 0) { }

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
                  new(SegmentType.Major, version.Major),
                  new(SegmentType.Minor, version.Minor),
                  new(SegmentType.Patch, version.Build),
                  new(SegmentType.Build, version.Revision)
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

        Segments = segments;
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
    /// <param name="version">The version.</param>
    [SetsRequiredMembers()]
    private Version(Version version)
    {
        Debug.Assert(version != null);

        this.Segments = version.Segments;

        this.MajorSegment = version.MajorSegment;
        this.MinorSegment = version.MinorSegment;
        this.PatchSegment = version.PatchSegment;
        this.BuildSegment = version.BuildSegment;
    }

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
              new(SegmentType.Major, major),
              new(SegmentType.Minor, minor),
              new(SegmentType.Patch, patch),
              new(SegmentType.Build, build)
          }
        )
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Version"/>class
    /// </summary>
    /// <param name="parts">All parts more than 4 in length are discards and ignored</param>
    [SetsRequiredMembers()]
    public Version(params int[] parts)
    {
        if (parts == null) throw new ArgumentNullException(nameof(parts));

        switch (parts.Length)
        {
            case > 4:
                throw new ArgumentOutOfRangeException(TooManySegments);
            case 0:
                throw new ArgumentException("Value cannot be an empty collection.", nameof(parts));
        }

        var segments = new List<Segment>(parts.Length);

        for (int i = 0; i < parts.Length; i++)
        {
            segments.Add(new((SegmentType)i, parts[i]));

            //this[i] = new Segment((SegmentType)(i), parts[i]);
            //if (i > 4) return;
        }
        Segments = segments;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="segments">The segments.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [SetsRequiredMembers()]
    public Version(params Segment[] segments)
        : this(segments.ToList()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="segments">The segments.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [SetsRequiredMembers()]
    public Version(List<Segment> segments)
    {
        if (segments == null) throw new ArgumentNullException(nameof(segments));
        if (segments.Count > 4) throw new ArgumentOutOfRangeException(TooManySegments);
        Segments = segments;
        SetVersionSegmentProperties(segments);
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <returns></returns>
    public (Segment major, Segment minor, Segment? patch, Segment? build) GetVersion()
        => (MajorSegment, MinorSegment, PatchSegment, BuildSegment);

    /// <summary>
    /// Sets the version segment properties.
    /// </summary>
    /// <param name="segments">The segments.</param>
    /// <exception cref="System.ArgumentNullException">segments</exception>
    /// <exception cref="System.ArgumentException">Value cannot be an empty collection. - segments</exception>
    private void SetVersionSegmentProperties(List<Segment> segments)
    {
        if (segments == null) throw new ArgumentNullException(nameof(segments));
        if (segments.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(segments));

        MajorSegment = segments[0];
        MinorSegment = segments[1];
        PatchSegment = segments[2] ?? Segment.Default;
        BuildSegment = segments[3] ?? Segment.Default;
    }

    /// <summary>
    /// Returns a default instance of the <see cref="Version"/> object
    /// </summary>
    public static Version Default => default(Version);


    /// <summary>
    /// Randoms the integer.
    /// </summary>
    /// <param name="minimum">The minimum.</param>
    /// <param name="maximum">The maximum.</param>
    /// <returns></returns>
    public static int RandomInteger(int minimum, int maximum)
    {
        return RandomNumberGenerator.GetInt32(minimum, maximum);
    }

    /// <summary>
    /// Creates a random Version object
    /// </summary>
    /// <param name="randomizer"></param>
    /// <param name="separator"></param>
    /// <param name="numberOfSegments"></param>
    /// <returns>A fully constructed <see cref="Version"/> object for consumption.</returns>
    public static Version CreateRandom(Func<int, int, int> randomizer, char? separator, int numberOfSegments = 4)
    {
        _ = separator ?? Constants.VersionSeparators[0];

        if (numberOfSegments > Constants.MaxNumberOfSegments)
            throw new ArgumentOutOfRangeException(nameof(numberOfSegments), $"Too many segments specified: {numberOfSegments} supplied, {Constants.MaxNumberOfSegments} is max accepted.");
        if (numberOfSegments <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfSegments), $"Must specify at least one segment for range {uint.MinValue} to {Constants.MaxNumberOfSegments}");

        int min = Constants.VersionSegmentFloor;
        int max = Constants.VersionSegmentCeiling;
        var random = randomizer;

        List<Segment> segments = new(numberOfSegments);
        for (int i = 0; i < numberOfSegments; i++)
        {
            Segment s = new((SegmentType)i, random(min, max));
            segments.Add(s);
        }
        return new Version(segments);
    }
    /// <summary>
    /// Adds the segment.
    /// </summary>
    /// <param name="segment">The segment.</param>
    /// <param name="replaceIfFound"></param>
    public bool AddSegment(Segment segment, bool replaceIfFound)
    {
        if (segment == null) throw new ArgumentNullException(nameof(segment));
        if (!Segments.Contains(segment))
        {
            // simply add the segment
            Segments.Add(segment);

        }
        else if (replaceIfFound && Segments.Contains(segment))
        {
            Segments.RemoveAt((int)segment.SegmentType);
            Segments.Add(segment);

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
        return AddSegment(segment, replaceIfFound);
    }

    /// <summary>
    /// Gets the segment.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public Segment GetSegment(int index)
        => Segments[index];

    /// <summary>
    /// Gets the segment.
    /// </summary>
    /// <param name="segmentType">Type of the segment.</param>
    /// <returns></returns>
    public Segment GetSegment(SegmentType segmentType)
        => Segments[(int)segmentType];

    /// <summary>
    /// Replaces the segment.
    /// </summary>
    /// <param name="segment">The segment.</param>
    /// <param name="replaceIfFound">if set to <c>true</c> [replace if found].</param>
    /// <returns></returns>
    public bool ReplaceSegment(Segment segment, bool replaceIfFound = true)
        => AddSegment(segment, replaceIfFound);

    /// <summary>
    /// Replaces the segment.
    /// </summary>
    /// <param name="segmentType">Type of the segment.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool ReplaceSegment(SegmentType segmentType, int value)
        => AddSegment(segmentType, value, true);

    /// <summary>
    /// Removes the segment.
    /// </summary>
    /// <param name="segment">The segment.</param>
    /// <returns></returns>
    public bool RemoveSegment(Segment segment)
    {
        return Segments.Contains(segment) && Segments.Remove(segment);
    }

    /// <summary>
    /// Removes the segment.
    /// </summary>
    /// <param name="segmentType">Type of the segment.</param>
    /// <returns></returns>
    public bool RemoveSegment(SegmentType segmentType)
    {
        if (Segments.Exists(s => s.SegmentType == segmentType))
        {
            var segment = Segments.Find(s => s.SegmentType == segmentType);
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
        get => Segments[(int)SegmentType.Major];
        set => Segments[(int)SegmentType.Major] = value;
    }

    /// <summary>
    /// Gets or sets the minor segment.
    /// </summary>
    /// <value>
    /// The minor segment.
    /// </value>
    public required Segment MinorSegment
    {
        get => Segments[(int)SegmentType.Minor];
        set => Segments[(int)SegmentType.Minor] = value;
    }
    /// <summary>
    /// Gets or sets the patch segment.
    /// </summary>
    /// <value>
    /// The patch segment.
    /// </value>
    public required Segment PatchSegment
    {
        get => Segments[(int)SegmentType.Patch];
        set => Segments[(int)SegmentType.Patch] = value ?? Segment.Default;
    }
    /// <summary>
    /// Gets or sets the build segment.
    /// </summary>
    /// <value>
    /// The build segment.
    /// </value>
    public required Segment BuildSegment
    {
        get => Segments[(int)SegmentType.Build];
        set => Segments[(int)SegmentType.Build] = value ?? Segment.Default;
    }

    /// <summary>
    /// Gets the additive identity.
    /// </summary>
    /// <value>
    /// The additive identity.
    /// </value>
    public static Version AdditiveIdentity => Version.Default;

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        StringBuilder segments = new();
        foreach (var segment in Segments)
        {
            segments.Append(segment);
            segments.Append(Separator);
        }

        segments.Length--;
        return segments.ToString();
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <param name="fieldCount">The field count.</param>
    /// <returns></returns>
    public string ToString(int fieldCount)
    {
        Span<char> dest = stackalloc char[(4 * (10 + 1)) + 3]; // at most 4 Int32s and 3 periods
        bool success = TryFormat(dest, fieldCount, out int charsWritten);
        Debug.Assert(success);
        return dest[..charsWritten].ToString();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns></returns>
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    /// <summary>
    /// Tries the format.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="charsWritten">The chars written.</param>
    /// <param name="format">The format.</param>
    /// <param name="provider">The provider.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        =>
            // format and provider are ignored.
            TryFormat(destination, DefaultFormatFieldCount, out charsWritten);

    /// <summary>
    /// Tries the format.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="charsWritten">The chars written.</param>
    /// <returns></returns>
    public bool TryFormat(Span<char> destination, out int charsWritten) =>
        TryFormat(destination, DefaultFormatFieldCount, out charsWritten);

    /// <summary>
    /// Tries the format.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="fieldCount">The field count.</param>
    /// <param name="charsWritten">The chars written.</param>
    /// <returns></returns>
    public bool TryFormat(Span<char> destination, int fieldCount, out int charsWritten)
    {
        switch ((uint)fieldCount)
        {
            case > 4:
                ThrowArgumentException("4");
                break;

            case >= 3 when BuildSegment.Value == -1:
                ThrowArgumentException("2");
                break;

            case 4 when PatchSegment.Value == -1:
                ThrowArgumentException("3");
                break;

                static void ThrowArgumentException(string failureUpperBound)
                {
                    //throw new ArgumentException(SR.Format(SR.ArgumentOutOfRange_Bounds_Lower_Upper, "0", failureUpperBound), nameof(fieldCount));

                    throw new ArgumentException(Format("ArgumentOutOfRange_Bounds_Lower_Upper", 0, failureUpperBound, true), nameof(fieldCount));


                }
        }

        int totalCharsWritten = 0;

        for (int i = 0; i < fieldCount; i++)
        {
            if (i != 0)
            {
                if (destination.IsEmpty)
                {
                    charsWritten = 0;
                    return false;
                }

                destination[0] = '.';
                destination = destination.Slice(1);
                totalCharsWritten++;
            }

            int value = i switch
            {
                0 => MajorSegment.Value,
                1 => MinorSegment.Value,
                2 => BuildSegment.Value,
                _ => PatchSegment.Value
            };

            if (!((uint)value).TryFormat(destination, out int valueCharsWritten))
            {
                charsWritten = 0;
                return false;
            }

            totalCharsWritten += valueCharsWritten;
            destination = destination[valueCharsWritten..];
        }

        charsWritten = totalCharsWritten;
        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Version Parse(string s, IFormatProvider? provider)
    {
        return Parse(s);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Version Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return Parse(s);
    }


    /// <summary>
    /// Parses the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static Version Parse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return ParseVersion(input.AsSpan(), throwOnFailure: true)!;
    }

    /// <summary>
    /// Parses the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static Version Parse(ReadOnlySpan<char> input) =>
        ParseVersion(input, throwOnFailure: true)!;

    /// <summary>
    /// Tries the parse.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out Version? result)
    {
        if (input == null)
        {
            result = null;
            return false;
        }

        return (result = ParseVersion(input.AsSpan(), throwOnFailure: false)) != null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <param name="provider"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Version result)
    {
        return TryParse(s, out result);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <param name="provider"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Version result)
    {
        return TryParse(s, out result);
    }

    /// <summary>
    /// Tries the parse.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    public static bool TryParse(ReadOnlySpan<char> input, [NotNullWhen(true)] out Version? result) =>
        (result = ParseVersion(input, throwOnFailure: false)) != null;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals([NotNullWhen(true)] Version? other)
    {
        if (other == null)
            return false;

        // check that major, minor, build & revision numbers match
        if (MajorSegment != other.MajorSegment ||
            MinorSegment != other.MinorSegment ||
            PatchSegment != other.PatchSegment ||
            BuildSegment != other.BuildSegment)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified <see cref="object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Version v && Equals(v);
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="value">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="value" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="value" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo([AllowNull] Version value)
    {
        if (value == null)
            return 1;

        if (MajorSegment != value.MajorSegment)
            if (MajorSegment > value.MajorSegment)
                return 1;
            else
                return -1;

        if (MinorSegment != value.MinorSegment)
            if (MinorSegment > value.MinorSegment)
                return 1;
            else
                return -1;

        if (PatchSegment != value.PatchSegment)
            if (PatchSegment > value.PatchSegment)
                return 1;
            else
                return -1;

        if (BuildSegment != value.BuildSegment)
            if (BuildSegment > value.BuildSegment)
                return 1;
            else
                return -1;

        return 0;

        //return
        //    object.ReferenceEquals(value, this) ? 0 :
        //    value is null ? 1 :
        //    MajorSegment != value.MajorSegment ? (MajorSegment > value.MajorSegment ? 1 : -1) :
        //    MinorSegment != value.MinorSegment ? (MinorSegment > value.MinorSegment ? 1 : -1) :
        //    BuildSegment != value.BuildSegment ? (BuildSegment > value.BuildSegment ? 1 : -1) :
        //    PatchSegment != value.PatchSegment ? (PatchSegment > value.PatchSegment ? 1 : -1) :
        //    0;
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="version">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="version" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="version" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="version" /> in the sort order.</description></item></list>
    /// </returns>
    /// <exception cref="ArgumentException">Object is not a Segment</exception>
    public int CompareTo([AllowNull] object version)
    {
        if (version == null) return 1;

        Version v = version as Version;
        if (v == null)
        {
            throw new ArgumentException("Argument must be of type 'Version'.", nameof(version));
        }

        return CompareTo(v);

        //if (version is Version otherVersion)
        //    return this.CompareTo(otherVersion);
        //else
        //    throw new ArgumentException("Object is not a Segment");

    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <remarks>
    /// Let's assume that most version numbers will be pretty small and just OR some bits together
    /// </remarks>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode()
    {
        int accumulator = 0;

        accumulator |= (MajorSegment.Value & 0x0000000F) << 28;
        accumulator |= (MinorSegment.Value & 0x000000FF) << 20;
        accumulator |= (BuildSegment.Value & 0x000000FF) << 12;
        accumulator |= (PatchSegment.Value & 0x00000FFF);

        return accumulator;
    }

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
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
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
        if ((object)left == null)
            throw new ArgumentNullException(nameof(left));
        return left.CompareTo(right) < 0;
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
        if ((object)left == null)
            throw new ArgumentNullException(nameof(left));
        return left.CompareTo(right) <= 0;
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
        return right < left;
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
        return right <= left;
    }

    /// <summary>
    /// Implements the operator op_Addition.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static Version operator +(Version left, Version right)
    {
        List<Segment> segments = new();

        foreach (var s in left)
        {
            foreach (var r in right)
            {
                if (s.SegmentType == r.SegmentType)
                {
                    var segment = s + r;
                    segments.Add(segment);
                }
            }
        }

        return new Version(segments);
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

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<Segment> GetEnumerator()
    {
        foreach (var segment in this.Segments)
        {
            yield return segment;
        }
    }

    /// <summary>
    /// Gets the non-generic enumerator, calls into generic version since <see cref="Segment"/> is known generic parameter type
    /// </summary>
    /// <returns>a <seealso cref="IEnumerable{Segment}"/></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Sets the default format field count.
    /// </summary>
    /// <value>
    /// The default format field count.
    /// </value>
    private int DefaultFormatFieldCount =>
        BuildSegment.Value == -1 ? 2 :
        PatchSegment.Value == -1 ? 3 :
        4;

    /// <summary>
    /// Parses the version.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="throwOnFailure">if set to <c>true</c> [throw on failure].</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">ArgumentOutOfRange_Version, nameof(input)</exception>
    private static Version? ParseVersion(ReadOnlySpan<char> input, bool throwOnFailure)
    {
        // Find the separator between major and minor.  It must exist.
        int majorEnd = input.IndexOf('.');
        if (majorEnd < 0)
        {
            if (throwOnFailure) throw new ArgumentException("ArgumentOutOfRange_Version", nameof(input));
            return null;
        }

        // Find the ends of the optional minor and build portions.
        // We musn't have any separators after build.
        int buildEnd = -1;
        int minorEnd = input.Slice(majorEnd + 1).IndexOf('.');
        if (minorEnd >= 0)
        {
            minorEnd += (majorEnd + 1);
            buildEnd = input.Slice(minorEnd + 1).IndexOf('.');
            if (buildEnd >= 0)
            {
                buildEnd += (minorEnd + 1);
                if (input.Slice(buildEnd + 1).Contains('.'))
                {
                    if (throwOnFailure) throw new ArgumentException("ArgumentOutOfRange_Version", nameof(input));
                    // throw new ArgumentException(SR.Arg_VersionString, nameof(input)); // Format("ArgumentOutOfRange_Bounds_Lower_Upper", 0, failureUpperBound, true), nameof(fieldCount));
                    return null;
                }
            }
        }

        int minor;

        // Parse the major version
        if (!TryParseComponent(input[..majorEnd], nameof(input), throwOnFailure, out int major))
        {
            return null;
        }

        if (minorEnd != -1)
        {
            // If there's more than a major and minor, parse the minor, too.
            if (!TryParseComponent(input.Slice(majorEnd + 1, minorEnd - majorEnd - 1), nameof(input), throwOnFailure, out minor))
            {
                return null;
            }

            int build;
            if (buildEnd != -1)
            {
                // major.minor.build.revision
                int revision;
                return
                    TryParseComponent(input.Slice(minorEnd + 1, buildEnd - minorEnd - 1), nameof(build), throwOnFailure, out build) &&
                    TryParseComponent(input[(buildEnd + 1)..], nameof(revision), throwOnFailure, out revision) ?
                        new Version(major, minor, build, revision) :
                        null;
            }

            // major.minor.build
            return TryParseComponent(input[(minorEnd + 1)..], nameof(build), throwOnFailure, out build) ?
                new Version(major, minor, build) :
                null;
        }

        // major.minor
        return TryParseComponent(input[(majorEnd + 1)..], nameof(input), throwOnFailure, out minor) ?
            new Version(major, minor) :
            null;
    }

    /// <summary>
    /// Tries the parse component.
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="componentName">Name of the component.</param>
    /// <param name="throwOnFailure">if set to <c>true</c> [throw on failure].</param>
    /// <param name="parsedComponent">The parsed component.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">componentName, ArgumentOutOfRange_Version</exception>
    private static bool TryParseComponent(ReadOnlySpan<char> component, string componentName, bool throwOnFailure, out int parsedComponent)
    {
        if (throwOnFailure)
        {
            if ((parsedComponent = int.Parse(component, NumberStyles.Integer, CultureInfo.InvariantCulture)) < 0)
            {
                throw new ArgumentOutOfRangeException(componentName, "ArgumentOutOfRange_Version");
            }
            return true;
        }

        return int.TryParse(component, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedComponent) && parsedComponent >= 0;
    }

    /// <summary>
    /// Formats the specified resource format.
    /// </summary>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="p1">The p1.</param>
    /// <param name="p2">The p2.</param>
    /// <param name="useResourceKeys">if set to <c>true</c> [use resource keys].</param>
    /// <returns></returns>
    private static string Format(string resourceFormat, object? p1, object? p2, bool useResourceKeys)
    {
        return useResourceKeys ? string.Join(", ", resourceFormat, p1, p2) : string.Format(resourceFormat, p1, p2);
    }
}