using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class ProjectDataAccess
{
    private static ProjectDataAccess? _instance;

    public static ProjectDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private ProjectDataAccess() { }

    public Project? GetById(int projectId)
    {
        string query = """
            SELECT Project.ProjectId
                , Project.Name
                , Project.CustomerName
                , Project.ManagerId
                , Project.StartDate
                , Project.EndDate
                , Project.StatusId
                , Project.IsDeleted
                , Project.CreatedAt
                , Project.UpdatedAt
                , Project.DeletedAt
            FROM Project
            WHERE Project.ProjectId = @ProjectId
                AND Project.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("ProjectId", SqlDbType.Int, projectId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(ProjectMapper.FromDataRow);
    }

    public Project? GetByAssignmentId(int assignmentId)
    {
        string query = """
            SELECT Project.ProjectId
                , Project.Name
                , Project.CustomerName
                , Project.ManagerId
                , Project.StartDate
                , Project.EndDate
                , Project.StatusId
                , Project.IsDeleted
                , Project.CreatedAt
                , Project.UpdatedAt
                , Project.DeletedAt
            FROM Project
            INNER JOIN Assignment
                    ON Assignment.ProjectId = Project.ProjectId
            WHERE Assignment.AssignmentId = @AssignmentId
                AND Project.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("AssignmentId", SqlDbType.Int, assignmentId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(ProjectMapper.FromDataRow);
    }

    /// <summary>
    ///     Retrieve all projects
    /// </summary>
    /// <returns>
    ///     List of projects
    /// </returns>
    public IEnumerable<Project> GetMany()
    {
        string query = """
            SELECT Project.ProjectId
                , Project.Name
                , Project.CustomerName
                , Project.ManagerId
                , Project.StartDate
                , Project.EndDate
                , Project.StatusId
                , Project.IsDeleted
                , Project.CreatedAt
                , Project.UpdatedAt
                , Project.DeletedAt
            FROM Project
            WHERE Project.IsDeleted = 0
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(ProjectMapper.FromDataRow);
    }

    public IEnumerable<Project> GetManyByIds(IEnumerable<int> projectIds)
    {
        if (projectIds == null || !projectIds.Any())
        {
            return [];
        }

        string query = """
            SELECT Project.ProjectId
                , Project.Name
                , Project.CustomerName
                , Project.ManagerId
                , Project.StartDate
                , Project.EndDate
                , Project.StatusId
                , Project.IsDeleted
                , Project.CreatedAt
                , Project.UpdatedAt
                , Project.DeletedAt
            FROM Project
            INNER JOIN @ProjectIds AS ProjectIds
                    ON ProjectIds.Id = Project.ProjectId
            WHERE Project.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];

        DataTable entityIds = TableParameters.CreateEntityIds(projectIds);
        parameters.AddTableValued("ProjectIds", "EntityIds", entityIds);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(ProjectMapper.FromDataRow);
    }

    public IEnumerable<Project> GetManyByEmployeeId(int employeeId)
    {
        string query = """
            SELECT DISTINCT Project.ProjectId
                , Project.Name
                , Project.CustomerName
                , Project.ManagerId
                , Project.StartDate
                , Project.EndDate
                , Project.StatusId
                , Project.IsDeleted
                , Project.CreatedAt
                , Project.UpdatedAt
                , Project.DeletedAt
            FROM Project
            INNER JOIN Assignment
                    ON Assignment.ProjectId = Project.ProjectId
            INNER JOIN AssignmentDetail
                    ON AssignmentDetail.AssignmentId = Assignment.AssignmentId
            WHERE AssignmentDetail.EmployeeId = @EmployeeId
                AND Project.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("EmployeeId", SqlDbType.Int, employeeId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(ProjectMapper.FromDataRow);
    }

    public IEnumerable<Project> Search(string searchValue)
    {
        string query = """
            SELECT Project.ProjectId
                , Project.Name
                , Project.CustomerName
                , Project.ManagerId
                , Project.StartDate
                , Project.EndDate
                , Project.StatusId
                , Project.IsDeleted
                , Project.CreatedAt
                , Project.UpdatedAt
                , Project.DeletedAt
            FROM Project
            WHERE
                (
                    Project.ProjectId LIKE '%' + @SearchValue + '%'
                    OR Project.Name LIKE '%' + @SearchValue + '%'
                    OR Project.CustomerName LIKE '%' + @SearchValue + '%'
                )
                AND Project.IsDeleted = 0
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
            .MapMany(ProjectMapper.FromDataRow);
    }

    public int Insert(InsertProjectModel model)
    {
        string query = """
            INSERT INTO Project
            (
                Name
                , CustomerName
                , ManagerId
                , StartDate
                , EndDate
                , StatusId
            )
            VALUES
            (
                @Name
                , @CustomerName
                , @ManagerId
                , @StartDate
                , @EndDate
                , @StatusId
            )
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("Name", SqlDbType.NVarChar, 50, model.Name);
        parameters.Add(
            "CustomerName",
            SqlDbType.NVarChar,
            50,
            model.CustomerName
        );
        parameters.Add("ManagerId", SqlDbType.Int, model.ManagerId);
        parameters.Add("StartDate", SqlDbType.Date, model.StartDate);
        parameters.Add("EndDate", SqlDbType.Date, model.EndDate);
        parameters.Add("StatusId", SqlDbType.Int, model.StatusId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }

    public int Update(int projectId, UpdateProjectModel model)
    {
        QueryBuildResult result = new SqlUpdateQueryBuilder("Project")
            .Set("Name", SqlDbType.NVarChar, 50, model.Name)
            .Set("CustomerName", SqlDbType.NVarChar, 50, model.CustomerName)
            .Set("ManagerId", SqlDbType.Int, model.ManagerId)
            .Set("StartDate", SqlDbType.Date, model.StartDate)
            .Set("EndDate", SqlDbType.Date, model.EndDate)
            .Set("StatusId", SqlDbType.Int, model.StatusId)
            .SetDirect("UpdatedAt", SqlDbType.DateTime, DateTime.Now)
            .Where("ProjectId", SqlDbType.Int, projectId)
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

    public int Delete(int projectId)
    {
        string query = """
            UPDATE Project
            SET IsDeleted = 1,
                DeletedAt = GetDate()
            WHERE ProjectId = @ProjectId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("ProjectId", SqlDbType.Int, projectId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }
}
