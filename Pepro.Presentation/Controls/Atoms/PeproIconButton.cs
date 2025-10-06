using System.ComponentModel;

namespace Pepro.Presentation.Controls.Atoms;

public class PeproIconButton : Button
{
    private Image? _defaultImage;

    public PeproIconButton()
        : base() { }

    [Category("Appearance")]
    [Description("The image displayed when the button is pressed.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(null)]
    public Image? PressedImage { get; set; }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        base.OnMouseDown(mevent);

        _defaultImage ??= Image;
        if (PressedImage != null)
        {
            Image = PressedImage;
        }
    }

    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        base.OnMouseUp(mevent);

        if (_defaultImage != null)
        {
            Image = _defaultImage;
        }
    }
}
