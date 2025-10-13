using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Pepro.Presentation.Controls.Atoms;

namespace Pepro.Presentation.Controls.Templates;

public partial class InputFieldTemplate : PeproUserControl
{
    public InputFieldTemplate()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the text displayed by the label associated with the input field.
    /// </summary>
    /// <remarks>
    /// This property affects the visible caption of the input field’s label.
    /// It can be modified at both design time and runtime.
    /// </remarks>
    [Browsable(true)]
    [Category("Appearance")]
    [Description(
        "The text displayed by the label associated with the input field."
    )]
    [DefaultValue("")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public virtual string LabelText
    {
        get => inputFieldLabel.Text;
        set => inputFieldLabel.Text = value;
    }

    /// <summary>
    /// Gets or sets the font used to display the label text.
    /// </summary>
    /// <remarks>
    /// Changing this value updates the label’s visual appearance immediately.
    /// </remarks>
    [AllowNull]
    [Category("Appearance")]
    [Description("The font used to display the label text.")]
    [DefaultValue(typeof(Font), "Segoe UI, 14px")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public virtual Font LabelFont
    {
        get => inputFieldLabel.Font;
        set => inputFieldLabel.Font = value;
    }

    protected override Size DefaultSize => new(240, 128);

    [DefaultValue(typeof(Size), "240, 128")]
    public new Size Size
    {
        get => base.Size;
        set => base.Size = value;
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
}
