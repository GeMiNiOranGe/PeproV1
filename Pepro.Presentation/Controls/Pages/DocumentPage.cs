using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class DocumentPage : CrudTemplate
{
    public DocumentPage()
    {
        InitializeComponent();
        documentDataGridView.SetupCellStyle();

        searchButton.ApplyFlatStyleWithIcon("Search", ThemeColors.Text);
        reloadButton.ApplyFlatStyleWithIcon("Refresh", ThemeColors.Text);
        insertButton.ApplyFlatStyleWithIcon("Plus", ThemeColors.Text);
        deleteButton.ApplyFlatStyleWithIcon("Trash", ThemeColors.Text);
        updateButton.ApplyFlatStyleWithIcon("EditPencil", ThemeColors.Text);
        downloadButton.ApplyFlatStyle();

        insertButton.ApplyPermission("Document.Create");
        deleteButton.ApplyPermission("Document.Delete");
        updateButton.ApplyPermission("Document.Update");
    }

    private void LoadDocuments()
    {
        documentDataGridView.DataSource = DocumentBusiness
            .Instance.GetDocuments()
            .ToList();
    }

    private void DocumentPage_Load(object sender, EventArgs e)
    {
        LoadDocuments();
    }

    private void DocumentDataGridView_CellClick(
        object sender,
        DataGridViewCellEventArgs e
    )
    {
        BindDataGridViewCellClick<DocumentDto>(
            (DataGridView)sender,
            e,
            (item) =>
            {
                documentIdTextBoxField.Text = item.DocumentId.ToString();
                assignmentIdTextBoxField.Text = item.AssignmentId.ToString();
                fileTypeTextBoxField.Text = item.NativeFileFormat;
            }
        );
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        BindSearchButtonClick(
            searchTextBox.Text,
            documentDataGridView,
            DocumentBusiness.Instance.SearchDocuments
        );
    }

    private void ReloadButton_Click(object sender, EventArgs e)
    {
        LoadDocuments();
    }

    private void InsertButton_Click(object sender, EventArgs e)
    {
        BindInsertButtonClick<DocumentDto>(
            new(),
            ControlUiEvent.PushDocumentEditorPage,
            LoadDocuments
        );
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
        BindUpdateButtonClick<DocumentDto>(
            documentDataGridView,
            ControlUiEvent.PushDocumentEditorPage,
            LoadDocuments
        );
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        BindDeleteButtonClick<DocumentDto>(
            documentDataGridView,
            (item) => DocumentBusiness.Instance.DeleteDocument(item.DocumentId),
            LoadDocuments
        );
    }

    private void DownloadButton_Click(object sender, EventArgs e)
    {
        MessageBoxWrapper.ShowInformation("TreasureFoundPremiumUnlock");
    }

    private void DocumentIdTextBoxField_TextChanged(object sender, EventArgs e)
    {
        if (!int.TryParse(documentIdTextBoxField.Text, out int documentId))
        {
            return;
        }

        AssignmentDto? assignment =
            AssignmentBusiness.Instance.GetAssignmentByDocumentId(documentId);
        if (assignment == null)
        {
            return;
        }

        assignmentNameTextBoxField.Text = assignment.Name;
    }

    private void AssignmentIdTextBoxField_TextChanged(
        object sender,
        EventArgs e
    )
    {
        if (!int.TryParse(assignmentIdTextBoxField.Text, out int assignmentId))
        {
            return;
        }

        ProjectDto? project = ProjectBusiness.Instance.GetProjectByAssignmentId(
            assignmentId
        );
        if (project == null)
        {
            return;
        }

        projectIdTextBoxField.Text = project.ProjectId.ToString();
        projectNameTextBoxField.Text = project.Name;
    }
}
