using System.Data;
using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.Business.Utilities;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class EmployeeBusiness
{
    private static EmployeeBusiness? _instance;

    public static EmployeeBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private EmployeeBusiness() { }

    public IEnumerable<EmployeeDto> GetEmployees()
    {
        IEnumerable<Employee> employees = EmployeeDataAccess.Instance.GetMany();
        return employees.ToDtos();
    }

    public IEnumerable<EmployeeView> GetEmployeeViews()
    {
        IEnumerable<Employee> employees = EmployeeDataAccess.Instance.GetMany();
        return MapEmployeesToViews(employees);
    }

    public IEnumerable<EmployeeView> SearchEmployeeViews(string searchValue)
    {
        IEnumerable<Employee> employees = EmployeeDataAccess.Instance.Search(
            searchValue
        );
        return MapEmployeesToViews(employees);
    }

    public string GetDisplayNameByEmployeeId(int employeeId)
    {
        Employee? employee = EmployeeDataAccess.Instance.GetById(employeeId);
        return employee != null
            ? employee.FirstName + ", " + employee.LastName
            : "";
    }

    public int UpdateEmployee(EmployeeDto dto)
    {
        Employee? entity = EmployeeDataAccess.Instance.GetById(dto.EmployeeId);
        if (entity == null)
        {
            return 0;
        }

        string? taxCode = EncryptionConverter.DecryptToString(entity.TaxCode);
        byte[]? encryptedTaxCode = EncryptionConverter.EncryptFromString(
            dto.TaxCode
        );

        UpdateEmployeeModel model = new()
        {
            FirstName = new(dto.FirstName, entity.FirstName != dto.FirstName),
            MiddleName = new(
                dto.MiddleName,
                entity.MiddleName != dto.MiddleName
            ),
            LastName = new(dto.LastName, entity.LastName != dto.LastName),
            DateOfBirth = new(
                dto.DateOfBirth,
                entity.DateOfBirth != dto.DateOfBirth
            ),
            Gender = new(dto.Gender, entity.Gender != dto.Gender),
            TaxCode = new(encryptedTaxCode, taxCode != dto.TaxCode),
            CitizenId = new(dto.CitizenId, entity.CitizenId != dto.CitizenId),
            DepartmentId = new(
                dto.DepartmentId,
                entity.DepartmentId != dto.DepartmentId
            ),
            PositionId = new(
                dto.PositionId,
                entity.PositionId != dto.PositionId
            ),
            SalaryLevelId = new(
                dto.SalaryLevelId,
                entity.SalaryLevelId != dto.SalaryLevelId
            ),
        };
        return EmployeeDataAccess.Instance.Update(dto.EmployeeId, model);
    }

    public int DeleteEmployee(int employeeId)
    {
        return EmployeeDataAccess.Instance.Delete(employeeId);
    }

    public int InsertEmployee(EmployeeDto dto)
    {
        InsertEmployeeModel model = dto.ToInsertModel();
        Employee? employee = EmployeeDataAccess.Instance.Add(model);
        if (employee == null)
        {
            return 0;
        }
        return AccountBusiness.Instance.InsertDefaultAccountByEmployee(
            employee
        );
    }

    public EmployeeDto? GetEmployeeByEmployeeId(int employeeID)
    {
        Employee? employee = EmployeeDataAccess.Instance.GetById(employeeID);
        return employee?.ToDto();
    }

    public string[] GetPhoneNumbersByEmployeeId(int employeeID)
    {
        IEnumerable<PhoneNumber> phoneNumbers =
            EmployeeDataAccess.Instance.GetPhoneNumbersById(employeeID);
        return [.. phoneNumbers.Select(phoneNumber => phoneNumber.Number)];
    }

    public IEnumerable<EmployeeDto> GetEmployeesByDepartmentId(int departmentId)
    {
        IEnumerable<Employee> employees =
            EmployeeDataAccess.Instance.GetManyByDepartmentId(departmentId);
        return employees.ToDtos();
    }

    public IEnumerable<EmployeeDto> GetEmployeesByAssignmentId(int assignmentId)
    {
        IEnumerable<Employee> employees =
            EmployeeDataAccess.Instance.GetManyByAssignmentId(assignmentId);
        return employees.ToDtos();
    }

    public IEnumerable<EmployeeDto> GetEmployeesByProjectId(int projectId)
    {
        IEnumerable<Employee> employees =
            EmployeeDataAccess.Instance.GetManyByProjectId(projectId);
        return employees.ToDtos();
    }

    public IEnumerable<EmployeeDto> GetEmployeesByEmployeeIds(
        IEnumerable<int> employeeIds
    )
    {
        IEnumerable<Employee> employees =
            EmployeeDataAccess.Instance.GetManyByIds(employeeIds);
        return employees.ToDtos();
    }

    private IEnumerable<EmployeeView> MapEmployeesToViews(
        IEnumerable<Employee> employees
    )
    {
        IEnumerable<int> departmentIds = employees
            .Select(e => e.DepartmentId)
            .Distinct();

        Dictionary<int, string> departments = DepartmentDataAccess
            .Instance.GetManyByIds(departmentIds)
            .ToDictionary(d => d.DepartmentId, d => d.Name);

        IEnumerable<int> positionIds = employees
            .Select(e => e.PositionId)
            .Distinct();

        Dictionary<int, string> positions = PositionDataAccess
            .Instance.GetManyByIds(positionIds)
            .ToDictionary(p => p.PositionId, p => p.Title);

        return employees.Select(employee => new EmployeeView()
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            MiddleName = employee.MiddleName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth,
            Gender = employee.Gender,
            TaxCode = EncryptionConverter.DecryptToString(employee.TaxCode),
            CitizenId = employee.CitizenId,
            DepartmentId = employee.DepartmentId,
            PositionId = employee.PositionId,
            SalaryLevelId = employee.SalaryLevelId,
            DepartmentName = departments.TryGetValue(
                employee.DepartmentId,
                out string? departmentName
            )
                ? departmentName
                : "",
            PositionTitle = positions.TryGetValue(
                employee.PositionId,
                out string? positionTitle
            )
                ? positionTitle
                : "",
        });
    }
}
