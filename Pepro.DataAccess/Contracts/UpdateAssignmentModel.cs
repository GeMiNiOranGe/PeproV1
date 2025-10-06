namespace Pepro.DataAccess.Contracts;

public class UpdateAssignmentModel
{
    private TrackedValue<string> _name = new();
    private TrackedValue<bool> _isPublicToProject = new();
    private TrackedValue<bool> _isPublicToDepartment = new();
    private TrackedValue<DateTime> _startDate = new();
    private TrackedValue<DateTime> _endDate = new();
    private TrackedValue<int> _requiredDocumentCount = new();
    private TrackedValue<int?> _managerId = new();
    private TrackedValue<int> _projectId = new();
    private TrackedValue<int> _statusId = new();

    public TrackedValue<string> Name
    {
        get => _name;
        set => _name = value;
    }

    public TrackedValue<bool> IsPublicToProject
    {
        get => _isPublicToProject;
        set => _isPublicToProject = value;
    }

    public TrackedValue<bool> IsPublicToDepartment
    {
        get => _isPublicToDepartment;
        set => _isPublicToDepartment = value;
    }

    public TrackedValue<DateTime> StartDate
    {
        get => _startDate;
        set => _startDate = value;
    }

    public TrackedValue<DateTime> EndDate
    {
        get => _endDate;
        set => _endDate = value;
    }

    public TrackedValue<int> RequiredDocumentCount
    {
        get => _requiredDocumentCount;
        set => _requiredDocumentCount = value;
    }

    public TrackedValue<int?> ManagerId
    {
        get => _managerId;
        set => _managerId = value;
    }

    public TrackedValue<int> ProjectId
    {
        get => _projectId;
        set => _projectId = value;
    }

    public TrackedValue<int> StatusId
    {
        get => _statusId;
        set => _statusId = value;
    }
}
