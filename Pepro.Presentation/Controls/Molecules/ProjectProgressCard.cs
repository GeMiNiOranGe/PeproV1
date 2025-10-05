using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using System.ComponentModel;

namespace Pepro.Presentation.Controls.Molecules;

public partial class ProjectProgressCard : CardTemplate
{
    private ProjectProgressView _item = null!;
    private Color _defaultBackColor;

    public ProjectProgressCard()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color MouseOverBackColor
    {
        get;
        set;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color MouseDownBackColor
    {
        get;
        set;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ProjectProgressView Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            projectIdLabel.Text = _item.ProjectId.ToString();
            projectNameLabel.Text = _item.Name;
            projectPercentLabel.Text = _item.ProgressPercent.ToString() + "%";
        }
    }

    private void ProjectProgressCardControl_MouseEnter(object sender, EventArgs e)
    {
        _defaultBackColor = BackColor;
        BackColor = MouseOverBackColor;
    }

    private void ProjectProgressCardControl_MouseLeave(object sender, EventArgs e)
    {
        BackColor = _defaultBackColor;
    }

    private void ProjectProgressCardControl_MouseDown(object sender, MouseEventArgs e)
    {
        BackColor = MouseDownBackColor;
    }

    private void ProjectProgressCardControl_Click(object sender, EventArgs e)
    {
        OnClick(e);
    }
}
