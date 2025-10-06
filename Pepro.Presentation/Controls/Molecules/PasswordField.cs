using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Pepro.Presentation.Controls.Templates;

namespace Pepro.Presentation.Controls.Molecules;

public partial class PasswordField : InputFieldTemplate
{
    private Image? _togglePasswordImage;

    public PasswordField()
    {
        InitializeComponent();

        FocusColor = Color.Gray;
    }

    protected override Size DefaultSize => new(240, 48);

    [DefaultValue(typeof(Size), "240, 48")]
    public new Size Size
    {
        get => base.Size;
        set => base.Size = value;
    }

    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue(typeof(Color), "Gray")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FocusColor { get; set; }

    [AllowNull]
    [DefaultValue("")]
    public virtual string PlaceholderText
    {
        get => textBoxField.PlaceholderText;
        set => textBoxField.PlaceholderText = value;
    }

    [AllowNull]
    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override string Text
    {
        get => textBoxField.Text;
        set => textBoxField.Text = value;
    }

    [Category("Appearance")]
    [Description("The image displayed default state of the toggle password")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(null)]
    public Image? TogglePasswordImage
    {
        get => _togglePasswordImage;
        set
        {
            _togglePasswordImage = value;
            togglePasswordButton.BackgroundImage = _togglePasswordImage;
        }
    }

    [Category("Appearance")]
    [Description(
        "The image displayed when the toggle password button is pressed."
    )]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(null)]
    public Image? TogglePasswordPressedImage { get; set; }

    /// <summary>
    ///     Clears all text from the text box control.
    /// </summary>
    public void Clear()
    {
        textBoxField.Clear();
    }

    protected override void SetBoundsCore(
        int x,
        int y,
        int width,
        int height,
        BoundsSpecified specified
    )
    {
        base.SetBoundsCore(x, y, width, DefaultSize.Height, specified);
    }

    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        underlinePanel.BackColor = FocusColor.IsEmpty ? Color.Gray : FocusColor;
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        underlinePanel.BackColor = ForeColor;
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (textBoxField != null)
        {
            textBoxField.BackColor = BackColor;
        }

        if (togglePasswordButton != null)
        {
            togglePasswordButton.BackColor = BackColor;
        }
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
        base.OnForeColorChanged(e);
        if (textBoxField != null)
        {
            textBoxField.ForeColor = ForeColor;
        }

        // Make the BackColor of the panel the same color as the text color
        if (underlinePanel != null)
        {
            underlinePanel.BackColor = ForeColor;
        }
    }

    private void TogglePasswordButton_Click(object sender, EventArgs e)
    {
        if (textBoxField.UseSystemPasswordChar)
        {
            textBoxField.UseSystemPasswordChar = false;
            togglePasswordButton.BackgroundImage = TogglePasswordPressedImage;
        }
        else
        {
            textBoxField.UseSystemPasswordChar = true;
            togglePasswordButton.BackgroundImage = TogglePasswordImage;
        }
    }
}
