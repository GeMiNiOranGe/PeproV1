using System.ComponentModel;

namespace Pepro.Presentation.Controls.Templates;

public class EditorTemplate : MediatedTemplate
{
    private Action? _onDataChanged;

    public EditorTemplate() { }

    /// <summary>
    /// Gets or sets the callback that is invoked when internal data changes occur.
    /// </summary>
    /// <remarks>
    /// The assigned delegate is triggered by the <see cref="NotifyDataChanged"/> method.
    /// Typically, external components (such as parent forms, mediators, or controllers)
    /// assign a handler to this property to stay synchronized with user edits.
    /// </remarks>
    /// <example>
    /// <code>
    /// var editor = new MyCustomEditor();
    /// editor.OnDataChanged = () => RefreshSummaryPanel();
    /// </code>
    /// </example>
    [Browsable(false)]
    [Description(
        "The callback that is invoked when internal data changes occur."
    )]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Action? OnDataChanged
    {
        set => _onDataChanged = value;
    }

    /// <summary>
    /// Invokes the <see cref="OnDataChanged"/> callback to notify listeners that the editor’s data has changed.
    /// </summary>
    /// <remarks>
    /// Derived classes should call this method after user input or state changes that affect
    /// data integrity or synchronization.
    /// This pattern promotes loose coupling between UI components and data consumers.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="OnDataChanged"/> is not assigned before calling this method.
    /// </exception>
    protected void NotifyDataChanged()
    {
        // Ensure the OnDataChanged delegate is defined before invoking it.
        if (_onDataChanged == null)
        {
            throw new InvalidOperationException(
                $"{nameof(OnDataChanged)} must be assigned before calling {nameof(NotifyDataChanged)}."
            );
        }

        // Invoke the assigned callback to signal data updates.
        _onDataChanged.Invoke();
    }
}
