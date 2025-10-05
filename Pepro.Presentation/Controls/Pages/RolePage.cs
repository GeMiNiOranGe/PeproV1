using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class RolePage : CrudTemplate
{
    public RolePage()
    {
        InitializeComponent();
        roleDataGridView.SetupCellStyle();

        searchButton.ApplyFlatStyleWithIcon("Search", ThemeColors.Text);
        reloadButton.ApplyFlatStyleWithIcon("Refresh", ThemeColors.Text);
        insertButton.ApplyFlatStyleWithIcon("Plus", ThemeColors.Text);
        deleteButton.ApplyFlatStyleWithIcon("Trash", ThemeColors.Text);
        updateButton.ApplyFlatStyleWithIcon("EditPencil", ThemeColors.Text);

        insertButton.ApplyPermission("Role.Create");
        deleteButton.ApplyPermission("Role.Delete");
        updateButton.ApplyPermission("Role.Update");
    }

    private void LoadRoles()
    {
        roleDataGridView.DataSource = RoleBusiness
            .Instance.GetRoles()
            .ToList();
    }

    private void RolePage_Load(object sender, EventArgs e)
    {
        LoadRoles();
    }

    private void RoleDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        BindDataGridViewCellClick<RoleDto>((DataGridView)sender, e);
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        BindSearchButtonClick(
            searchTextBox.Text,
            roleDataGridView,
            RoleBusiness.Instance.SearchRoles
        );
    }

    private void ReloadButton_Click(object sender, EventArgs e)
    {
        LoadRoles();
    }

    private void InsertButton_Click(object sender, EventArgs e)
    {
        BindInsertButtonClick<RoleDto>(
            new(),
            ControlUiEvent.PushRoleEditorPage,
            LoadRoles
        );
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
        BindUpdateButtonClick<RoleDto>(
            roleDataGridView,
            ControlUiEvent.PushRoleEditorPage,
            LoadRoles
        );
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        BindDeleteButtonClick<RoleDto>(
            roleDataGridView,
            (item) => RoleBusiness.Instance.DeleteRole(item.RoleId),
            LoadRoles
        );
    }
}
