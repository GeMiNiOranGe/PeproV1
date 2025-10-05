using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class EmployeePage : CrudTemplate
{
    public EmployeePage()
    {
        InitializeComponent();
        employeeDataGridView.SetupCellStyle();
        fullNameColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

        searchButton.ApplyFlatStyleWithIcon("Search", ThemeColors.Text);
        reloadButton.ApplyFlatStyleWithIcon("Refresh", ThemeColors.Text);
        insertButton.ApplyFlatStyleWithIcon("Plus", ThemeColors.Text);
        deleteButton.ApplyFlatStyleWithIcon("Trash", ThemeColors.Text);
        updateButton.ApplyFlatStyleWithIcon("EditPencil", ThemeColors.Text);
        exportButton.ApplyFlatStyle();

        insertButton.ApplyPermission("Employee.Create");
        deleteButton.ApplyPermission("Employee.Delete");
        updateButton.ApplyPermission("Employee.Update");
    }

    private void LoadEmployees()
    {
        List<EmployeeView> employees =
        [
            .. EmployeeBusiness.Instance.GetEmployeeViews()
        ];
        employeeDataGridView.DataSource = employees;
        numberOfEmployeesTextBoxField.Text = employees.Count.ToString();
    }

    private void EmployeePage_Load(object sender, EventArgs e)
    {
        LoadEmployees();
    }

    private void EmployeeDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        BindDataGridViewCellClick<EmployeeDto>(
            (DataGridView)sender,
            e,
            (item) =>
            {
                employeeIdTextBoxField.Text = item.EmployeeId.ToString();
                employeeNameTextBoxField.Text = item.FullName;
            }
        );
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        BindSearchButtonClick(
            searchTextBox.Text,
            employeeDataGridView,
            EmployeeBusiness.Instance.SearchEmployeeViews,
            (items) =>
                numberOfEmployeesTextBoxField.Text = items.Count().ToString()
        );
    }

    private void ReloadButton_Click(object sender, EventArgs e)
    {
        LoadEmployees();
    }

    private void InsertButton_Click(object sender, EventArgs e)
    {
        BindInsertButtonClick<EmployeeDto>(
            new(),
            ControlUiEvent.PushEmployeeEditorPage,
            LoadEmployees
        );
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
        BindUpdateButtonClick<EmployeeDto>(
            employeeDataGridView,
            ControlUiEvent.PushEmployeeEditorPage,
            LoadEmployees
        );
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        BindDeleteButtonClick<EmployeeDto>(
            employeeDataGridView,
            (item) => EmployeeBusiness.Instance.DeleteEmployee(item.EmployeeId),
            LoadEmployees
        );
    }

    private void ExportButton_Click(object sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog = new()
        {
            FileName = "Employees-" + DateTime.Now.ToString("ddMMyyyy-HHmmss"),
            Filter = "Excel Files|*.xlsx",
            Title = "Save Excel File"
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            ExcelExporter.Export(saveFileDialog.FileName, employeeDataGridView);
        }
    }
}
