namespace Pepro.DataAccess.Contracts;

/// <summary>
/// Represents a value with an associated modification state, typically used to track changes for update operations.
/// </summary>
/// <typeparam name="T">
/// The type of the tracked value.
/// </typeparam>
public class TrackedValue<T>
{
    private readonly T? _value;
    private readonly bool _isModified;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackedValue{T}"/> class with default values.
    /// </summary>
    public TrackedValue() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackedValue{T}"/> class with the specified value and modification state.
    /// </summary>
    /// <param name="value">
    /// The tracked value.
    /// </param>
    /// <param name="isModified">
    /// Indicates whether the value has been modified.
    /// </param>
    public TrackedValue(T value, bool isModified)
    {
        _value = value;
        _isModified = isModified;
    }

    /// <summary>
    /// Gets the tracked value.
    /// </summary>
    public T? Value => _value;

    /// <summary>
    /// Gets a value indicating whether the tracked value has been modified.
    /// </summary>
    public bool IsModified => _isModified;
}
