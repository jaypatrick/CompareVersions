namespace CompareVersions.Interfaces;

public interface ISegmentable<T> :
    IAdditiveIdentity<T, T>,
    IAdditionOperators<T, T, T>,
    ISubtractionOperators<T, T, T>,
    IIncrementOperators<T>,
    IDecrementOperators<T>
    where T : class, ISegmentable<T>
{
    /// <summary>
    /// Gets the additive identity.
    /// </summary>
    /// <value>
    /// The additive identity.
    /// </value>
    public static abstract T AdditiveIdentity { get; }

    /// <summary>
    /// Implements the operator op_Addition.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static abstract T operator +(T left, T right);

    /// <summary>
    /// Implements the operator op_Subtraction.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static abstract T operator -(T left, T right);

    /// <summary>
    /// Implements the operator op_Increment.
    /// </summary>
    /// <param name="self">The self.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static abstract T operator ++(T self);

    /// <summary>
    /// Implements the operator op_Decrement.
    /// </summary>
    /// <param name="self">The self.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static abstract T operator --(T self);


}