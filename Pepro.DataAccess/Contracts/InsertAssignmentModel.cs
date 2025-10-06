namespace Pepro.DataAccess.Contracts;

public class InsertAssignmentModel
{
    private string _name = null!;
    private bool _isPublicToProject;
    private bool _isPublicToDepartment;
    private DateTime _startDate;
    private DateTime _endDate;
    private int _requiredDocumentCount;
    private int? _managerId;
    private int _projectId;
    private int _statusId;

    public required string Name
    {
        get => _name;
        set => _name = value;
    }

    public required bool IsPublicToProject
    {
        get => _isPublicToProject;
        set => _isPublicToProject = value;
    }

    public required bool IsPublicToDepartment
    {
        get => _isPublicToDepartment;
        set => _isPublicToDepartment = value;
    }

    public required DateTime StartDate
    {
        get => _startDate;
        set => _startDate = value;
    }

    public required DateTime EndDate
    {
        get => _endDate;
        set => _endDate = value;
    }

    public required int RequiredDocumentCount
    {
        get => _requiredDocumentCount;
        set => _requiredDocumentCount = value;
    }

    public required int? ManagerId
    {
        get => _managerId;
        set => _managerId = value;
    }

    public required int ProjectId
    {
        get => _projectId;
        set => _projectId = value;
    }

    public required int StatusId
    {
        get => _statusId;
        set => _statusId = value;
    }
}
