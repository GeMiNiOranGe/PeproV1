using Pepro.Presentation.Extensions;

namespace Pepro.Presentation.Controls.Atoms;

public class PeproLabel : Label
{
    protected override void OnPaint(PaintEventArgs e)
    {
        // Clears the drawing surface using the label's background color.
        e.Graphics.Clear(BackColor);

        // Retrieves the internal leading (extra spacing) in pixels for the current font.
        float internalLeading = Font.FontFamily.GetInternalLeadingInPixels(
            Font
        );
        int topOffset = (int)Math.Round(internalLeading);

        // Defines the drawing bounds for the text, adjusted by the internal leading value.
        Rectangle bounds = new(
            0,
            topOffset,
            ClientSize.Width,
            ClientSize.Height - topOffset
        );

        // Configures text rendering flags for padding and layout behavior.
        TextFormatFlags flags = TextFormatFlags.NoPadding;

        // Enables multiline wrapping when AutoSize is disabled.
        if (!AutoSize)
        {
            flags |= TextFormatFlags.WordBreak;
        }
        else
        {
            // For AutoSize labels, restrict to a single line and use ellipsis for overflow.
            flags |= TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;
        }

        TextRenderer.DrawText(e.Graphics, Text, Font, bounds, ForeColor, flags);
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        // Starts with no padding and applies layout rules consistent with OnPaint.
        TextFormatFlags flags = TextFormatFlags.NoPadding;

        // Allows multiline wrapping only when AutoSize is disabled.
        if (!AutoSize)
        {
            flags |= TextFormatFlags.WordBreak;
        }

        Size size = TextRenderer.MeasureText(Text, Font, proposedSize, flags);

        // Adds internal leading to the measured height for proper vertical spacing.
        float internalLeading = Font.FontFamily.GetInternalLeadingInPixels(
            Font
        );
        int newHeight = size.Height + (int)Math.Round(internalLeading);
        return new Size(size.Width, newHeight);
    }
}
