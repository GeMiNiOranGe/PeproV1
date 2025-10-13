using System.ComponentModel;

namespace Pepro.Presentation.Controls.Atoms;

public class PeproIconButton : Button
{
    // Stores the button's default image to restore it after the pressed state.
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

        // Cache the current image before switching to the pressed image.
        _defaultImage ??= Image;

        // Replace the image only if a pressed image is defined.
        if (PressedImage != null)
        {
            Image = PressedImage;
        }
    }

    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        base.OnMouseUp(mevent);

        // Revert the image back to the default one after releasing the button.
        if (_defaultImage != null)
        {
            Image = _defaultImage;
        }
    }
}
