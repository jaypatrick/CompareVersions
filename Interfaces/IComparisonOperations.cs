namespace CompareVersions.Interfaces;


public interface IComparisonOperations<T>
    : IComparer<T>, IEqualityComparer<T>, IComparer, IEqualityComparer
{
    int Compare(string leftSide, string rightSide);

    public int Compare(T? leftSide, T? rightSide);

    public int Compare(object? leftSide, object? rightSide);

    public bool Equals(T? leftSide, T? rightSide);

    public new bool Equals(object? leftSide, object? rightSide);
}