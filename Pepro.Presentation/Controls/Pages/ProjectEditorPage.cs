using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Interfaces;
using Pepro.Presentation.Utilities;
using System.ComponentModel;

namespace Pepro.Presentation.Controls.Pages;

public partial class ProjectEditorPage : EditorTemplate, IEditorUserControl<ProjectDto>
{
    private ProjectDto _item = null!;
    private EditorMode _mode;

    public ProjectEditorPage()
    {
        InitializeComponent();

        saveButton.ApplyFlatStyle();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ProjectDto Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            projectNameTextBoxField.Text = _item.Name;
            customerNameTextBoxField.Text = _item.CustomerName;
            startDateTimePicker.SetValue(_item.StartDate);
            endDateTimePicker.SetValue(_item.EndDate);
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
                EditorMode.Create => "Create a new project",
                EditorMode.Edit => "Edit project",
                _ => throw new InvalidEnumArgumentException(nameof(Mode), (int)_mode, typeof(EditorMode)),
            };
        }
    }

    private void ProjectEditorPage_Load(object sender, EventArgs e)
    {
        statusComboBoxField.DisplayMember = nameof(StatusDto.Name);
        statusComboBoxField.ValueMember = nameof(StatusDto.StatusId);

        managerComboBoxField.DisplayMember = nameof(EmployeeDto.FullName);
        managerComboBoxField.ValueMember = nameof(EmployeeDto.EmployeeId);

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
        statusComboBoxField.SelectedIndex = -1;
        managerComboBoxField.Enabled = false;
    }

    private void SetupEditMode()
    {
        statusComboBoxField.SelectedValue = _item.StatusId;

        managerComboBoxField.DataSource = EmployeeBusiness
            .Instance.GetEmployeesByProjectId(_item.ProjectId)
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
                statusComboBoxField.SelectedValue?.ToString(),
                out int statusId
            )
        )
        {
            return;
        }

        int? managerId = managerComboBoxField.SelectedValue.ToNullableInt();

        ProjectDto project = new()
        {
            ProjectId = _item.ProjectId,
            Name = projectNameTextBoxField.Text.Trim(),
            CustomerName = customerNameTextBoxField.Text.Trim(),
            ManagerId = managerId,
            StartDate = startDateTimePicker.Value,
            EndDate = endDateTimePicker.Value,
            StatusId = statusId,
        };

        int result = _mode switch
        {
            EditorMode.Create => ProjectBusiness.Instance.InsertProject(project),
            EditorMode.Edit => ProjectBusiness.Instance.UpdateProject(project),
            _ => throw new InvalidEnumArgumentException(nameof(Mode), (int)_mode, typeof(EditorMode)),
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
        return !string.IsNullOrWhiteSpace(projectNameTextBoxField.Text)
            && !string.IsNullOrWhiteSpace(customerNameTextBoxField.Text)
            && !string.IsNullOrWhiteSpace(
                statusComboBoxField.SelectedValue?.ToString()
            );
    }
}
