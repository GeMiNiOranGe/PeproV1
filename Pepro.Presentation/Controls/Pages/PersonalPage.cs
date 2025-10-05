using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Extensions;
using System.ComponentModel;

namespace Pepro.Presentation.Controls.Pages;

public partial class PersonalPage : MediatedTemplate
{
    private int _employeeId;

    public PersonalPage()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public required int EmployeeId
    {
        get => _employeeId;
        set => _employeeId = value;
    }

    private void PersonalPage_Load(object sender, EventArgs e)
    {
        EmployeeDto? employee = EmployeeBusiness.Instance.GetEmployeeByEmployeeId(_employeeId);
        if (employee == null)
        {
            MessageBox.Show("Employee not found");
            return;
        }

        employeeIdTextBoxField.Text = employee.EmployeeId.ToString();
        fullNameTextBoxField.Text = employee.FullName;
        (
            employee.Gender switch
            {
                true => maleRadioButton,
                false => femaleRadioButton,
                _ => otherRadioButton,
            }
        ).Checked = true;
        dateOfBirthDateTimePicker.SetValue(employee.DateOfBirth);
        citizenIdTextBoxField.Text = employee.CitizenId;

        positionTextBoxField.Text = PositionBusiness.Instance.GetPositionTitleByEmployeeId(_employeeId);

        DepartmentDto? department = DepartmentBusiness.Instance.GetDepartmentByDepartmentId(employee.DepartmentId);
        if (department != null)
        {
            departmentTextBoxField.Text = department.Name;
        }
        else
        {
            departmentTextBoxField.Text = "Department not found";
        }

        string[] projectNames = ProjectBusiness.Instance.GetProjectNamesByEmployeeId(_employeeId);
        assignedProjectsComboBoxField.DataSource = projectNames.Length != 0 ? projectNames : ["Không tìm thấy"];

        string[] phoneNumbers = EmployeeBusiness.Instance.GetPhoneNumbersByEmployeeId(_employeeId);
        phoneNumberComboBoxField.DataSource = phoneNumbers.Length != 0 ? phoneNumbers : ["Không tìm thấy"];
    }
}
