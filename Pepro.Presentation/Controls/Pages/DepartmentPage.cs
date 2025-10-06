using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class DepartmentPage : CrudTemplate
{
    public DepartmentPage()
    {
        InitializeComponent();
        departmentDataGridView.SetupCellStyle();
        managerColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

        searchButton.ApplyFlatStyleWithIcon("Search", ThemeColors.Text);
        reloadButton.ApplyFlatStyleWithIcon("Refresh", ThemeColors.Text);
        insertButton.ApplyFlatStyleWithIcon("Plus", ThemeColors.Text);
        deleteButton.ApplyFlatStyleWithIcon("Trash", ThemeColors.Text);
        updateButton.ApplyFlatStyleWithIcon("EditPencil", ThemeColors.Text);

        insertButton.ApplyPermission("Department.Create");
        deleteButton.ApplyPermission("Department.Delete");
        updateButton.ApplyPermission("Department.Update");
    }

    private void LoadDepartments()
    {
        departmentDataGridView.DataSource = DepartmentBusiness
            .Instance.GetDepartmentViews()
            .ToList();
    }

    private void DepartmentPage_Load(object sender, EventArgs e)
    {
        LoadDepartments();
    }

    private void DepartmentDataGridView_CellClick(
        object sender,
        DataGridViewCellEventArgs e
    )
    {
        BindDataGridViewCellClick<DepartmentDto>(
            (DataGridView)sender,
            e,
            (item) =>
            {
                managerTextBoxField.Text = item.DepartmentId.ToString();
                departmentNameTextBoxField.Text = item.Name;
            }
        );
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        BindSearchButtonClick(
            searchTextBox.Text,
            departmentDataGridView,
            DepartmentBusiness.Instance.SearchDepartmentViews
        );
    }

    private void ReloadButton_Click(object sender, EventArgs e)
    {
        LoadDepartments();
    }

    private void InsertButton_Click(object sender, EventArgs e)
    {
        BindInsertButtonClick<DepartmentDto>(
            new(),
            ControlUiEvent.PushDepartmentEditorPage,
            LoadDepartments
        );
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
        BindUpdateButtonClick<DepartmentDto>(
            departmentDataGridView,
            ControlUiEvent.PushDepartmentEditorPage,
            LoadDepartments
        );
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        BindDeleteButtonClick<DepartmentDto>(
            departmentDataGridView,
            (item) =>
                DepartmentBusiness.Instance.DeleteDepartment(item.DepartmentId),
            LoadDepartments
        );
    }
}
