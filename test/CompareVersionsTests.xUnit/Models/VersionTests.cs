using FluentAssertions;
using Xunit;
using Version = CompareVersions.Models.Version;

namespace CompareVersions.Models.Tests.xUnit;

public class VersionTests
{
    #region Constructor Tests

    [Fact]
    public void Version_DefaultConstructor_ShouldCreate0000Version()
    {
        // Act
        var version = new Version();

        // Assert
        version.ToString().Should().Be("0.0.0.0");
    }

    [Fact]
    public void Version_ConstructorWithIntegers_ShouldCreateVersion()
    {
        // Act
        var version = new Version(1, 2, 3, 4);

        // Assert
        version.MajorSegment.Value.Should().Be(1);
        version.MinorSegment.Value.Should().Be(2);
        version.PatchSegment.Value.Should().Be(3);
        version.BuildSegment.Value.Should().Be(4);
    }

    [Fact]
    public void Version_ConstructorWithString_ShouldParseVersion()
    {
        // Act
        var version = new Version("1.2.3.4", '.');

        // Assert
        version.ToString().Should().Be("1.2.3.4");
    }

    [Fact]
    public void Version_ConstructorWithSegments_ShouldCreateVersion()
    {
        // Arrange
        var segments = new List<Segment>
        {
            new(SegmentType.Major, 1),
            new(SegmentType.Minor, 2),
            new(SegmentType.Patch, 3),
            new(SegmentType.Build, 4)
        };

        // Act
        var version = new Version(segments);

        // Assert
        version.ToString().Should().Be("1.2.3.4");
    }

    #endregion

    #region Parsing Tests

    [Fact]
    public void Parse_ValidVersionString_ShouldReturnVersion()
    {
        // Act
        var version = Version.Parse("1.2.3.4");

        // Assert
        version.ToString().Should().Be("1.2.3.4");
    }

    [Fact]
    public void Parse_InvalidVersionString_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Version.Parse("invalid"));
    }

    [Fact]
    public void TryParse_ValidVersionString_ShouldReturnTrueAndVersion()
    {
        // Act
        var success = Version.TryParse("1.2.3.4", out var version);

        // Assert
        success.Should().BeTrue();
        version.Should().NotBeNull();
        version!.ToString().Should().Be("1.2.3.4");
    }

    [Fact]
    public void TryParse_InvalidVersionString_ShouldReturnFalse()
    {
        // Act
        var success = Version.TryParse("invalid", out var version);

        // Assert
        success.Should().BeFalse();
        version.Should().BeNull();
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_Default_ShouldReturnAllFourSegments()
    {
        // Arrange
        var version = new Version(1, 2, 3, 4);

        // Act
        var result = version.ToString();

        // Assert
        result.Should().Be("1.2.3.4");
    }

    [Fact]
    public void ToString_WithFieldCount_ShouldReturnSpecifiedSegments()
    {
        // Arrange
        var version = new Version(1, 2, 3, 4);

        // Act
        var result = version.ToString(2);

        // Assert
        result.Should().Be("1.2");
    }

    #endregion

    #region Comparison Tests

    [Fact]
    public void Equals_SameVersions_ShouldReturnTrue()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 4);

        // Act
        var result = version1.Equals(version2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentVersions_ShouldReturnFalse()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 5);

        // Act
        var result = version1.Equals(version2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CompareTo_LesserVersion_ShouldReturnNegative()
    {
        // Arrange
        var version1 = new Version(1, 0, 0, 0);
        var version2 = new Version(2, 0, 0, 0);

        // Act
        var result = version1.CompareTo(version2);

        // Assert
        result.Should().BeNegative();
    }

    [Fact]
    public void CompareTo_GreaterVersion_ShouldReturnPositive()
    {
        // Arrange
        var version1 = new Version(2, 0, 0, 0);
        var version2 = new Version(1, 0, 0, 0);

        // Act
        var result = version1.CompareTo(version2);

        // Assert
        result.Should().BePositive();
    }

    [Fact]
    public void CompareTo_EqualVersions_ShouldReturn0()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 4);

        // Act
        var result = version1.CompareTo(version2);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region Operator Tests

    [Fact]
    public void EqualityOperator_SameVersions_ShouldReturnTrue()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 4);

        // Act
        var result = version1 == version2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void InequalityOperator_DifferentVersions_ShouldReturnTrue()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 5);

        // Act
        var result = version1 != version2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void LessThanOperator_ShouldReturnCorrectResult()
    {
        // Arrange
        var version1 = new Version(1, 0, 0, 0);
        var version2 = new Version(2, 0, 0, 0);

        // Act
        var result = version1 < version2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GreaterThanOperator_ShouldReturnCorrectResult()
    {
        // Arrange
        var version1 = new Version(2, 0, 0, 0);
        var version2 = new Version(1, 0, 0, 0);

        // Act
        var result = version1 > version2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void LessThanOrEqualOperator_WithEqual_ShouldReturnTrue()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 4);

        // Act
        var result = version1 <= version2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithEqual_ShouldReturnTrue()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 4);

        // Act
        var result = version1 >= version2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AdditionOperator_ShouldAddMatchingSegments()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 1, 1, 1);

        // Act
        var result = version1 + version2;

        // Assert
        result.MajorSegment.Value.Should().Be(2);
        result.MinorSegment.Value.Should().Be(3);
        result.PatchSegment.Value.Should().Be(4);
        result.BuildSegment.Value.Should().Be(5);
    }

    [Fact]
    public void SubtractionOperator_ShouldSubtractMatchingSegments()
    {
        // Arrange
        var version1 = new Version(5, 5, 5, 5);
        var version2 = new Version(1, 2, 3, 4);

        // Act
        var result = version1 - version2;

        // Assert
        result.MajorSegment.Value.Should().Be(4);
        result.MinorSegment.Value.Should().Be(3);
        result.PatchSegment.Value.Should().Be(2);
        result.BuildSegment.Value.Should().Be(1);
    }

    #endregion

    #region Segment Access Tests

    [Fact]
    public void Indexer_ByPosition_ShouldReturnCorrectSegment()
    {
        // Arrange
        var version = new Version(1, 2, 3, 4);

        // Act
        var major = version[0];
        var minor = version[1];
        var patch = version[2];
        var build = version[3];

        // Assert
        major.Value.Should().Be(1);
        minor.Value.Should().Be(2);
        patch.Value.Should().Be(3);
        build.Value.Should().Be(4);
    }

    [Fact]
    public void Indexer_BySegmentType_ShouldReturnCorrectSegment()
    {
        // Arrange
        var version = new Version(1, 2, 3, 4);

        // Act
        var major = version[SegmentType.Major];
        var minor = version[SegmentType.Minor];
        var patch = version[SegmentType.Patch];
        var build = version[SegmentType.Build];

        // Assert
        major.Value.Should().Be(1);
        minor.Value.Should().Be(2);
        patch.Value.Should().Be(3);
        build.Value.Should().Be(4);
    }

    [Fact]
    public void AddSegment_ShouldAddSegmentToList()
    {
        // Arrange
        var version = new Version();
        var newSegment = new Segment(SegmentType.Major, 5);

        // Act
        version.AddSegment(newSegment);

        // Assert
        version.MajorSegment.Value.Should().Be(5);
    }

    [Fact]
    public void ReplaceSegment_ShouldReplaceExistingSegment()
    {
        // Arrange
        var version = new Version(1, 2, 3, 4);
        var newSegment = new Segment(SegmentType.Major, 10);

        // Act
        version.ReplaceSegment(SegmentType.Major, newSegment);

        // Assert
        version.MajorSegment.Value.Should().Be(10);
    }

    [Fact]
    public void RemoveSegment_ShouldRemoveSegmentByType()
    {
        // Arrange
        var version = new Version(1, 2, 3, 4);

        // Act
        version.RemoveSegment(SegmentType.Build);

        // Assert
        version.BuildSegment.Value.Should().Be(-1);
    }

    #endregion

    #region Clone and Enumeration Tests

    [Fact]
    public void Clone_ShouldCreateDeepCopy()
    {
        // Arrange
        var original = new Version(1, 2, 3, 4);

        // Act
        var clone = original.Clone();

        // Assert
        clone.Should().NotBeSameAs(original);
        clone.Should().Be(original);
        clone.ToString().Should().Be(original.ToString());
    }

    [Fact]
    public void GetEnumerator_ShouldEnumerateAllSegments()
    {
        // Arrange
        var version = new Version(1, 2, 3, 4);

        // Act
        var segments = version.ToList();

        // Assert
        segments.Should().HaveCount(4);
        segments[0].Value.Should().Be(1);
        segments[1].Value.Should().Be(2);
        segments[2].Value.Should().Be(3);
        segments[3].Value.Should().Be(4);
    }

    #endregion

    #region HashCode Tests

    [Fact]
    public void GetHashCode_ForEqualVersions_ShouldReturnSameHash()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 4);

        // Act
        var hash1 = version1.GetHashCode();
        var hash2 = version2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void GetHashCode_ForDifferentVersions_ShouldReturnDifferentHash()
    {
        // Arrange
        var version1 = new Version(1, 2, 3, 4);
        var version2 = new Version(1, 2, 3, 5);

        // Act
        var hash1 = version1.GetHashCode();
        var hash2 = version2.GetHashCode();

        // Assert
        hash1.Should().NotBe(hash2);
    }

    #endregion

    #region Random Version Tests

    [Fact]
    public void CreateRandom_ShouldGenerateValidVersion()
    {
        // Act
        var version = Version.CreateRandom();

        // Assert
        version.Should().NotBeNull();
        version.MajorSegment.Value.Should().BeInRange(0, 99);
        version.MinorSegment.Value.Should().BeInRange(0, 99);
        version.PatchSegment.Value.Should().BeInRange(0, 99);
        version.BuildSegment.Value.Should().BeInRange(0, 99);
    }

    #endregion

    #region Property Tests

    [Fact]
    public void MajorSegment_ShouldReturnCorrectSegment()
    {
        // Arrange
        var version = new Version(10, 20, 30, 40);

        // Act & Assert
        version.MajorSegment.Value.Should().Be(10);
        version.MajorSegment.SegmentType.Should().Be(SegmentType.Major);
    }

    [Fact]
    public void MinorSegment_ShouldReturnCorrectSegment()
    {
        // Arrange
        var version = new Version(10, 20, 30, 40);

        // Act & Assert
        version.MinorSegment.Value.Should().Be(20);
        version.MinorSegment.SegmentType.Should().Be(SegmentType.Minor);
    }

    [Fact]
    public void PatchSegment_ShouldReturnCorrectSegment()
    {
        // Arrange
        var version = new Version(10, 20, 30, 40);

        // Act & Assert
        version.PatchSegment.Value.Should().Be(30);
        version.PatchSegment.SegmentType.Should().Be(SegmentType.Patch);
    }

    [Fact]
    public void BuildSegment_ShouldReturnCorrectSegment()
    {
        // Arrange
        var version = new Version(10, 20, 30, 40);

        // Act & Assert
        version.BuildSegment.Value.Should().Be(40);
        version.BuildSegment.SegmentType.Should().Be(SegmentType.Build);
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void Version_WithThreeSegments_ShouldHandleCorrectly()
    {
        // Act
        var version = Version.Parse("1.2.3");

        // Assert
        version.MajorSegment.Value.Should().Be(1);
        version.MinorSegment.Value.Should().Be(2);
        version.PatchSegment.Value.Should().Be(3);
    }

    [Fact]
    public void Version_WithTwoSegments_ShouldHandleCorrectly()
    {
        // Act
        var version = Version.Parse("1.2");

        // Assert
        version.MajorSegment.Value.Should().Be(1);
        version.MinorSegment.Value.Should().Be(2);
    }

    [Fact]
    public void AdditiveIdentity_ShouldReturnZeroVersion()
    {
        // Act
        var identity = Version.AdditiveIdentity;

        // Assert
        identity.ToString().Should().Be("0.0.0.0");
    }

    #endregion
}
