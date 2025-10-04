namespace Pepro.DataAccess.Entities;

public class Employee
{
    private int _employeeId;
    private string _firstName = null!;
    private string? _middleName;
    private string _lastName = null!;
    private DateOnly _dateOfBirth;
    private bool? _gender;
    private byte[]? _taxCode;
    private string _citizenId = null!;
    private int _departmentId;
    private int _positionId;
    private int _salaryLevelId;
    private bool _isDeleted;
    private DateTime _createdAt;
    private DateTime _updatedAt;
    private DateTime? _deletedAt;

    public required int EmployeeId
    {
        get => _employeeId;
        set => _employeeId = value;
    }

    public required string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public required string? MiddleName
    {
        get => _middleName;
        set => _middleName = value;
    }

    public required string LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public required DateOnly DateOfBirth
    {
        get => _dateOfBirth;
        set => _dateOfBirth = value;
    }

    public required bool? Gender
    {
        get => _gender;
        set => _gender = value;
    }

    public required byte[]? TaxCode
    {
        get => _taxCode;
        set => _taxCode = value;
    }

    public required string CitizenId
    {
        get => _citizenId;
        set => _citizenId = value;
    }

    public required int DepartmentId
    {
        get => _departmentId;
        set => _departmentId = value;
    }

    public required int PositionId
    {
        get => _positionId;
        set => _positionId = value;
    }

    public required int SalaryLevelId
    {
        get => _salaryLevelId;
        set => _salaryLevelId = value;
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
