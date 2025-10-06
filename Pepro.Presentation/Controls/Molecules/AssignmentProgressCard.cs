using System.ComponentModel;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;

namespace Pepro.Presentation.Controls.Molecules;

public partial class AssignmentProgressCard : CardTemplate
{
    private AssignmentProgressView _item = null!;
    private Color _defaultBackColor;

    public AssignmentProgressCard()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color MouseOverBackColor { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color MouseDownBackColor { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AssignmentProgressView Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            assignmentIdLabel.Text = _item.AssignmentId.ToString();
            assignmentNameLabel.Text = _item.Name;
            assignmentPercentLabel.Text =
                _item.ProgressPercent.ToString() + "%";
        }
    }

    private void AssignmentProgressCardControl_MouseEnter(
        object sender,
        EventArgs e
    )
    {
        _defaultBackColor = BackColor;
        BackColor = MouseOverBackColor;
    }

    private void AssignmentProgressCardControl_MouseLeave(
        object sender,
        EventArgs e
    )
    {
        BackColor = _defaultBackColor;
    }

    private void AssignmentProgressCardControl_MouseDown(
        object sender,
        MouseEventArgs e
    )
    {
        BackColor = MouseDownBackColor;
    }

    private void AssignmentProgressCardControl_MouseUp(
        object sender,
        MouseEventArgs e
    )
    {
        BackColor = MouseOverBackColor;
    }

    private void AssignmentProgressCardControl_Click(object sender, EventArgs e)
    {
        OnClick(e);
    }
}
