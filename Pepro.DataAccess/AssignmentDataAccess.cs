using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class AssignmentDataAccess
{
    private static AssignmentDataAccess? _instance;

    public static AssignmentDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private AssignmentDataAccess() { }

    public Assignment? GetById(int assignmentId)
    {
        string query = """
            SELECT Assignment.AssignmentId
                , Assignment.Name
                , Assignment.IsPublicToProject
                , Assignment.IsPublicToDepartment
                , Assignment.StartDate
                , Assignment.EndDate
                , Assignment.RequiredDocumentCount
                , Assignment.ManagerId
                , Assignment.ProjectId
                , Assignment.StatusId
                , Assignment.IsDeleted
                , Assignment.CreatedAt
                , Assignment.UpdatedAt
                , Assignment.DeletedAt
            FROM Assignment
            WHERE Assignment.AssignmentId = @AssignmentId
                AND Assignment.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("AssignmentId", SqlDbType.Int, assignmentId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(AssignmentMapper.FromDataRow);
    }

    public Assignment? GetByDocumentId(int documentId)
    {
        string query = """
            SELECT Assignment.AssignmentId
                , Assignment.Name
                , Assignment.IsPublicToProject
                , Assignment.IsPublicToDepartment
                , Assignment.StartDate
                , Assignment.EndDate
                , Assignment.RequiredDocumentCount
                , Assignment.ManagerId
                , Assignment.ProjectId
                , Assignment.StatusId
                , Assignment.IsDeleted
                , Assignment.CreatedAt
                , Assignment.UpdatedAt
                , Assignment.DeletedAt
            FROM Assignment
            INNER JOIN Document
                    ON Document.AssignmentId = Assignment.AssignmentId
            WHERE Document.DocumentId = @DocumentId
                AND Assignment.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("DocumentId", SqlDbType.Int, documentId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(AssignmentMapper.FromDataRow);
    }

    /// <summary>
    ///     Take out the manager of the assignment
    /// </summary>
    /// <param name="assignmentId">Assignment id</param>
    /// <returns>
    ///     Manager
    /// </returns>
    public Employee? GetManager(int assignmentId)
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
            INNER JOIN Assignment
                    ON Assignment.ManagerId = Employee.EmployeeId
            WHERE Assignment.AssignmentId = @AssignmentId
                AND Employee.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("AssignmentId", SqlDbType.VarChar, 10, assignmentId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(EmployeeMapper.FromDataRow);
    }

    public IEnumerable<Assignment> GetMany()
    {
        string query = """
            SELECT Assignment.AssignmentId
                , Assignment.Name
                , Assignment.IsPublicToProject
                , Assignment.IsPublicToDepartment
                , Assignment.StartDate
                , Assignment.EndDate
                , Assignment.RequiredDocumentCount
                , Assignment.ManagerId
                , Assignment.ProjectId
                , Assignment.StatusId
                , Assignment.IsDeleted
                , Assignment.CreatedAt
                , Assignment.UpdatedAt
                , Assignment.DeletedAt
            FROM Assignment
            WHERE Assignment.IsDeleted = 0
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(AssignmentMapper.FromDataRow);
    }

    public IEnumerable<Assignment> GetManyByEmployeeId(int employeeId)
    {
        string query = """
            SELECT Assignment.AssignmentId
                , Assignment.Name
                , Assignment.IsPublicToProject
                , Assignment.IsPublicToDepartment
                , Assignment.StartDate
                , Assignment.EndDate
                , Assignment.RequiredDocumentCount
                , Assignment.ManagerId
                , Assignment.ProjectId
                , Assignment.StatusId
                , Assignment.IsDeleted
                , Assignment.CreatedAt
                , Assignment.UpdatedAt
                , Assignment.DeletedAt
            FROM Assignment
            INNER JOIN AssignmentDetail
                    ON AssignmentDetail.AssignmentId = Assignment.AssignmentId
            WHERE AssignmentDetail.EmployeeId = @EmployeeId
                AND Assignment.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("EmployeeId", SqlDbType.Int, employeeId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(AssignmentMapper.FromDataRow);
    }

    public IEnumerable<Assignment> GetManyByProjectId(int projectId)
    {
        string query = """
            SELECT Assignment.AssignmentId
                , Assignment.Name
                , Assignment.IsPublicToProject
                , Assignment.IsPublicToDepartment
                , Assignment.StartDate
                , Assignment.EndDate
                , Assignment.RequiredDocumentCount
                , Assignment.ManagerId
                , Assignment.ProjectId
                , Assignment.StatusId
                , Assignment.IsDeleted
                , Assignment.CreatedAt
                , Assignment.UpdatedAt
                , Assignment.DeletedAt
            FROM Assignment
            WHERE Assignment.ProjectId = @ProjectId
                AND Assignment.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("ProjectId", SqlDbType.Int, projectId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(AssignmentMapper.FromDataRow);
    }

    public IEnumerable<Assignment> Search(string searchValue)
    {
        string query = """
            SELECT Assignment.AssignmentId
                , Assignment.Name
                , Assignment.IsPublicToProject
                , Assignment.IsPublicToDepartment
                , Assignment.StartDate
                , Assignment.EndDate
                , Assignment.RequiredDocumentCount
                , Assignment.ManagerId
                , Assignment.ProjectId
                , Assignment.StatusId
                , Assignment.IsDeleted
                , Assignment.CreatedAt
                , Assignment.UpdatedAt
                , Assignment.DeletedAt
            FROM Assignment
            WHERE
                (
                    Assignment.AssignmentId LIKE '%' + @SearchValue + '%'
                    OR Assignment.Name LIKE '%' + @SearchValue + '%'
                )
                AND Assignment.IsDeleted = 0
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
            .MapMany(AssignmentMapper.FromDataRow);
    }

    public IEnumerable<(
        int AssignmentId,
        int DocumentCount
    )> CountDocumentsByAssignmentIds(IEnumerable<int> assignmentIds)
    {
        if (!assignmentIds.Any())
        {
            return [];
        }

        string query = """
            SELECT AssignmentIds.Id             AS [AssignmentId]
                , Count(Document.AssignmentId)  AS [DocumentCount]
            FROM @AssignmentIds AS AssignmentIds
            LEFT JOIN Document
                    ON Document.AssignmentId = AssignmentIds.Id
                    AND Document.IsDeleted = 0
            GROUP BY AssignmentIds.Id
            """;
        List<SqlParameter> parameters = [];

        DataTable entityIds = TableParameters.CreateEntityIds(assignmentIds);
        parameters.AddTableValued("AssignmentIds", "EntityIds", entityIds);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .AsEnumerable()
            .Select(row =>
                (
                    AssignmentId: row.Field<int>("AssignmentId"),
                    DocumentCount: row.Field<int>("DocumentCount")
                )
            );
    }

    public int Insert(InsertAssignmentModel model)
    {
        string query = """
            INSERT INTO Assignment
            (
                Name
                , IsPublicToProject
                , IsPublicToDepartment
                , StartDate
                , EndDate
                , RequiredDocumentCount
                , ManagerId
                , ProjectId
                , StatusId
            )
            VALUES
            (
                @Name
                , @IsPublicToProject
                , @IsPublicToDepartment
                , @StartDate
                , @EndDate
                , @RequiredDocumentCount
                , @ManagerId
                , @ProjectId
                , @StatusId
            )
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("Name", SqlDbType.NVarChar, 50, model.Name);
        parameters.Add(
            "IsPublicToProject",
            SqlDbType.Bit,
            model.IsPublicToProject
        );
        parameters.Add(
            "IsPublicToDepartment",
            SqlDbType.Bit,
            model.IsPublicToDepartment
        );
        parameters.Add("StartDate", SqlDbType.Date, model.StartDate);
        parameters.Add("EndDate", SqlDbType.Date, model.EndDate);
        parameters.Add(
            "RequiredDocumentCount",
            SqlDbType.Int,
            model.RequiredDocumentCount
        );
        parameters.Add("ManagerId", SqlDbType.Int, model.ManagerId);
        parameters.Add("ProjectId", SqlDbType.Int, model.ProjectId);
        parameters.Add("StatusId", SqlDbType.Int, model.StatusId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }

    public int Update(int assignmentId, UpdateAssignmentModel model)
    {
        QueryBuildResult result = new SqlUpdateQueryBuilder("Assignment")
            .Set("Name", SqlDbType.NVarChar, 50, model.Name)
            .Set("IsPublicToProject", SqlDbType.Bit, model.IsPublicToProject)
            .Set(
                "IsPublicToDepartment",
                SqlDbType.Bit,
                model.IsPublicToDepartment
            )
            .Set("StartDate", SqlDbType.Date, model.StartDate)
            .Set("EndDate", SqlDbType.Date, model.EndDate)
            .Set(
                "RequiredDocumentCount",
                SqlDbType.Int,
                model.RequiredDocumentCount
            )
            .Set("ManagerId", SqlDbType.Int, model.ManagerId)
            .Set("ProjectId", SqlDbType.Int, model.ProjectId)
            .Set("StatusId", SqlDbType.Int, model.StatusId)
            .SetDirect("UpdatedAt", SqlDbType.DateTime, DateTime.Now)
            .Where("AssignmentId", SqlDbType.Int, assignmentId)
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

    public int Delete(int assignmentId)
    {
        string query = """
            UPDATE Assignment
            SET IsDeleted = 1,
                DeletedAt = GetDate()
            WHERE AssignmentId = @AssignmentId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("AssignmentId", SqlDbType.Int, assignmentId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }
}
