using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Payloads;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Templates;

public class CrudTemplate : MediatedTemplate
{
    public CrudTemplate() { }

    protected static void BindDataGridViewCellClick<ItemType>(
        DataGridView dataGridView,
        DataGridViewCellEventArgs e,
        Action<ItemType>? onCellClicked = null
    )
    {
        int selectedRowIndex = e.RowIndex;

        // Ignore clicks on the column header or invalid row indices
        if (selectedRowIndex < 0 || selectedRowIndex >= dataGridView.Rows.Count)
        {
            return;
        }

        if (dataGridView.Rows[selectedRowIndex].DataBoundItem is ItemType item)
        {
            onCellClicked?.Invoke(item);
        }
    }

    protected static void BindSearchButtonClick<ItemType>(
        string keyword,
        DataGridView dataGridView,
        Func<string, IEnumerable<ItemType>> onSearch,
        Action<IEnumerable<ItemType>>? onSearchCompleted = null
    )
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return;
        }
        IEnumerable<ItemType> items = onSearch(keyword);
        dataGridView.DataSource = items.ToList();
        onSearchCompleted?.Invoke(items);
    }

    protected void BindInsertButtonClick<ItemType>(
        ItemType item,
        ControlUiEvent uiEvent,
        Action? onDataChanged
    )
    {
        Mediator?.Notify(
            this,
            uiEvent,
            new EditorControlPayload<ItemType>
            {
                Item = item,
                Mode = EditorMode.Create,
                OnDataChanged = onDataChanged,
            }
        );
    }

    protected void BindUpdateButtonClick<ItemType>(
        DataGridView dataGridView,
        ControlUiEvent uiEvent,
        Action? onDataChanged
    )
    {
        if (!dataGridView.TryGetCurrentRow(out ItemType? item))
        {
            return;
        }

        Mediator?.Notify(
            this,
            uiEvent,
            new EditorControlPayload<ItemType>
            {
                Item = item,
                Mode = EditorMode.Edit,
                OnDataChanged = onDataChanged
            }
        );
    }

    protected static void BindDeleteButtonClick<ItemType>(
        DataGridView dataGridView,
        Func<ItemType, int> onDelete,
        Action? onDataChanged
    )
    {
        if (!dataGridView.TryGetCurrentRow(out ItemType? item))
        {
            return;
        }

        if (MessageBoxWrapper.Confirm("ConfirmDelete") != DialogResult.Yes)
        {
            return;
        }

        int numberOfRowsAffected = onDelete(item);
        MessageBoxWrapper.ShowInformation(
            "DeleteSuccess",
            numberOfRowsAffected
        );
        onDataChanged?.Invoke();
    }
}
