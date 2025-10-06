using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class EmployeeDataAccess
{
    private static EmployeeDataAccess? _instance;

    public static EmployeeDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private EmployeeDataAccess() { }

    public Employee? GetById(int employeeId)
    {
        string query = """
            SELECT Employee.EmployeeId
                , Employee.FirstName
                , Employee.MiddleName
                , Employee.LastName
                , Employee.DateOfBirth
                , Employee.Gender
                , Employee.TaxCode
                , Employee.CitizenId
                , Employee.DepartmentId
                , Employee.PositionId
                , Employee.SalaryLevelId
                , Employee.IsDeleted
                , Employee.CreatedAt
                , Employee.UpdatedAt
                , Employee.DeletedAt
            FROM Employee
            WHERE Employee.EmployeeId = @EmployeeId
            AND Employee.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("EmployeeId", SqlDbType.Int, employeeId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(EmployeeMapper.FromDataRow);
    }

    public IEnumerable<Employee> GetMany()
    {
        string query = """
            SELECT Employee.EmployeeId
                , Employee.FirstName
                , Employee.MiddleName
                , Employee.LastName
                , Employee.DateOfBirth
                , Employee.Gender
                , Employee.TaxCode
                , Employee.CitizenId
                , Employee.DepartmentId
                , Employee.PositionId
                , Employee.SalaryLevelId
                , Employee.IsDeleted
                , Employee.CreatedAt
                , Employee.UpdatedAt
                , Employee.DeletedAt
            FROM Employee
            WHERE Employee.IsDeleted = 0
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(EmployeeMapper.FromDataRow);
    }

    public IEnumerable<Employee> GetManyByIds(IEnumerable<int> employeeIds)
    {
        if (employeeIds == null || !employeeIds.Any())
        {
            return [];
        }

        string query = """
            SELECT Employee.EmployeeId
                , Employee.FirstName
                , Employee.MiddleName
                , Employee.LastName
                , Employee.DateOfBirth
                , Employee.Gender
                , Employee.TaxCode
                , Employee.CitizenId
                , Employee.DepartmentId
                , Employee.PositionId
                , Employee.SalaryLevelId
                , Employee.IsDeleted
                , Employee.CreatedAt
                , Employee.UpdatedAt
                , Employee.DeletedAt
            FROM Employee
            INNER JOIN @EmployeeIds AS EmployeeIds
                    ON EmployeeIds.Id = Employee.EmployeeId
            WHERE Employee.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];

        DataTable entityIds = TableParameters.CreateEntityIds(employeeIds);
        parameters.AddTableValued("EmployeeIds", "EntityIds", entityIds);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(EmployeeMapper.FromDataRow);
    }

    public IEnumerable<Employee> GetManyByAssignmentId(int assignmentId)
    {
        string query = """
            SELECT Employee.EmployeeId
                , Employee.FirstName
                , Employee.MiddleName
                , Employee.LastName
                , Employee.DateOfBirth
                , Employee.Gender
                , Employee.TaxCode
                , Employee.CitizenId
                , Employee.DepartmentId
                , Employee.PositionId
                , Employee.SalaryLevelId
                , Employee.IsDeleted
                , Employee.CreatedAt
                , Employee.UpdatedAt
                , Employee.DeletedAt
            FROM Employee
            INNER JOIN AssignmentDetail
                    ON AssignmentDetail.EmployeeId = Employee.EmployeeId
            WHERE AssignmentDetail.AssignmentId = @AssignmentId
            AND Employee.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("AssignmentId", SqlDbType.Int, assignmentId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(EmployeeMapper.FromDataRow);
    }

    public IEnumerable<Employee> GetManyByDepartmentId(int departmentId)
    {
        string query = """
            SELECT Employee.EmployeeId
                , Employee.FirstName
                , Employee.MiddleName
                , Employee.LastName
                , Employee.DateOfBirth
                , Employee.Gender
                , Employee.TaxCode
                , Employee.CitizenId
                , Employee.DepartmentId
                , Employee.PositionId
                , Employee.SalaryLevelId
                , Employee.IsDeleted
                , Employee.CreatedAt
                , Employee.UpdatedAt
                , Employee.DeletedAt
            FROM Employee
            WHERE Employee.DepartmentId = @DepartmentId
            AND Employee.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("DepartmentId", SqlDbType.Int, departmentId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(EmployeeMapper.FromDataRow);
    }

    public IEnumerable<Employee> GetManyByProjectId(int projectId)
    {
        string query = """
            SELECT Employee.EmployeeId
                , Employee.FirstName
                , Employee.MiddleName
                , Employee.LastName
                , Employee.DateOfBirth
                , Employee.Gender
                , Employee.TaxCode
                , Employee.CitizenId
                , Employee.DepartmentId
                , Employee.PositionId
                , Employee.SalaryLevelId
                , Employee.IsDeleted
                , Employee.CreatedAt
                , Employee.UpdatedAt
                , Employee.DeletedAt
            FROM Employee
            INNER JOIN Department
                    ON Department.DepartmentId = Employee.DepartmentId
                    AND Department.IsDeleted = 0
            INNER JOIN DepartmentProject
                    ON DepartmentProject.DepartmentId = Department.DepartmentId
            WHERE DepartmentProject.ProjectId = @ProjectId
                AND Employee.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("ProjectId", SqlDbType.Int, projectId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(EmployeeMapper.FromDataRow);
    }

    public IEnumerable<Employee> Search(string searchValue)
    {
        string query = """
            SELECT Employee.EmployeeId
                , Employee.FirstName
                , Employee.MiddleName
                , Employee.LastName
                , Employee.DateOfBirth
                , Employee.Gender
                , Employee.TaxCode
                , Employee.CitizenId
                , Employee.DepartmentId
                , Employee.PositionId
                , Employee.SalaryLevelId
                , Employee.IsDeleted
                , Employee.CreatedAt
                , Employee.UpdatedAt
                , Employee.DeletedAt
            FROM Employee
            WHERE
                (
                    Employee.EmployeeId LIKE '%' + @SearchValue + '%'
                    OR Employee.LastName + ' ' + IsNull(Employee.MiddleName + ' ', '') + Employee.FirstName LIKE '%' + @SearchValue + '%'
                )
                AND Employee.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add(
            "SearchValue",
            SqlDbType.NVarChar,
            DatabaseConstants.SEARCH_SIZE,
            searchValue
        );

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(EmployeeMapper.FromDataRow);
    }

    public IEnumerable<PhoneNumber> GetPhoneNumbersById(int employeeId)
    {
        string query = """
            SELECT PhoneNumber.PhoneNumberId
                , PhoneNumber.Number
                , PhoneNumber.EmployeeId
            FROM PhoneNumber
            INNER JOIN Employee
                    ON Employee.EmployeeId = PhoneNumber.EmployeeId
            WHERE Employee.EmployeeId = @EmployeeId
            AND Employee.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("EmployeeId", SqlDbType.Int, employeeId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(PhoneNumberMapper.FromDataRow);
    }

    public Employee? Add(InsertEmployeeModel model)
    {
        string query = """
            INSERT INTO [dbo].[Employee]
            (
                [FirstName]
                , [MiddleName]
                , [LastName]
                , [DateOfBirth]
                , [Gender]
                , [TaxCode]
                , [CitizenId]
                , [DepartmentId]
                , [PositionId]
                , [SalaryLevelId]
            )
            OUTPUT
                Inserted.[EmployeeId]
                , Inserted.[FirstName]
                , Inserted.[MiddleName]
                , Inserted.[LastName]
                , Inserted.[DateOfBirth]
                , Inserted.[Gender]
                , Inserted.[TaxCode]
                , Inserted.[CitizenId]
                , Inserted.[DepartmentId]
                , Inserted.[PositionId]
                , Inserted.[SalaryLevelId]
                , Inserted.[IsDeleted]
                , Inserted.[CreatedAt]
                , Inserted.[UpdatedAt]
                , Inserted.[DeletedAt]
            VALUES
            (
                @FirstName
                , @MiddleName
                , @LastName
                , @DateOfBirth
                , @Gender
                , @TaxCode
                , @CitizenId
                , @DepartmentId
                , @PositionId
                , @SalaryLevelId
            )
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("FirstName", SqlDbType.NVarChar, 10, model.FirstName);
        parameters.Add("MiddleName", SqlDbType.NVarChar, 30, model.MiddleName);
        parameters.Add("LastName", SqlDbType.NVarChar, 10, model.LastName);
        parameters.Add("DateOfBirth", SqlDbType.Date, model.DateOfBirth);
        parameters.Add("Gender", SqlDbType.Bit, model.Gender);
        parameters.Add(
            "TaxCode",
            SqlDbType.VarBinary,
            DatabaseConstants.MAX_SIZE,
            model.TaxCode
        );
        parameters.Add("CitizenId", SqlDbType.VarChar, 12, model.CitizenId);
        parameters.Add("DepartmentId", SqlDbType.Int, model.DepartmentId);
        parameters.Add("PositionId", SqlDbType.Int, model.PositionId);
        parameters.Add("SalaryLevelId", SqlDbType.Int, model.SalaryLevelId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(EmployeeMapper.FromDataRow);
    }

    public int Insert(InsertEmployeeModel model)
    {
        string query = """
            INSERT INTO [dbo].[Employee]
            (
                [FirstName]
                , [MiddleName]
                , [LastName]
                , [DateOfBirth]
                , [Gender]
                , [TaxCode]
                , [CitizenId]
                , [DepartmentId]
                , [PositionId]
                , [SalaryLevelId]
            )
            VALUES
            (
                @FirstName
                , @MiddleName
                , @LastName
                , @DateOfBirth
                , @Gender
                , @TaxCode
                , @CitizenId
                , @DepartmentId
                , @PositionId
                , @SalaryLevelId
            )
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("FirstName", SqlDbType.NVarChar, 10, model.FirstName);
        parameters.Add("MiddleName", SqlDbType.NVarChar, 30, model.MiddleName);
        parameters.Add("LastName", SqlDbType.NVarChar, 10, model.LastName);
        parameters.Add("DateOfBirth", SqlDbType.Date, model.DateOfBirth);
        parameters.Add("Gender", SqlDbType.Bit, model.Gender);
        parameters.Add(
            "TaxCode",
            SqlDbType.VarBinary,
            DatabaseConstants.MAX_SIZE,
            model.TaxCode
        );
        parameters.Add("CitizenId", SqlDbType.VarChar, 12, model.CitizenId);
        parameters.Add("DepartmentId", SqlDbType.Int, model.DepartmentId);
        parameters.Add("PositionId", SqlDbType.Int, model.PositionId);
        parameters.Add("SalaryLevelId", SqlDbType.Int, model.SalaryLevelId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }

    public int Update(int employeeId, UpdateEmployeeModel model)
    {
        QueryBuildResult result = new SqlUpdateQueryBuilder("Employee")
            .Set("FirstName", SqlDbType.NVarChar, 10, model.FirstName)
            .Set("MiddleName", SqlDbType.NVarChar, 30, model.MiddleName)
            .Set("LastName", SqlDbType.NVarChar, 10, model.LastName)
            .Set("DateOfBirth", SqlDbType.Date, model.DateOfBirth)
            .Set("Gender", SqlDbType.Bit, model.Gender)
            .Set(
                "TaxCode",
                SqlDbType.VarBinary,
                DatabaseConstants.MAX_SIZE,
                model.TaxCode
            )
            .Set("CitizenId", SqlDbType.VarChar, 12, model.CitizenId)
            .Set("DepartmentId", SqlDbType.Int, model.DepartmentId)
            .Set("PositionId", SqlDbType.Int, model.PositionId)
            .Set("SalaryLevelId", SqlDbType.Int, model.SalaryLevelId)
            .SetDirect("UpdatedAt", SqlDbType.DateTime, DateTime.Now)
            .Where("EmployeeId", SqlDbType.Int, employeeId)
            .Build();

        if (string.IsNullOrEmpty(result.Query) || result.Parameters.Count == 0)
        {
            return 0;
        }

        return DataProvider.Instance.ExecuteNonQuery(
            result.Query,
            [.. result.Parameters]
        );
    }

    public int Delete(int employeeId)
    {
        string query = """
            UPDATE Employee
            SET IsDeleted = 1,
                DeletedAt = GetDate()
            WHERE EmployeeId = @EmployeeId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("EmployeeId", SqlDbType.Int, employeeId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }
}
