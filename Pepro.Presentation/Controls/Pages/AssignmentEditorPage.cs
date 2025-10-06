using System.ComponentModel;
using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Interfaces;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class AssignmentEditorPage
    : EditorTemplate,
        IEditorUserControl<AssignmentDto>
{
    private AssignmentDto _item = null!;
    private EditorMode _mode;

    public AssignmentEditorPage()
    {
        InitializeComponent();

        saveButton.ApplyFlatStyle();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AssignmentDto Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            assignmentNameTextBoxField.Text = _item.Name;
            isPublicToProjectCheckBox.Checked = _item.IsPublicToProject;
            isPublicToDepartmentCheckBox.Checked = _item.IsPublicToDepartment;
            startDateTimePicker.SetValue(_item.StartDate);
            endDateTimePicker.SetValue(_item.EndDate);
            requiredDocumentCountNumericUpDownField.Value =
                _item.RequiredDocumentCount;
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
                EditorMode.Create => "Create a new assignment",
                EditorMode.Edit => "Edit assignment",
                _ => throw new InvalidEnumArgumentException(
                    nameof(Mode),
                    (int)_mode,
                    typeof(EditorMode)
                ),
            };
        }
    }

    private void AssignmentEditorPage_Load(object sender, EventArgs e)
    {
        projectComboBoxField.DisplayMember = nameof(ProjectDto.Name);
        projectComboBoxField.ValueMember = nameof(ProjectDto.ProjectId);

        statusComboBoxField.DisplayMember = nameof(StatusDto.Name);
        statusComboBoxField.ValueMember = nameof(StatusDto.StatusId);

        managerComboBoxField.DisplayMember = nameof(EmployeeDto.FullName);
        managerComboBoxField.ValueMember = nameof(EmployeeDto.EmployeeId);

        projectComboBoxField.DataSource = ProjectBusiness
            .Instance.GetProjects()
            .ToList();
        statusComboBoxField.DataSource = StatusBusiness
            .Instance.GetStatuses()
            .ToList();

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
        projectComboBoxField.SelectedIndex = -1;
        statusComboBoxField.SelectedIndex = -1;
        managerComboBoxField.Enabled = false;
    }

    private void SetupEditMode()
    {
        projectComboBoxField.SelectedValue = _item.ProjectId;
        statusComboBoxField.SelectedValue = _item.StatusId;

        managerComboBoxField.DataSource = EmployeeBusiness
            .Instance.GetEmployeesByAssignmentId(_item.AssignmentId)
            .ToList();

        if (_item.ManagerId.HasValue)
        {
            managerComboBoxField.SelectedValue = _item.ManagerId.Value;
        }
        else
        {
            managerComboBoxField.SelectedIndex = -1;
        }
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        if (!ValidateInputs())
        {
            MessageBoxWrapper.ShowInformation("FillInformation");
            return;
        }

        if (
            !int.TryParse(
                projectComboBoxField.SelectedValue?.ToString(),
                out int projectId
            )
            || !int.TryParse(
                statusComboBoxField.SelectedValue?.ToString(),
                out int statusId
            )
        )
        {
            return;
        }

        int? managerId = managerComboBoxField.SelectedValue.ToNullableInt();

        AssignmentDto assignment = new()
        {
            AssignmentId = _item.AssignmentId,
            Name = assignmentNameTextBoxField.Text.Trim(),
            IsPublicToProject = isPublicToProjectCheckBox.Checked,
            IsPublicToDepartment = isPublicToDepartmentCheckBox.Checked,
            ManagerId = managerId,
            StartDate = startDateTimePicker.Value,
            EndDate = endDateTimePicker.Value,
            RequiredDocumentCount = (int)
                requiredDocumentCountNumericUpDownField.Value,
            ProjectId = projectId,
            StatusId = statusId,
        };

        int result = _mode switch
        {
            EditorMode.Create => AssignmentBusiness.Instance.InsertAssignment(
                assignment
            ),
            EditorMode.Edit => AssignmentBusiness.Instance.UpdateAssignment(
                assignment
            ),
            _ => throw new InvalidEnumArgumentException(
                nameof(Mode),
                (int)_mode,
                typeof(EditorMode)
            ),
        };

        if (result > 0)
        {
            NotifyDataChanged();
            Close();
            MessageBoxWrapper.ShowInformation("SaveSuccess");
        }
        else
        {
            MessageBoxWrapper.ShowError("SaveFailed");
        }
    }

    private bool ValidateInputs()
    {
        return !string.IsNullOrWhiteSpace(assignmentNameTextBoxField.Text)
            && !string.IsNullOrWhiteSpace(
                projectComboBoxField.SelectedValue?.ToString()
            )
            && !string.IsNullOrWhiteSpace(
                statusComboBoxField.SelectedValue?.ToString()
            );
    }
}
