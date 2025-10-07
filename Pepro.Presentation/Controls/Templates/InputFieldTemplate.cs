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

    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public virtual string LabelText
    {
        get => inputFieldLabel.Text;
        set => inputFieldLabel.Text = value;
    }

    [AllowNull]
    [Category("Appearance")]
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
