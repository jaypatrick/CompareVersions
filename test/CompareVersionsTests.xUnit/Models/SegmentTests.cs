using FluentAssertions;
using Xunit;

namespace CompareVersions.Models.Tests.xUnit;

public class SegmentTests
{
    [Fact]
    public void Segment_Constructor_WithValidValues_ShouldCreateSegment()
    {
        // Arrange & Act
        var segment = new Segment(SegmentType.Major, 5);

        // Assert
        segment.SegmentType.Should().Be(SegmentType.Major);
        segment.Value.Should().Be(5);
    }

    [Fact]
    public void Segment_Constructor_WithZeroValue_ShouldCreateSegment()
    {
        // Arrange & Act
        var segment = new Segment(SegmentType.Minor, 0);

        // Assert
        segment.Value.Should().Be(0);
    }

    [Fact]
    public void Segment_Constructor_WithValueGreaterThan99_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Segment(SegmentType.Major, 100));

        exception.ParamName.Should().Be("value");
    }

    [Fact]
    public void GetSegment_ShouldReturnTupleWithSegmentTypeAndValue()
    {
        // Arrange
        var segment = new Segment(SegmentType.Patch, 42);

        // Act
        var result = segment.GetSegment();

        // Assert
        result.segmentType.Should().Be(SegmentType.Patch);
        result.value.Should().Be(42);
    }

    [Fact]
    public void CompareTo_Object_WithNull_ShouldReturn1()
    {
        // Arrange
        var segment = new Segment(SegmentType.Major, 5);

        // Act
        var result = segment.CompareTo((object)null);

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void CompareTo_Object_WithGreaterSegment_ShouldReturnNegative()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 10);

        // Act
        var result = segment1.CompareTo((object)segment2);

        // Assert
        result.Should().BeNegative();
    }

    [Fact]
    public void CompareTo_Segment_WithEqualSegment_ShouldReturn0()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 5);

        // Act
        var result = segment1.CompareTo(segment2);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Equals_Segment_WithEqualSegment_ShouldReturnTrue()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 5);

        // Act
        var result = segment1.Equals(segment2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_Segment_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment(SegmentType.Major, 5);

        // Act
        var result = segment.Equals((Segment)null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_Object_WithDifferentSegment_ShouldReturnFalse()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 10);

        // Act
        var result = segment1.Equals((object)segment2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ForEqualSegments_ShouldReturnSameHashCode()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 5);

        // Act
        var hash1 = segment1.GetHashCode();
        var hash2 = segment2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnValueAsString()
    {
        // Arrange
        var segment = new Segment(SegmentType.Major, 42);

        // Act
        var result = segment.ToString();

        // Assert
        result.Should().Be("42");
    }

    [Fact]
    public void EqualityOperator_WithEqualSegments_ShouldReturnTrue()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 5);

        // Act
        var result = segment1 == segment2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void InequalityOperator_WithDifferentSegments_ShouldReturnTrue()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 10);

        // Act
        var result = segment1 != segment2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GreaterThanOperator_ShouldReturnCorrectResult()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 10);
        var segment2 = new Segment(SegmentType.Major, 5);

        // Act
        var result = segment1 > segment2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void LessThanOperator_ShouldReturnCorrectResult()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 10);

        // Act
        var result = segment1 < segment2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AdditionOperator_WithSameSegmentType_ShouldReturnSum()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 10);

        // Act
        var result = segment1 + segment2;

        // Assert
        result.Value.Should().Be(15);
        result.SegmentType.Should().Be(SegmentType.Major);
    }

    [Fact]
    public void AdditionOperator_WithDifferentSegmentTypes_ShouldThrowArgumentException()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Minor, 10);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => segment1 + segment2);
    }

    [Fact]
    public void SubtractionOperator_WithValidValues_ShouldReturnDifference()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 10);
        var segment2 = new Segment(SegmentType.Major, 5);

        // Act
        var result = segment1 - segment2;

        // Assert
        result.Value.Should().Be(5);
        result.SegmentType.Should().Be(SegmentType.Major);
    }

    [Fact]
    public void SubtractionOperator_ResultingInNegative_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var segment1 = new Segment(SegmentType.Major, 5);
        var segment2 = new Segment(SegmentType.Major, 10);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => segment1 - segment2);
    }

    [Fact]
    public void IncrementOperator_WithValidValue_ShouldIncreaseBy1()
    {
        // Arrange
        var segment = new Segment(SegmentType.Major, 5);

        // Act
        var result = ++segment;

        // Assert
        result.Value.Should().Be(6);
    }

    [Fact]
    public void IncrementOperator_AtMaxValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var segment = new Segment(SegmentType.Major, 99);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ++segment);
    }

    [Fact]
    public void DecrementOperator_WithValidValue_ShouldDecreaseBy1()
    {
        // Arrange
        var segment = new Segment(SegmentType.Major, 5);

        // Act
        var result = --segment;

        // Assert
        result.Value.Should().Be(4);
    }

    [Fact]
    public void DecrementOperator_AtMinValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var segment = new Segment(SegmentType.Major, 0);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => --segment);
    }

    [Fact]
    public void Default_ShouldReturnSegmentWithValue0()
    {
        // Act
        var segment = Segment.Default;

        // Assert
        segment.Value.Should().Be(0);
    }

    [Fact]
    public void AdditiveIdentity_ShouldReturnDefaultSegment()
    {
        // Act
        var segment = Segment.AdditiveIdentity;

        // Assert
        segment.Should().Be(Segment.Default);
    }
}
