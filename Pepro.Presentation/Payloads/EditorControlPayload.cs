using Pepro.Presentation.Enums;

namespace Pepro.Presentation.Payloads;

public record EditorControlPayload<ItemType>
{
    public required ItemType Item { get; init; }

    public required EditorMode Mode { get; init; }

    public Action? OnDataChanged { get; init; }
}
