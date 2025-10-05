using System.ComponentModel;

namespace Pepro.Business.Contracts;

public class EmployeeDto
{
    private int _employeeId;
    private string _firstName = "";
    private string? _middleName;
    private string _lastName = "";
    private DateOnly _dateOfBirth;
    private bool? _gender;
    private string? _taxCode;
    private string _citizenId = "";
    private int _departmentId;
    private int _positionId;
    private int _salaryLevelId;

    public int EmployeeId
    {
        get => _employeeId;
        set => _employeeId = value;
    }

    public string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public string? MiddleName
    {
        get => _middleName;
        set => _middleName = value;
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public string FullName
    {
        get
        {
            return string.Join(" ", new[] {
                _lastName,
                _middleName,
                _firstName,
            }.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }

    public DateOnly DateOfBirth
    {
        get => _dateOfBirth;
        set => _dateOfBirth = value;
    }

    public bool? Gender
    {
        get => _gender;
        set => _gender = value;
    }

    public string? TaxCode
    {
        get => _taxCode;
        set => _taxCode = value;
    }

    public string CitizenId
    {
        get => _citizenId;
        set => _citizenId = value;
    }

    public int DepartmentId
    {
        get => _departmentId;
        set => _departmentId = value;
    }

    public int PositionId
    {
        get => _positionId;
        set => _positionId = value;
    }

    public int SalaryLevelId
    {
        get => _salaryLevelId;
        set => _salaryLevelId = value;
    }
}
