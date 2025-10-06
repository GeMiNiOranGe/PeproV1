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
        BackgroundColor =
            Parent != null ? Parent.BackColor : SystemColors.ControlDark;
    }

    public bool ShouldSerializeBackgroundColor()
    {
        return Parent != null
            ? BackgroundColor != Parent.BackColor
            : BackgroundColor != SystemColors.ControlDark;
    }

    protected override void OnDataBindingComplete(
        DataGridViewBindingCompleteEventArgs e
    )
    {
        base.OnDataBindingComplete(e);
        ClearSelection();
        CurrentCell = null;
    }

    protected override void OnParentBackColorChanged(EventArgs e)
    {
        base.OnParentBackColorChanged(e);
        if (Parent != null)
        {
            BackgroundColor = Parent.BackColor;
        }
    }

    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
        if (Parent != null)
        {
            BackgroundColor = Parent.BackColor;
        }
    }

    protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
    {
        base.OnRowPostPaint(e);
        string rowNumber = (e.RowIndex + 1).ToString();

        StringFormat format = new()
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center,
        };

        Rectangle headerBounds = new(
            e.RowBounds.Left - 2,
            e.RowBounds.Top + 1,
            RowHeadersWidth,
            e.RowBounds.Height
        );

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
