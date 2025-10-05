using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Molecules;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Interfaces;
using Pepro.Presentation.Utilities;
using System.ComponentModel;

namespace Pepro.Presentation.Controls.Pages;

public partial class DocumentEditorPage : EditorTemplate, IEditorUserControl<DocumentDto>
{
    private DocumentDto _item = null!;
    private EditorMode _mode;

    public DocumentEditorPage()
    {
        InitializeComponent();

        saveButton.ApplyFlatStyle();
        browseButton.ApplyFlatStyle();

        projectIdTextBoxField.FocusColor = ThemeColors.Accent.Base;
        assignmentIdTextBoxField.FocusColor = ThemeColors.Accent.Base;
        documentIdTextBoxField.FocusColor = ThemeColors.Accent.Base;
        titleTextBoxField.FocusColor = ThemeColors.Accent.Base;
        preparedByTextBoxField.FocusColor = ThemeColors.Accent.Base;
        checkedByTextBoxField.FocusColor = ThemeColors.Accent.Base;
        approvedByTextBoxField.FocusColor = ThemeColors.Accent.Base;
        fileTypeTextBoxField.FocusColor = ThemeColors.Accent.Base;
        revisionNumberTextBoxField.FocusColor = ThemeColors.Accent.Base;
        revisionStatusTextBoxField.FocusColor = ThemeColors.Accent.Base;
        filePathTextBoxField.FocusColor = ThemeColors.Accent.Base;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DocumentDto Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            documentIdTextBoxField.Text = _item.DocumentId.ToString();
            titleTextBoxField.Text = _item.Title;
            createdAtDateTimePicker.SetValue(_item.CreateAt);
            revisionNumberTextBoxField.Text = _item.RevisionNumber.ToString();
            revisionStatusTextBoxField.Text = _item.RevisionStatus;
            filePathTextBoxField.Text = _item.DocumentUrl;
            fileTypeTextBoxField.Text = _item.NativeFileFormat;
            preparedByTextBoxField.Text = _item.PreparedBy;
            checkedByTextBoxField.Text = _item.CheckedBy;
            approvedByTextBoxField.Text = _item.ApprovedBy;
            assignmentIdTextBoxField.Text = _item.AssignmentId.ToString();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EditorMode Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            HeaderText = _mode switch
            {
                EditorMode.Create => "Create a new document",
                EditorMode.Edit => "Edit document",
                _ => throw new InvalidEnumArgumentException(nameof(Mode), (int)_mode, typeof(EditorMode)),
            };
        }
    }

    private void DocumentEditorPage_Load(object sender, EventArgs e)
    {
        projectIdTextBoxField.BindTextToValue(projectNameComboBoxField);
        assignmentIdTextBoxField.BindTextToValue(assignmentNameComboBoxField);

        projectNameComboBoxField.DisplayMember = nameof(ProjectDto.Name);
        projectNameComboBoxField.ValueMember = nameof(ProjectDto.ProjectId);

        assignmentNameComboBoxField.DisplayMember = nameof(AssignmentDto.Name);
        assignmentNameComboBoxField.ValueMember = nameof(AssignmentDto.AssignmentId);

        switch (_mode)
        {
            case EditorMode.Create:
            {
                SetupCreateMode();
                break;
            }
            case EditorMode.Edit:
            {
                SetupEditMode();
                break;
            }
            default:
            {
                throw new InvalidEnumArgumentException(
                    nameof(Mode),
                    (int)_mode,
                    typeof(EditorMode)
                );
            }
        }
    }

    private void SetupCreateMode()
    {
        projectNameComboBoxField.DataSource = ProjectBusiness
            .Instance.GetProjects()
            .ToList();
        projectNameComboBoxField.SelectedIndex = -1;
    }

    private void SetupEditMode()
    {
        assignmentNameComboBoxField.Enabled = false;
        projectNameComboBoxField.Enabled = false;

        if (!int.TryParse(documentIdTextBoxField.Text, out int documentId))
        {
            return;
        }

        AssignmentDto? assignment = AssignmentBusiness.Instance.GetAssignmentByDocumentId(documentId);
        if (assignment == null)
        {
            return;
        }

        assignmentNameComboBoxField.DataSource = (List<AssignmentDto>)[assignment];

        ProjectDto? project = ProjectBusiness.Instance.GetProjectByAssignmentId(assignment.AssignmentId);
        if (project == null)
        {
            return;
        }

        projectNameComboBoxField.ExecuteWithoutEvent(
            nameof(ComboBoxField.SelectedIndexChanged),
            ProjectNameComboBoxField_SelectedIndexChanged,
            () => projectNameComboBoxField.DataSource = (List<ProjectDto>)[project]
        );
    }

    private void ProjectNameComboBoxField_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!int.TryParse(projectNameComboBoxField.SelectedValue?.ToString(), out int projectId))
        {
            return;
        }

        assignmentNameComboBoxField.DataSource = AssignmentBusiness
            .Instance.GetAssignmentsByProjectId(projectId)
            .ToList();
        assignmentNameComboBoxField.SelectedIndex = -1;
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        MessageBoxWrapper.ShowInformation("TreasureFoundPremiumUnlock2");
        NotifyDataChanged();
        Close();
    }

    private void BrowseButton_Click(object sender, EventArgs e)
    {
        MessageBoxWrapper.ShowInformation("TreasureFoundPremiumUnlock1");
    }
}
