using System.ComponentModel;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Atoms;
using Pepro.Presentation.Controls.Molecules;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Organisms;

/// <summary>
/// Displays a dynamic list of project progress cards within a flow layout panel,
/// supporting automatic resizing and item click events.
/// </summary>
public partial class ProjectProgressList : PeproUserControl
{
    private IEnumerable<ProjectProgressView> _data = [];
    private static readonly object s_onItemClickEvent = new();

    public ProjectProgressList()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the collection of project progress data displayed in the list.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IEnumerable<ProjectProgressView> Data
    {
        private get => _data;
        set
        {
            _data = value;
            // Refresh the visual list when new data is assigned.
            ReloadData();
        }
    }

    /// <summary>
    /// Occurs when a project progress item is clicked.
    /// </summary>
    public event EventHandler<ProjectProgressView>? OnItemClick
    {
        add => Events.AddHandler(s_onItemClickEvent, value);
        remove => Events.RemoveHandler(s_onItemClickEvent, value);
    }

    private void ProjectsFlowLayoutPanel_SizeChanged(object sender, EventArgs e)
    {
        int panelWidth = projectsFlowLayoutPanel.ClientSize.Width;
        int panelHorizontal = projectsFlowLayoutPanel.Padding.Horizontal;

        // Adjust the width of each contained control to match the panel width.
        foreach (Control control in projectsFlowLayoutPanel.Controls)
        {
            control.Width = panelWidth - panelHorizontal;
        }
    }

    /// <summary>
    /// Rebuilds the list of project progress cards based on the current data source.
    /// </summary>
    private void ReloadData()
    {
        // Clear existing cards before populating new ones.
        if (projectsFlowLayoutPanel.Controls.Count > 0)
        {
            projectsFlowLayoutPanel.Controls.Clear();
        }

        int size = _data.Count();
        int panelWidth = projectsFlowLayoutPanel.ClientSize.Width;
        int panelHorizontal = projectsFlowLayoutPanel.Padding.Horizontal;

        IEnumerable<ProjectProgressCard> cards = _data.Select(
            (item, i) =>
            {
                ProjectProgressCard projectProgressCard = new()
                {
                    Item = item,
                    // Adds bottom margin except for the last card to maintain spacing.
                    Margin =
                        i != size - 1
                            ? new Padding(0, 0, 0, 8)
                            : new Padding(0),
                    Width = panelWidth - panelHorizontal,
                    Cursor = Cursors.Hand,
                    ForeColor = ThemeColors.Text,
                    BackColor = Color.FromArgb(29, 29, 29),
                    MouseOverBackColor = ThemeColors.Accent.Base,
                    MouseDownBackColor = ThemeColors.Accent.Dark,
                };

                // Attach a click handler that raises the OnItemClick event for this card.
                projectProgressCard.Click += (sender, e) =>
                    ProjectProgressCard_OnItemClick(item);

                return projectProgressCard;
            }
        );
        projectsFlowLayoutPanel.Controls.AddRange([.. cards]);
    }

    /// <summary>
    /// Raises the OnItemClick event when a project card is clicked.
    /// </summary>
    /// <param name="item">
    /// The associated project progress view for the clicked card.
    /// </param>
    private void ProjectProgressCard_OnItemClick(ProjectProgressView item)
    {
        // Retrieve the event handler from the internal event storage and invoke it if available.
        if (
            Events[s_onItemClickEvent]
            is EventHandler<ProjectProgressView> handler
        )
        {
            handler(this, item);
        }
    }
}
