using System.ComponentModel;
using Pepro.Presentation.Controls.Templates;

namespace Pepro.Presentation.Controls.Molecules;

public partial class NumericUpDownField : InputFieldTemplate
{
    public NumericUpDownField()
    {
        InitializeComponent();
    }

    protected override Size DefaultSize => new(240, 48);

    [DefaultValue(typeof(Size), "240, 48")]
    public new Size Size
    {
        get => base.Size;
        set => base.Size = value;
    }

    /// <summary>
    ///     Gets or sets the value
    ///     assigned to the up-down control.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue(typeof(decimal), "0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public decimal Value
    {
        get => numericUpDownField.Value;
        set => numericUpDownField.Value = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new bool Enabled
    {
        get => numericUpDownField.Enabled;
        set
        {
            numericUpDownField.Enabled = value;
            TabStop = value;
        }
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

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (numericUpDownField != null)
        {
            numericUpDownField.BackColor = BackColor;
        }
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
        base.OnForeColorChanged(e);
        if (numericUpDownField != null)
        {
            numericUpDownField.ForeColor = ForeColor;
        }
    }
}
