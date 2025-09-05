﻿namespace Pepro.DataAccess.Entities;

public class Assignment
{
    private int _assignmentId;
    private string _name = null!;
    private bool _isPublicToProject;
    private bool _isPublicToDepartment;
    private DateTime _startDate;
    private DateTime _endDate;
    private int _requiredDocumentCount;
    private int? _managerId;
    private int _projectId;
    private int _statusId;

    #region Optional fields
    private bool _isDeleted;
    private DateTime _createdAt;
    private DateTime _updatedAt;
    private DateTime? _deletedAt;
    #endregion

    public required int AssignmentId
    {
        get => _assignmentId;
        set => _assignmentId = value;
    }

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

    public bool IsDeleted
    {
        get => _isDeleted;
        set => _isDeleted = value;
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value;
    }

    public DateTime UpdatedAt
    {
        get => _updatedAt;
        set => _updatedAt = value;
    }

    public DateTime? DeletedAt
    {
        get => _deletedAt;
        set => _deletedAt = value;
    }
}
