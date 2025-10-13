using System.ComponentModel;

namespace Pepro.Presentation.Controls.Atoms;

public class PeproDataGridView : DataGridView
{
    public PeproDataGridView()
        : base()
    {
        AllowUserToAddRows = false;
        AllowUserToDeleteRows = false;
        Anchor =
            AnchorStyles.Top
            | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right;
        AutoGenerateColumns = false;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        BorderStyle = BorderStyle.Fixed3D;
        EnableHeadersVisualStyles = false;
        ReadOnly = true;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    }

    [DefaultValue(typeof(bool), "False")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new bool AllowUserToAddRows
    {
        get => base.AllowUserToAddRows;
        set => base.AllowUserToAddRows = value;
    }

    [DefaultValue(typeof(bool), "False")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new bool AllowUserToDeleteRows
    {
        get => base.AllowUserToDeleteRows;
        set => base.AllowUserToDeleteRows = value;
    }

    [DefaultValue(typeof(AnchorStyles), "Top, Bottom, Left, Right")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new AnchorStyles Anchor
    {
        get => base.Anchor;
        set => base.Anchor = value;
    }

    [DefaultValue(typeof(DataGridViewAutoSizeColumnsMode), "AllCells")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
    {
        get => base.AutoSizeColumnsMode;
        set => base.AutoSizeColumnsMode = value;
    }

    [DefaultValue(typeof(DataGridViewAutoSizeRowsMode), "AllCells")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new DataGridViewAutoSizeRowsMode AutoSizeRowsMode
    {
        get => base.AutoSizeRowsMode;
        set => base.AutoSizeRowsMode = value;
    }

    public new Color BackgroundColor
    {
        get => base.BackgroundColor;
        set => base.BackgroundColor = value;
    }

    [DefaultValue(typeof(BorderStyle), "Fixed3D")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new BorderStyle BorderStyle
    {
        get => base.BorderStyle;
        set => base.BorderStyle = value;
    }

    [DefaultValue(typeof(bool), "False")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new bool EnableHeadersVisualStyles
    {
        get => base.EnableHeadersVisualStyles;
        set => base.EnableHeadersVisualStyles = value;
    }

    [DefaultValue(typeof(bool), "True")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new bool ReadOnly
    {
        get => base.ReadOnly;
        set => base.ReadOnly = value;
    }

    [DefaultValue(typeof(DataGridViewSelectionMode), "FullRowSelect")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new DataGridViewSelectionMode SelectionMode
    {
        get => base.SelectionMode;
        set => base.SelectionMode = value;
    }

    public void ResetBackgroundColor()
    {
        // Reverts to parent's background color when available;
        // otherwise uses a default control shade.
        BackgroundColor =
            Parent != null ? Parent.BackColor : SystemColors.ControlDark;
    }

    public bool ShouldSerializeBackgroundColor()
    {
        // Only serialize when the color differs from parent
        // or default control color.
        return Parent != null
            ? BackgroundColor != Parent.BackColor
            : BackgroundColor != SystemColors.ControlDark;
    }

    protected override void OnDataBindingComplete(
        DataGridViewBindingCompleteEventArgs e
    )
    {
        base.OnDataBindingComplete(e);

        // Clears all selections after binding to avoid preselected rows.
        ClearSelection();

        // Ensures no cell remains active after binding is done.
        CurrentCell = null;
    }

    protected override void OnParentBackColorChanged(EventArgs e)
    {
        base.OnParentBackColorChanged(e);
        if (Parent != null)
        {
            // Keeps DataGridView visually consistent with its parent container.
            BackgroundColor = Parent.BackColor;
        }
    }

    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
        if (Parent != null)
        {
            // Synchronizes background color with new parent container.
            BackgroundColor = Parent.BackColor;
        }
    }

    protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
    {
        base.OnRowPostPaint(e);

        // Calculates the displayed row number (1-based index).
        string rowNumber = (e.RowIndex + 1).ToString();

        // Aligns row number text to the right and centers vertically.
        StringFormat format = new()
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center,
        };

        // Defines the bounds for the row header area where the number will be drawn.
        Rectangle headerBounds = new(
            e.RowBounds.Left - 2,
            e.RowBounds.Top + 1,
            RowHeadersWidth,
            e.RowBounds.Height
        );

        // Draws the row number text using the current header text color.
        using SolidBrush brush = new(RowHeadersDefaultCellStyle.ForeColor);
        e.Graphics.DrawString(
            rowNumber,
            e.InheritedRowStyle.Font,
            brush,
            headerBounds,
            format
        );
    }
}
