using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class AssignmentPage : CrudTemplate
{
    public AssignmentPage()
    {
        InitializeComponent();
        assignmentDataGridView.SetupCellStyle();
        managerColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

        searchButton.ApplyFlatStyleWithIcon("Search", ThemeColors.Text);
        reloadButton.ApplyFlatStyleWithIcon("Refresh", ThemeColors.Text);
        insertButton.ApplyFlatStyleWithIcon("Plus", ThemeColors.Text);
        deleteButton.ApplyFlatStyleWithIcon("Trash", ThemeColors.Text);
        updateButton.ApplyFlatStyleWithIcon("EditPencil", ThemeColors.Text);

        insertButton.ApplyPermission("Assignment.Create");
        deleteButton.ApplyPermission("Assignment.Delete");
        updateButton.ApplyPermission("Assignment.Update");
    }

    private void LoadAssignments()
    {
        assignmentDataGridView.DataSource = AssignmentBusiness
            .Instance.GetAssignmentViews()
            .ToList();
    }

    private void AssignmentPage_Load(object sender, EventArgs e)
    {
        LoadAssignments();
    }

    private void AssignmentDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        BindDataGridViewCellClick<AssignmentDto>((DataGridView)sender, e);
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        BindSearchButtonClick(
            searchTextBox.Text,
            assignmentDataGridView,
            AssignmentBusiness.Instance.SearchAssignmentViews
        );
    }

    private void ReloadButton_Click(object sender, EventArgs e)
    {
        LoadAssignments();
    }

    private void InsertButton_Click(object sender, EventArgs e)
    {
        BindInsertButtonClick<AssignmentDto>(
            new(),
            ControlUiEvent.PushAssignmentEditorPage,
            LoadAssignments
        );
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
        BindUpdateButtonClick<AssignmentDto>(
            assignmentDataGridView,
            ControlUiEvent.PushAssignmentEditorPage,
            LoadAssignments
        );
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        BindDeleteButtonClick<AssignmentDto>(
            assignmentDataGridView,
            (item) => AssignmentBusiness.Instance.DeleteAssignment(item.AssignmentId),
            LoadAssignments
        );
    }
}
