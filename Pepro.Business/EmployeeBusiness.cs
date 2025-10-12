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

    /// <summary>
    /// Gets the singleton instance of the <see cref="EmployeeBusiness"/> class.
    /// </summary>
    public static EmployeeBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private EmployeeBusiness() { }

    /// <summary>
    /// Retrieves all employees.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="EmployeeDto"/> representing all employees.
    /// </returns>
    public IEnumerable<EmployeeDto> GetEmployees()
    {
        IEnumerable<Employee> employees = EmployeeDataAccess.Instance.GetMany();
        return employees.ToDtos();
    }

    /// <summary>
    /// Retrieves all employees as view models with related department and position information.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="EmployeeView"/>.
    /// </returns>
    public IEnumerable<EmployeeView> GetEmployeeViews()
    {
        IEnumerable<Employee> employees = EmployeeDataAccess.Instance.GetMany();
        return MapEmployeesToViews(employees);
    }

    /// <summary>
    /// Searches employees by a given keyword and returns view models.
    /// </summary>
    /// <param name="searchValue">
    /// The keyword to search for.
    /// </param>
    /// <returns>
    /// A filtered collection of <see cref="EmployeeView"/>.
    /// </returns>
    public IEnumerable<EmployeeView> SearchEmployeeViews(string searchValue)
    {
        IEnumerable<Employee> employees = EmployeeDataAccess.Instance.Search(
            searchValue
        );
        return MapEmployeesToViews(employees);
    }

    /// <summary>
    /// Retrieves the display name of an employee by ID.
    /// </summary>
    /// <param name="employeeId">
    /// The employee's unique ID.
    /// </param>
    /// <returns>
    /// The display name in the format "FirstName, LastName", or an empty string if not found.
    /// </returns>
    public string GetDisplayNameByEmployeeId(int employeeId)
    {
        Employee? employee = EmployeeDataAccess.Instance.GetById(employeeId);
        return employee != null
            ? employee.FirstName + ", " + employee.LastName
            : "";
    }

    /// <summary>
    /// Updates an existing employee's data if changes are detected.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing new employee data.
    /// </param>
    /// <returns>
    /// The number of affected rows.
    /// </returns>
    public int UpdateEmployee(EmployeeDto dto)
    {
        Employee? entity = EmployeeDataAccess.Instance.GetById(dto.EmployeeId);
        if (entity == null)
        {
            return 0;
        }

        // Decrypts existing tax code for comparison.
        string? taxCode = EncryptionConverter.DecryptToString(entity.TaxCode);

        // Encrypts new tax code before saving.
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

    /// <summary>
    /// Deletes an employee by ID.
    /// </summary>
    /// <param name="employeeId">
    /// The employee's unique ID.
    /// </param>
    /// <returns>
    /// The number of affected rows.
    /// </returns>
    public int DeleteEmployee(int employeeId)
    {
        return EmployeeDataAccess.Instance.Delete(employeeId);
    }

    /// <summary>
    /// Inserts a new employee and automatically creates a default account for them.
    /// </summary>
    /// <param name="dto">
    /// The employee data to insert.
    /// </param>
    /// <returns>
    /// The number of affected rows or 0 if insertion failed.
    /// </returns>
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

    /// <summary>
    /// Retrieves a single employee by ID.
    /// </summary>
    /// <param name="employeeID">
    /// The employee's unique ID.
    /// </param>
    /// <returns>
    /// An <see cref="EmployeeDto"/> instance or null if not found.
    /// </returns>
    public EmployeeDto? GetEmployeeByEmployeeId(int employeeID)
    {
        Employee? employee = EmployeeDataAccess.Instance.GetById(employeeID);
        return employee?.ToDto();
    }

    /// <summary>
    /// Retrieves all phone numbers associated with an employee.
    /// </summary>
    /// <param name="employeeID">
    /// The employee's unique ID.
    /// </param>
    /// <returns>
    /// An array of phone numbers.
    /// </returns>
    public string[] GetPhoneNumbersByEmployeeId(int employeeID)
    {
        IEnumerable<PhoneNumber> phoneNumbers =
            EmployeeDataAccess.Instance.GetPhoneNumbersById(employeeID);
        return [.. phoneNumbers.Select(phoneNumber => phoneNumber.Number)];
    }

    /// <summary>
    /// Retrieves all employees belonging to a specific department.
    /// </summary>
    /// <param name="departmentId">
    /// The department's unique ID.
    /// </param>
    /// <returns>
    /// A collection of <see cref="EmployeeDto"/>.
    /// </returns>
    public IEnumerable<EmployeeDto> GetEmployeesByDepartmentId(int departmentId)
    {
        IEnumerable<Employee> employees =
            EmployeeDataAccess.Instance.GetManyByDepartmentId(departmentId);
        return employees.ToDtos();
    }

    /// <summary>
    /// Retrieves all employees associated with a specific assignment.
    /// </summary>
    /// <param name="assignmentId">
    /// The assignment's unique ID.
    /// </param>
    /// <returns>
    /// A collection of <see cref="EmployeeDto"/>.
    /// </returns>
    public IEnumerable<EmployeeDto> GetEmployeesByAssignmentId(int assignmentId)
    {
        IEnumerable<Employee> employees =
            EmployeeDataAccess.Instance.GetManyByAssignmentId(assignmentId);
        return employees.ToDtos();
    }

    /// <summary>
    /// Retrieves all employees associated with a specific project.
    /// </summary>
    /// <param name="projectId">
    /// The project's unique ID.
    /// </param>
    /// <returns>
    /// A collection of <see cref="EmployeeDto"/>.
    /// </returns>
    public IEnumerable<EmployeeDto> GetEmployeesByProjectId(int projectId)
    {
        IEnumerable<Employee> employees =
            EmployeeDataAccess.Instance.GetManyByProjectId(projectId);
        return employees.ToDtos();
    }

    /// <summary>
    /// Retrieves multiple employees by their IDs.
    /// </summary>
    /// <param name="employeeIds">
    /// A collection of employee IDs.
    /// </param>
    /// <returns>
    /// A collection of <see cref="EmployeeDto"/>.
    /// </returns>
    public IEnumerable<EmployeeDto> GetEmployeesByEmployeeIds(
        IEnumerable<int> employeeIds
    )
    {
        IEnumerable<Employee> employees =
            EmployeeDataAccess.Instance.GetManyByIds(employeeIds);
        return employees.ToDtos();
    }

    /// <summary>
    /// Maps employee entities to their view models, including related department and position names.
    /// </summary>
    /// <param name="employees">
    /// The employee entities to map.
    /// </param>
    /// <returns>
    /// A collection of <see cref="EmployeeView"/> with enriched information.
    /// </returns>
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
