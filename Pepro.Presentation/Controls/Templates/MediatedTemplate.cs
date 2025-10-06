using System.ComponentModel;
using Pepro.Presentation.Controls.Atoms;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Interfaces;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Templates;

public partial class MediatedTemplate : PeproUserControl
{
    private IMediator? _mediator;

    public MediatedTemplate()
    {
        InitializeComponent();

        headerReturnButton.ApplyFlatStyleNoBackColor();
    }

    [Browsable(true)]
    [Category("Behavior")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public virtual bool ReturnButtonVisible
    {
        get => headerReturnButton.Visible;
        set
        {
            headerReturnButton.Visible = value;
            headerReturnButton.Width = value ? 48 : 0;
        }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public virtual string HeaderText
    {
        get => headerLabel.Text;
        set => headerLabel.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IMediator? Mediator
    {
        protected get => _mediator;
        init => _mediator = value;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (!DesignMode)
        {
            headerReturnButton.Image = IconProvider.GetIcon(
                "ArrowLeft",
                color: ThemeColors.Text
            );
        }
    }

    protected void Close()
    {
        Parent?.Controls.Remove(this);
    }

    private void HeaderReturnButton_Click(object sender, EventArgs e)
    {
        Close();
    }
}
