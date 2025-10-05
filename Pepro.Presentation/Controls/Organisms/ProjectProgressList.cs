using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Atoms;
using Pepro.Presentation.Controls.Molecules;
using Pepro.Presentation.Utilities;
using System.ComponentModel;

namespace Pepro.Presentation.Controls.Organisms;

public partial class ProjectProgressList : PeproUserControl
{
    private IEnumerable<ProjectProgressView> _data = [];
    private static readonly object s_onItemClickEvent = new();

    public ProjectProgressList()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IEnumerable<ProjectProgressView> Data
    {
        private get => _data;
        set
        {
            _data = value;
            ReloadData();
        }
    }

    public event EventHandler<ProjectProgressView>? OnItemClick
    {
        add => Events.AddHandler(s_onItemClickEvent, value);
        remove => Events.RemoveHandler(s_onItemClickEvent, value);
    }

    private void ProjectsFlowLayoutPanel_SizeChanged(object sender, EventArgs e)
    {
        int panelWidth = projectsFlowLayoutPanel.ClientSize.Width;
        int panelHorizontal = projectsFlowLayoutPanel.Padding.Horizontal;

        foreach (Control control in projectsFlowLayoutPanel.Controls)
        {
            control.Width = panelWidth - panelHorizontal;
        }
    }

    private void ReloadData()
    {
        if (projectsFlowLayoutPanel.Controls.Count > 0)
        {
            projectsFlowLayoutPanel.Controls.Clear();
        }

        int size = _data.Count();
        int panelWidth = projectsFlowLayoutPanel.ClientSize.Width;
        int panelHorizontal = projectsFlowLayoutPanel.Padding.Horizontal;

        IEnumerable<ProjectProgressCard> cards = _data.Select((item, i) =>
        {
            ProjectProgressCard projectProgressCard = new()
            {
                Item = item,
                Margin = i != size - 1
                    ? new Padding(0, 0, 0, 8)
                    : new Padding(0),
                Width = panelWidth - panelHorizontal,
                Cursor = Cursors.Hand,
                ForeColor = ThemeColors.Text,
                BackColor = Color.FromArgb(29, 29, 29),
                MouseOverBackColor = ThemeColors.Accent.Base,
                MouseDownBackColor = ThemeColors.Accent.Dark
            };

            projectProgressCard.Click += (sender, e) =>
                ProjectProgressCard_OnItemClick(item);

            return projectProgressCard;
        });
        projectsFlowLayoutPanel.Controls.AddRange([.. cards]);
    }

    private void ProjectProgressCard_OnItemClick(ProjectProgressView item)
    {
        if (Events[s_onItemClickEvent] is EventHandler<ProjectProgressView> handler)
        {
            handler(this, item);
        }
    }
}
