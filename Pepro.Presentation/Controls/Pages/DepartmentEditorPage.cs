using System.ComponentModel;
using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Interfaces;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class DepartmentEditorPage
    : EditorTemplate,
        IEditorUserControl<DepartmentDto>
{
    private DepartmentDto _item = null!;
    private EditorMode _mode;

    public DepartmentEditorPage()
    {
        InitializeComponent();

        saveButton.ApplyFlatStyle();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DepartmentDto Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            departmentNameTextBoxField.Text = _item.Name;
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
                EditorMode.Create => "Create a new department",
                EditorMode.Edit => "Edit department",
                _ => throw new InvalidEnumArgumentException(
                    nameof(Mode),
                    (int)_mode,
                    typeof(EditorMode)
                ),
            };
        }
    }

    private void DepartmentEditorPage_Load(object sender, EventArgs e)
    {
        managerComboBoxField.DisplayMember = nameof(EmployeeDto.FullName);
        managerComboBoxField.ValueMember = nameof(EmployeeDto.EmployeeId);

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
        managerComboBoxField.Enabled = false;
    }

    private void SetupEditMode()
    {
        managerComboBoxField.DataSource = EmployeeBusiness
            .Instance.GetEmployeesByDepartmentId(_item.DepartmentId)
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

        int? managerId = managerComboBoxField.SelectedValue.ToNullableInt();

        DepartmentDto department = new()
        {
            DepartmentId = _item.DepartmentId,
            Name = departmentNameTextBoxField.Text,
            ManagerId = managerId,
        };

        int result = _mode switch
        {
            EditorMode.Create => DepartmentBusiness.Instance.InsertDepartment(
                department
            ),
            EditorMode.Edit => DepartmentBusiness.Instance.UpdateDepartment(
                department
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
        return !string.IsNullOrWhiteSpace(departmentNameTextBoxField.Text);
    }
}
