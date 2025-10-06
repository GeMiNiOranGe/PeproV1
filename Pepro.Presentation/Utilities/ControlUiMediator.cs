using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Pages;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Interfaces;
using Pepro.Presentation.Payloads;

namespace Pepro.Presentation.Utilities;

public class ControlUiMediator(Panel workplacePanel) : IMediator
{
    private readonly Panel _workplacePanel = workplacePanel;

    public void Notify(object sender, ControlUiEvent ev, object? data = null)
    {
        switch (ev)
        {
            case ControlUiEvent.NavigateAccountPage:
                Navigate<AccountPage>();
                break;
            case ControlUiEvent.NavigateAssignmentPage:
                Navigate<AssignmentPage>();
                break;
            case ControlUiEvent.PushAssignmentEditorPage:
                PushEditor<AssignmentEditorPage, AssignmentDto>(data);
                break;
            case ControlUiEvent.NavigateDocumentPage:
                Navigate<DocumentPage>();
                break;
            case ControlUiEvent.NavigateDepartmentPage:
                Navigate<DepartmentPage>();
                break;
            case ControlUiEvent.PushDepartmentEditorPage:
                PushEditor<DepartmentEditorPage, DepartmentDto>(data);
                break;
            case ControlUiEvent.PushDocumentEditorPage:
                PushEditor<DocumentEditorPage, DocumentDto>(data);
                break;
            case ControlUiEvent.NavigateProgressPage:
                Navigate<ProgressPage>();
                break;
            case ControlUiEvent.NavigateProjectPage:
                Navigate<ProjectPage>();
                break;
            case ControlUiEvent.PushProjectEditorPage:
                PushEditor<ProjectEditorPage, ProjectDto>(data);
                break;
            case ControlUiEvent.NavigateEmployeePage:
                Navigate<EmployeePage>();
                break;
            case ControlUiEvent.PushEmployeeEditorPage:
                PushEditor<EmployeeEditorPage, EmployeeDto>(data);
                break;
            case ControlUiEvent.PushAssignmentDetailPage:
                PushAssignmentDetailPage(data);
                break;
            case ControlUiEvent.NavigatePersonalPage:
                NavigatePersonalPage(data);
                break;
            case ControlUiEvent.NavigatePayrollPage:
                Navigate<PayrollPage>();
                break;
            case ControlUiEvent.NavigateRolePage:
                Navigate<RolePage>();
                break;
            case ControlUiEvent.PushRoleEditorPage:
                PushEditor<RoleEditorPage, RoleDto>(data);
                break;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(ev),
                    ev,
                    "This event is not supported."
                );
        }
    }

    private TControl Create<TControl>()
        where TControl : MediatedTemplate, new()
    {
        return new() { Mediator = this };
    }

    private void PushEditor<TEditorControl, TDto>(object? data)
        where TEditorControl : EditorTemplate, IEditorUserControl<TDto>, new()
    {
        if (data is not EditorControlPayload<TDto> payload)
        {
            return;
        }

        TEditorControl control = Create<TEditorControl>();
        control.Item = payload.Item;
        control.Mode = payload.Mode;
        control.OnDataChanged = payload.OnDataChanged;

        Push(control);
    }

    private void Navigate(UserControl control)
    {
        _workplacePanel.Controls.Clear();
        control.Dock = DockStyle.Fill;
        _workplacePanel.Controls.Add(control);
        control.BringToFront();
    }

    private void Navigate<TControl>()
        where TControl : MediatedTemplate, new()
    {
        TControl control = Create<TControl>();
        Navigate(control);
    }

    private void Push(UserControl control)
    {
        control.Dock = DockStyle.Fill;
        _workplacePanel.Controls.Add(control);
        control.BringToFront();
    }

    private void PushAssignmentDetailPage(object? data)
    {
        if (data is not AssignmentDetailPagePayload payload)
        {
            return;
        }

        AssignmentDetailPage assignmentDetailControl = new()
        {
            Mediator = this,
            ProjectId = payload.ProjectId,
            ProjectName = payload.ProjectName,
        };
        Push(assignmentDetailControl);
    }

    private void NavigatePersonalPage(object? data)
    {
        if (data is not PersonalPagePayload payload)
        {
            return;
        }

        PersonalPage employeeInformationControl = new()
        {
            Mediator = this,
            EmployeeId = payload.EmployeeId,
        };
        Navigate(employeeInformationControl);
    }
}
