namespace Domain.Utilities.Comparers;

internal class EntityEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, string> _keySelector;

    public EntityEqualityComparer(Func<T, string> keySelector)
    {
        _keySelector = keySelector;
    }

    public bool Equals(T? x, T? y)
    {
        if (x == null || y == null)
            return false;

        return EqualityComparer<string>.Default.Equals(_keySelector(x), _keySelector(y));
    }

    public int GetHashCode(T obj)
    {
        return obj == null ? 0 : _keySelector(obj).GetHashCode();
    }
}