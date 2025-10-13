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

    /// <summary>
    /// Gets or sets a value indicating whether the return button in the header is visible.
    /// </summary>
    /// <remarks>
    /// When set to <see langword="true"/>, the return button becomes visible and is assigned
    /// a standard width of 48 pixels.
    /// When set to <see langword="false"/>, the button is hidden and its width is reduced to zero
    /// to maintain layout consistency without occupying space.
    /// </remarks>
    [Browsable(true)]
    [Category("Behavior")]
    [Description(
        "Determines whether the return button in the header is visible."
    )]
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

    /// <summary>
    /// Gets or sets the text displayed in the header area of the template.
    /// </summary>
    /// <remarks>
    /// This property defines the main title or label shown at the top of the control.
    /// It can be used to describe the content or purpose of the derived editor or view.
    /// </remarks>
    [Browsable(true)]
    [Category("Appearance")]
    [Description(
        "Specifies the text displayed in the header area of the template."
    )]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public virtual string HeaderText
    {
        get => headerLabel.Text;
        set => headerLabel.Text = value;
    }

    /// <summary>
    /// Gets the mediator instance associated with this template.
    /// </summary>
    /// <remarks>
    /// The mediator acts as an intermediary between UI components to reduce direct dependencies.
    /// It can be set only during initialization via object initializers using the <c>init</c> accessor.
    /// </remarks>
    [Description(
        "Gets the mediator instance that coordinates communication between components."
    )]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IMediator? Mediator
    {
        protected get => _mediator;
        init => _mediator = value;
    }

    /// <summary>
    /// Handles the control’s load event and initializes
    /// the header’s icon appearance when not in design mode.
    /// </summary>
    /// <param name="e">
    /// The event data for the load event.
    /// </param>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Avoid icon assignment in design mode to prevent runtime-only
        // dependencies from loading in the designer.
        if (!DesignMode)
        {
            headerReturnButton.Image = IconProvider.GetIcon(
                "ArrowLeft",
                color: ThemeColors.Text
            );
        }
    }

    /// <summary>
    /// Closes the current template by removing it from its parent container.
    /// </summary>
    /// <remarks>
    /// This method provides a standard way for derived templates to dismiss themselves
    /// from view without directly manipulating parent layout logic.
    /// </remarks>
    protected void Close()
    {
        Parent?.Controls.Remove(this);
    }

    /// <summary>
    /// Handles the click event of the header’s return button.
    /// </summary>
    /// <param name="sender">
    /// The source of the event.
    /// </param>
    /// <param name="e">
    /// The event data.
    /// </param>
    private void HeaderReturnButton_Click(object sender, EventArgs e)
    {
        Close();
    }
}
