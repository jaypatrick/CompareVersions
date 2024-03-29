﻿using Version = CompareVersions.Models.Version;

namespace CompareVersions.Interfaces;

/// <summary>
///     Interface that does comparison operations on for <see cref="Version"/>version strings
/// </summary>
/// <typeparam name="T">T is <see cref="Version"/></typeparam>
/// <seealso cref="System.Collections.Generic.IComparer{T}" />
/// <seealso cref="System.Collections.Generic.IEqualityComparer{T}" />
/// <seealso cref="System.Collections.IComparer" />
/// <seealso cref="System.Collections.IEqualityComparer" />
public interface IComparisonOperations<in T>
    : IComparer<T>, IEqualityComparer<T>, IComparer, IEqualityComparer
{
    /// <summary>
    /// Compares the specified left side.
    /// </summary>
    /// <param name="leftSide">The left side.</param>
    /// <param name="rightSide">The right side.</param>
    /// <returns></returns>
    int Compare(string leftSide, string rightSide);

    /// <summary>
    /// Compares the specified left side.
    /// </summary>
    /// <param name="leftSide">The left side.</param>
    /// <param name="rightSide">The right side.</param>
    /// <returns></returns>
    public int Compare(T? leftSide, T? rightSide);

    /// <summary>
    /// Compares the specified left side.
    /// </summary>
    /// <param name="leftSide">The left side.</param>
    /// <param name="rightSide">The right side.</param>
    /// <returns></returns>
    public int Compare(object? leftSide, object? rightSide);

    /// <summary>
    /// Equals the specified left side.
    /// </summary>
    /// <param name="leftSide">The left side.</param>
    /// <param name="rightSide">The right side.</param>
    /// <returns></returns>
    public bool Equals(T? leftSide, T? rightSide);

    /// <summary>
    /// Equals the specified left side.
    /// </summary>
    /// <param name="leftSide">The left side.</param>
    /// <param name="rightSide">The right side.</param>
    /// <returns></returns>
    public new bool Equals(object? leftSide, object? rightSide);
}