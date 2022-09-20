

namespace CompareVersions.UI;

/// <summary>
///     Class that represents a single segment of <see cref="Version"/> object
/// </summary>
/// <seealso cref="System.IComparable<CompareVersions.UI.Segment>" />
/// <seealso cref="System.IEquatable<CompareVersions.UI.Segment>" />
/// <seealso cref="System.IComparable" />
public class Segment : IComparable<Segment>, IEquatable<Segment>, IComparable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Segment"/> class.
    /// </summary>
    /// <param name="segmentType">The SegmentType<see cref="SegmentType"/></param>
    /// <param name="value">The value.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">Segment cannot be greater than 100, per spec</exception>
    [SetsRequiredMembers()]
    public Segment(SegmentType segmentType, int value = 0)
    {
        if (value >= 100) throw new ArgumentOutOfRangeException("Segment cannot be greater than 100, per spec");

        SegmentType = segmentType;
        Value = value;
    }

    /// <summary>
    /// Gets the segment.
    /// </summary>
    /// <returns></returns>
    public (SegmentType segmentType, int value) GetSegment()
    {
        return (this.SegmentType, this.Value);
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public int Value { get; init; }
    /// <summary>
    /// Defaults this instance.
    /// </summary>
    /// <returns></returns>
    public static Segment Default => new Segment(0);
    /// <summary>
    /// Gets or sets the type of the segment.
    /// </summary>
    /// <value>
    /// The type of the segment.
    /// </value>
    public required SegmentType SegmentType { get; set; }
    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="obj" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="obj" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="obj" /> in the sort order.</description></item></list>
    /// </returns>
    /// <exception cref="System.ArgumentException">Object is not a Segment</exception>
    public int CompareTo([AllowNull] object obj)
    {
        if (obj == null) return 1;

        if (obj is Segment otherSegment)
            return this.Value.CompareTo(otherSegment.Value);
        else
            throw new ArgumentException("Object is not a Segment");
    }
    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo([AllowNull] Segment other)
    {
        if (other == null) return 1;
        return this.Value.CompareTo(other.Value);
    }
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals([AllowNull] Segment other)
    {
        if (other == null)
            return false;

        if (this.Value == other.Value)
            return true;
        else
            return false;
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
        if (obj == null)
            return false;

        Segment segmentObj = obj as Segment;
        if (segmentObj == null)
            return false;
        else
            return Equals(segmentObj);
    }
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }
    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="segment1">The segment1.</param>
    /// <param name="segment2">The segment2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator ==(Segment segment1, Segment segment2)
    {
        if (((object)segment1) == null || ((object)segment2) == null)
            return Object.Equals(segment1, segment2);

        return segment1.Equals(segment2);
    }
    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="segment1">The segment1.</param>
    /// <param name="segment2">The segment2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator !=(Segment segment1, Segment segment2)
    {
        if (((object)segment1) == null || ((object)segment2) == null)
            return !Object.Equals(segment1, segment2);

        return !(segment1.Equals(segment2));
    }

    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="segment1">The segment1.</param>
    /// <param name="segment2">The segment2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >(Segment segment1, Segment segment2)
    {
        return segment1.CompareTo(segment2) > 0;
    }

    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="segment1">The segment1.</param>
    /// <param name="segment2">The segment2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <(Segment segment1, Segment segment2)
    {
        return segment1.CompareTo(segment2) < 0;
    }

    /// <summary>
    /// Implements the operator &gt;=.
    /// </summary>
    /// <param name="segment1">The segment1.</param>
    /// <param name="segment2">The segment2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >=(Segment segment1, Segment segment2)
    {
        return segment1.CompareTo(segment2) >= 0;
    }

    /// <summary>
    /// Implements the operator &lt;=.
    /// </summary>
    /// <param name="segment1">The segment1.</param>
    /// <param name="segment2">The segment2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <=(Segment segment1, Segment segment2)
    {
        return segment1.CompareTo(segment2) <= 0;
    }
    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return this.Value.ToString();
    }
}