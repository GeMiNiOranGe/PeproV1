using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Payloads;

namespace Pepro.Presentation.Controls.Pages;

public partial class ProgressPage : MediatedTemplate
{
    public ProgressPage()
    {
        InitializeComponent();
    }

    private void ProgressPage_Load(object sender, EventArgs e)
    {
        projectProgressList.Data = ProjectBusiness.Instance.GetProjectProgressViews();
    }

    private void ProjectsFlowLayoutPanel_OnItemClick(
        object? sender,
        ProjectProgressView item
    )
    {
        Mediator?.Notify(
            this,
            ControlUiEvent.PushAssignmentDetailPage,
            new AssignmentDetailPagePayload()
            {
                ProjectId = item.ProjectId,
                ProjectName = item.Name
            }
        );
    }
}
