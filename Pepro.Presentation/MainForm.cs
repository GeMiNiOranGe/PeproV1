using System.ComponentModel;
using Pepro.Business;
using Pepro.Presentation.Controls.Atoms;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Payloads;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation;

public partial class MainForm : PeproForm
{
    private readonly string _defaultText;
    private readonly ControlUiMediator _mediator;
    private int _employeeId;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public required int EmployeeId
    {
        get => _employeeId;
        set => _employeeId = value;
    }

    public MainForm()
    {
        InitializeComponent();
        _mediator = new ControlUiMediator(workplacePanel);
        _defaultText = Text;

        assignmentButton.SetSidebarButtonImages("Task");
        projectButton.SetSidebarButtonImages("Folder");
        documentButton.SetSidebarButtonImages("MultiplePages");
        progressButton.SetSidebarButtonImages("DoughnutChart");
        employeeButton.SetSidebarButtonImages("User");
        attendanceButton.SetSidebarButtonImages("CalendarCheck");
        payrollButton.SetSidebarButtonImages("UserSalary");
        dormitoryButton.SetSidebarButtonImages("BunkBed");
        departmentButton.SetSidebarButtonImages("Group");
        accountButton.SetSidebarButtonImages("UserSetting");
        roleButton.SetSidebarButtonImages("Shield");

        logoutButton.ApplyFlatStyleNoBackColor();
        logoutButton.ApplyIcon("LogOut", color: ThemeColors.Text);

        appNameLabel.Image = IconProvider.GetLogo();

        imageUserPictureBox.Image = IconProvider.GetIcon(
            "Profile",
            color: Color.White,
            size: 48
        );

        assignmentButton.ApplyPermission("Assignment.Read");
        projectButton.ApplyPermission("Project.Read");
        documentButton.ApplyPermission("Document.Read");
        employeeButton.ApplyPermission("Employee.Read");
        attendanceButton.ApplyPermission("Salary.Read");
        payrollButton.ApplyPermission("Salary.Read");
        dormitoryButton.ApplyPermission("Salary.Read");
        departmentButton.ApplyPermission("Department.Read");
        accountButton.ApplyPermission("Account.Read");
        roleButton.ApplyPermission("Role.Read");

        assignmentButton.Tag = ControlUiEvent.NavigateAssignmentPage;
        projectButton.Tag = ControlUiEvent.NavigateProjectPage;
        documentButton.Tag = ControlUiEvent.NavigateDocumentPage;
        progressButton.Tag = ControlUiEvent.NavigateProgressPage;
        employeeButton.Tag = ControlUiEvent.NavigateEmployeePage;
        payrollButton.Tag = ControlUiEvent.NavigatePayrollPage;
        departmentButton.Tag = ControlUiEvent.NavigateDepartmentPage;
        accountButton.Tag = ControlUiEvent.NavigateAccountPage;
        roleButton.Tag = ControlUiEvent.NavigateRolePage;
    }

    private void MenuForm_Load(object sender, EventArgs e)
    {
        employeeLabel.Text =
            EmployeeBusiness.Instance.GetDisplayNameByEmployeeId(_employeeId);
        positionLabel.Text =
            PositionBusiness.Instance.GetPositionTitleByEmployeeId(_employeeId);

        _mediator.Notify(this, ControlUiEvent.NavigateProgressPage);
    }

    private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (MessageBoxWrapper.Confirm("ConfirmExit") == DialogResult.No)
        {
            e.Cancel = true;
        }
    }

    private void Personal_MouseClick(object sender, MouseEventArgs e)
    {
        Text = $"Personal Information - {_defaultText}";
        optionPanel.Visible = false;
        _mediator.Notify(
            this,
            ControlUiEvent.NavigatePersonalPage,
            new PersonalPagePayload() { EmployeeId = _employeeId }
        );
    }

    private void Personal_MouseEnter(object sender, EventArgs e)
    {
        personalPanel.BackColor = ThemeColors.Accent.Base;
    }

    private void Personal_MouseLeave(object sender, EventArgs e)
    {
        personalPanel.BackColor = Color.Transparent;
    }

    private void AppNameLabel_Click(object sender, EventArgs e)
    {
        workplacePanel.Controls.Clear();
        Text = _defaultText;
        optionPanel.Visible = false;

        _mediator.Notify(this, ControlUiEvent.NavigateProgressPage);
    }

    private void LogoutButton_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Retry;
        Close();
    }

    private void SidebarButton_Click(object sender, EventArgs e)
    {
        if (
            sender is not Button button
            || button.Tag is not ControlUiEvent uiEvent
        )
        {
            return;
        }

        Text = $"{button.Text.Trim()} - {_defaultText}";
        optionPanel.SetLocationY(button.Location.Y);
        optionPanel.Visible = true;
        _mediator.Notify(this, uiEvent);
    }

    private void SidebarLockedFeatureButton_Click(object sender, EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        optionPanel.SetLocationY(button.Location.Y);
        optionPanel.Visible = true;
        workplacePanel.Controls.Clear();
        MessageBoxWrapper.ShowInformation("TreasureFoundPremiumUnlock3");
        optionPanel.Visible = false;
        Text = _defaultText;
    }
}
