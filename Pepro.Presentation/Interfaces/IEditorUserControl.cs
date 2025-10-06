using Pepro.Presentation.Enums;

namespace Pepro.Presentation.Interfaces;

public interface IEditorUserControl<ItemType>
{
    ItemType Item { get; set; }

    EditorMode Mode { get; set; }
}
