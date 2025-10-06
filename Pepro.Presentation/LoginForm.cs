using System.ComponentModel;
using System.Runtime.InteropServices;
using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Atoms;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation;

public partial class LoginForm : PeproForm
{
    private int _employeeId;

    public LoginForm()
    {
        InitializeComponent();
        InitializeRuntimeComponents();
    }

    private void InitializeRuntimeComponents()
    {
        accountNameTextBoxField.FocusColor = ThemeColors.Secondary.Base;

        passwordField.FocusColor = ThemeColors.Secondary.Base;
        passwordField.TogglePasswordImage = IconProvider.GetIcon(
            "EyeClosed",
            color: ThemeColors.Text
        );
        passwordField.TogglePasswordPressedImage = IconProvider.GetIcon(
            "Eye",
            color: ThemeColors.Text
        );

        signinButton.FlatAppearance.MouseDownBackColor = ThemeColors
            .Secondary
            .Dark;
        signinButton.FlatAppearance.MouseOverBackColor = ThemeColors
            .Secondary
            .Light;

        closeButton.BackColor = ThemeColors.System.CloseButton.Normal;
        closeButton.BackgroundImage = IconProvider.GetIcon(
            "XMark",
            size: 32,
            color: ThemeColors.Text
        );
        closeButton.FlatAppearance.BorderSize = 0;
        closeButton.FlatAppearance.MouseDownBackColor = ThemeColors
            .System
            .CloseButton
            .Dark;
        closeButton.FlatAppearance.MouseOverBackColor = ThemeColors
            .System
            .CloseButton
            .Light;
        closeButton.FlatStyle = FlatStyle.Flat;

        logoPictureBox.BackgroundImage = IconProvider.GetLogo();

        illustrationPictureBox.Image = IconProvider.GetImage(
            "LoginPage",
            frameColor: Color.FromArgb(29, 29, 29)
        );
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int EmployeeId
    {
        get => _employeeId;
        private set => _employeeId = value;
    }

    private void LoginButton_Click(object sender, EventArgs e)
    {
        string accountName = accountNameTextBoxField.Text;
        string password = passwordField.Text;

        try
        {
            LoginResult loginResult = AccountBusiness.Instance.TryLogin(
                accountName,
                password
            );

            switch (loginResult.Status)
            {
                case LoginStatus.Success:
                    _employeeId = loginResult.EmployeeId;
                    DialogResult = DialogResult.OK;
                    Close();
                    break;
                case LoginStatus.InvalidInput:
                    errorLabel.Text =
                        "Username and password must not be empty!";
                    break;
                case LoginStatus.InvalidAccount:
                    errorLabel.Text = "Incorrect username or password!";
                    break;
                case LoginStatus.LockedAccount:
                    errorLabel.Text = "Your account has been locked!";
                    break;
            }
        }
        catch (Exception ex)
        {
            errorLabel.Text = "An error occurred during the login process!";
            MessageBox.Show(ex.Message);
        }

        accountNameTextBoxField.Clear();
        passwordField.Clear();
        accountNameTextBoxField.Focus();
    }

    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;

    [DllImport("user32.dll")]
    public static extern int SendMessage(
        IntPtr hWnd,
        int Msg,
        int wParam,
        int lParam
    );

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    private void LoginForm_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }

    private void ClosePictureBox_Click(object sender, EventArgs e)
    {
        Close();
    }
}
