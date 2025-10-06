using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class AccountPage : CrudTemplate
{
    public AccountPage()
    {
        InitializeComponent();
        accountDataGridView.SetupCellStyle();
        employeeColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

        searchButton.ApplyFlatStyleWithIcon("Search", ThemeColors.Text);
        reloadButton.ApplyFlatStyleWithIcon("Refresh", ThemeColors.Text);
        deleteButton.ApplyFlatStyleWithIcon("Trash", ThemeColors.Text);
        toggleActiveButton.ApplyFlatStyle();
        resetPasswordButton.ApplyFlatStyle();

        deleteButton.ApplyPermission("Account.Delete");
        toggleActiveButton.ApplyPermission("Account.Update");
        resetPasswordButton.ApplyPermission("Account.Update");
    }

    private void LoadAccounts()
    {
        accountDataGridView.DataSource = AccountBusiness
            .Instance.GetAccountViews()
            .ToList();
    }

    private void AccountPage_Load(object sender, EventArgs e)
    {
        LoadAccounts();
    }

    private void AccountDataGridView_CellClick(
        object sender,
        DataGridViewCellEventArgs e
    )
    {
        BindDataGridViewCellClick<AccountDto>(
            (DataGridView)sender,
            e,
            (item) =>
                toggleActiveButton.Text = item.IsActive
                    ? "Deactivate"
                    : "Activate"
        );
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        BindSearchButtonClick(
            searchTextBox.Text,
            accountDataGridView,
            AccountBusiness.Instance.SearchAccountViews
        );
    }

    private void ReloadButton_Click(object sender, EventArgs e)
    {
        LoadAccounts();
    }

    private void ResetPasswordButton_Click(object sender, EventArgs e)
    {
        if (!accountDataGridView.TryGetCurrentRow(out AccountDto? item))
        {
            return;
        }

        if (MessageBoxWrapper.Confirm() != DialogResult.Yes)
        {
            return;
        }

        int numberOfRowsAffected =
            AccountBusiness.Instance.ResetPasswordAccount(item.AccountId);
        MessageBoxWrapper.ShowInformation(
            "ResetPasswordSuccess",
            numberOfRowsAffected
        );
        LoadAccounts();
    }

    private void ToggleActiveButton_Click(object sender, EventArgs e)
    {
        if (!accountDataGridView.TryGetCurrentRow(out AccountDto? item))
        {
            return;
        }

        if (MessageBoxWrapper.Confirm() != DialogResult.Yes)
        {
            return;
        }

        int numberOfRowsAffected = AccountBusiness.Instance.ToggleActiveAccount(
            item.AccountId
        );
        MessageBoxWrapper.ShowInformation(
            item.IsActive ? "LockSuccess" : "UnlockSuccess",
            numberOfRowsAffected
        );
        LoadAccounts();
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        BindDeleteButtonClick<AccountDto>(
            accountDataGridView,
            (item) => AccountBusiness.Instance.DeleteAccount(item.AccountId),
            LoadAccounts
        );
    }
}
