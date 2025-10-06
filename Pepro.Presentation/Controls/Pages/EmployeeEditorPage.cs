using System.ComponentModel;
using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Molecules;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Interfaces;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class EmployeeEditorPage
    : EditorTemplate,
        IEditorUserControl<EmployeeDto>
{
    private EmployeeDto _item = null!;
    private EditorMode _mode;

    public EmployeeEditorPage()
    {
        InitializeComponent();

        saveButton.ApplyFlatStyle();

        taxCodeTextBoxField.ApplyPermission("Salary.Update", true);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EmployeeDto Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            firstNameTextBoxField.Text = _item.FirstName;
            middleNameTextBoxField.Text = _item.MiddleName;
            lastNameTextBoxField.Text = _item.LastName;
            dateOfBirthDateTimePicker.SetValue(_item.DateOfBirth);
            (
                _item.Gender switch
                {
                    true => maleRadioButton,
                    false => femaleRadioButton,
                    _ => otherRadioButton,
                }
            ).Checked = true;
            taxCodeTextBoxField.Text = _item.TaxCode;
            citizenIdTextBoxField.Text = _item.CitizenId;
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
                EditorMode.Create => "Create a new employee",
                EditorMode.Edit => "Edit employee",
                _ => throw new InvalidEnumArgumentException(
                    nameof(Mode),
                    (int)_mode,
                    typeof(EditorMode)
                ),
            };
        }
    }

    private void EmployeeEditorPage_Load(object sender, EventArgs e)
    {
        if (
            string.IsNullOrEmpty(positionTextBoxField.Text)
            || string.IsNullOrEmpty(salaryScaleTextBoxField.Text)
            || string.IsNullOrEmpty(salaryLevelTextBoxField.Text)
        )
        {
            lbCheck1.ForeColor = Color.Red;
            lbCheck2.ForeColor = Color.Red;
            lbCheck3.ForeColor = Color.Red;
            lbCheck4.ForeColor = Color.Red;
        }

        departmentComboBoxField.DisplayMember = nameof(DepartmentDto.Name);
        departmentComboBoxField.ValueMember = nameof(
            DepartmentDto.DepartmentId
        );

        positionComboBoxField.DisplayMember = nameof(PositionDto.Title);
        positionComboBoxField.ValueMember = nameof(PositionDto.PositionId);

        salaryScaleComboBoxField.DisplayMember = nameof(SalaryScaleDto.Name);
        salaryScaleComboBoxField.ValueMember = nameof(
            SalaryScaleDto.SalaryScaleId
        );

        salaryLevelComboBoxField.DisplayMember = nameof(SalaryLevelDto.Level);
        salaryLevelComboBoxField.ValueMember = nameof(
            SalaryLevelDto.SalaryLevelId
        );

        departmentComboBoxField.DataSource = DepartmentBusiness
            .Instance.GetDepartments()
            .ToList();
        positionComboBoxField.DataSource = PositionBusiness
            .Instance.GetPositions()
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
        salaryScaleComboBoxField.ExecuteWithoutEvent(
            nameof(ComboBoxField.SelectedIndexChanged),
            SalaryScaleComboBoxField_SelectedIndexChanged,
            () =>
                salaryScaleComboBoxField.DataSource = SalaryScaleBusiness
                    .Instance.GetSalaryScales()
                    .ToList()
        );

        departmentComboBoxField.SelectedIndex = -1;
        positionComboBoxField.SelectedIndex = -1;
        salaryScaleComboBoxField.SelectedIndex = -1;
    }

    private void SetupEditMode()
    {
        // FIXME: The condition `SalaryScaleId != 1` is not optimized.
        // It causes the `GetSalaryLevelsBySalaryScaleId` method to be called twice.
        // It does not affect functionality but may reduce performance slightly.
        salaryScaleComboBoxField.DataSource = SalaryScaleBusiness
            .Instance.GetSalaryScales()
            .ToList();

        departmentComboBoxField.SelectedValue = _item.DepartmentId;
        positionComboBoxField.SelectedValue = _item.PositionId;

        SalaryScaleDto? salaryScale =
            SalaryScaleBusiness.Instance.GetSalaryScaleBySalaryLevelId(
                _item.SalaryLevelId
            );
        if (salaryScale != null)
        {
            salaryScaleComboBoxField.SelectedValue = salaryScale.SalaryScaleId;
        }
        salaryLevelComboBoxField.SelectedValue = _item.SalaryLevelId;
    }

    private void SalaryScaleComboBoxField_SelectedIndexChanged(
        object? sender,
        EventArgs e
    )
    {
        if (salaryScaleComboBoxField.SelectedItem is not SalaryScaleDto dto)
        {
            salaryScaleTextBoxField.Text = "";
            return;
        }
        salaryScaleTextBoxField.Text = dto.Group;

        salaryLevelComboBoxField.DataSource = SalaryLevelBusiness
            .Instance.GetSalaryLevelsBySalaryScaleId(dto.SalaryScaleId)
            .ToList();
        salaryLevelComboBoxField.SelectedIndex = -1;
    }

    private void SalaryLevelComboBoxField_SelectedIndexChanged(
        object sender,
        EventArgs e
    )
    {
        if (salaryLevelComboBoxField.SelectedItem is not SalaryLevelDto dto)
        {
            salaryLevelTextBoxField.Text = "";
            return;
        }
        salaryLevelTextBoxField.Text = dto.Coefficient.ToString();
    }

    private void PositionComboBoxField_SelectedIndexChanged(
        object sender,
        EventArgs e
    )
    {
        if (positionComboBoxField.SelectedItem is not PositionDto dto)
        {
            positionTextBoxField.Text = "";
            return;
        }
        positionTextBoxField.Text = dto.AllowancePercent.ToString();
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
                positionComboBoxField.SelectedValue?.ToString(),
                out int positionId
            )
            || !int.TryParse(
                salaryLevelComboBoxField.SelectedValue?.ToString(),
                out int salaryLevelId
            )
            || !int.TryParse(
                departmentComboBoxField.SelectedValue?.ToString(),
                out int departmentId
            )
        )
        {
            return;
        }

        EmployeeDto employee = new()
        {
            EmployeeId = _item.EmployeeId,
            FirstName = firstNameTextBoxField.Text.Trim(),
            MiddleName = middleNameTextBoxField.Text.Trim(),
            LastName = lastNameTextBoxField.Text.Trim(),
            DateOfBirth = DateOnly.FromDateTime(
                dateOfBirthDateTimePicker.Value
            ),
            Gender =
                maleRadioButton.Checked ? true
                : femaleRadioButton.Checked ? false
                : null,
            TaxCode = taxCodeTextBoxField.Text.Trim(),
            CitizenId = citizenIdTextBoxField.Text.Trim(),
            DepartmentId = departmentId,
            PositionId = positionId,
            SalaryLevelId = salaryLevelId,
        };

        int result = _mode switch
        {
            EditorMode.Create => EmployeeBusiness.Instance.InsertEmployee(
                employee
            ),
            EditorMode.Edit => EmployeeBusiness.Instance.UpdateEmployee(
                employee
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
        return !string.IsNullOrWhiteSpace(citizenIdTextBoxField.Text)
            && !string.IsNullOrWhiteSpace(firstNameTextBoxField.Text)
            && !string.IsNullOrWhiteSpace(lastNameTextBoxField.Text)
            && !string.IsNullOrWhiteSpace(
                departmentComboBoxField.SelectedValue?.ToString()
            )
            && !string.IsNullOrWhiteSpace(
                positionComboBoxField.SelectedValue?.ToString()
            )
            && !string.IsNullOrWhiteSpace(
                salaryLevelComboBoxField.SelectedValue?.ToString()
            );
    }
}
