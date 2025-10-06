namespace Pepro.DataAccess.Entities;

public class Project
{
    private int _projectId;
    private string _name = null!;
    private string _customerName = null!;
    private DateTime _startDate;
    private DateTime _endDate;
    private int? _managerId;
    private int _statusId;
    private bool _isDeleted;
    private DateTime _createdAt;
    private DateTime _updatedAt;
    private DateTime? _deletedAt;

    public required int ProjectId
    {
        get => _projectId;
        set => _projectId = value;
    }

    public required string Name
    {
        get => _name;
        set => _name = value;
    }

    public required string CustomerName
    {
        get => _customerName;
        set => _customerName = value;
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

    public required int? ManagerId
    {
        get => _managerId;
        set => _managerId = value;
    }

    public required int StatusId
    {
        get => _statusId;
        set => _statusId = value;
    }

    public required bool IsDeleted
    {
        get => _isDeleted;
        set => _isDeleted = value;
    }

    public required DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value;
    }

    public required DateTime UpdatedAt
    {
        get => _updatedAt;
        set => _updatedAt = value;
    }

    public required DateTime? DeletedAt
    {
        get => _deletedAt;
        set => _deletedAt = value;
    }
}
