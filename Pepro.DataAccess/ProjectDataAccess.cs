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

    /// <summary>
    /// Gets the singleton instance of <see cref="ProjectDataAccess"/>.
    /// </summary>
    public static ProjectDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private ProjectDataAccess() { }

    /// <summary>
    /// Retrieves a project by its unique identifier.
    /// </summary>
    /// <param name="projectId">
    /// The ID of the project to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Project"/> object if found; otherwise, <c>null</c>.
    /// </returns>
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

    /// <summary>
    /// Retrieves a project associated with a specific assignment.
    /// </summary>
    /// <param name="assignmentId">
    /// The ID of the assignment linked to the project.
    /// </param>
    /// <returns>
    /// A <see cref="Project"/> object if found; otherwise, <c>null</c>.
    /// </returns>
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
    /// Retrieves all projects.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Project"/> objects.
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

    /// <summary>
    /// Retrieves multiple projects based on a list of project IDs.
    /// </summary>
    /// <param name="projectIds">
    /// The collection of project IDs to retrieve.
    /// </param>
    /// <returns>
    /// A collection of <see cref="Project"/> objects matching the specified IDs.
    /// </returns>
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

    /// <summary>
    /// Retrieves all projects associated with a given employee.
    /// </summary>
    /// <param name="employeeId">
    /// The ID of the employee whose projects are to be retrieved.
    /// </param>
    /// <returns>
    /// A collection of <see cref="Project"/> objects related to the specified employee.
    /// </returns>
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

    /// <summary>
    /// Searches for projects that match the specified keyword.
    /// </summary>
    /// <param name="searchValue">
    /// The keyword to search by (matches project ID, name, or customer name).
    /// </param>
    /// <returns>
    /// A collection of <see cref="Project"/> objects that match the search criteria.
    /// </returns>
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

    /// <summary>
    /// Inserts a new project record into the database.
    /// </summary>
    /// <param name="model">
    /// The project data to insert.
    /// </param>
    /// <returns>
    /// The number of rows affected by the insert operation.
    /// </returns>
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

    /// <summary>
    /// Updates an existing project record.
    /// </summary>
    /// <param name="projectId">
    /// The ID of the project to update.
    /// </param>
    /// <param name="model">
    /// The updated project data.
    /// </param>
    /// <returns>
    /// The number of rows affected by the update operation.
    /// </returns>
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

    /// <summary>
    /// Deletes a project from the database.
    /// </summary>
    /// <param name="projectId">
    /// The ID of the project to delete.
    /// </param>
    /// <returns>
    /// The number of rows affected by the delete operation.
    /// </returns>
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
