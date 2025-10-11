using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class DepartmentDataAccess
{
    private static DepartmentDataAccess? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="DepartmentDataAccess"/>.
    /// </summary>
    public static DepartmentDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private DepartmentDataAccess() { }

    /// <summary>
    /// Retrieves a department by its ID.
    /// </summary>
    /// <param name="departmentId">
    /// The ID of the department to retrieve.
    /// </param>
    /// <returns>
    /// The <see cref="Department"/> if found, otherwise null.
    /// </returns>
    public Department? GetById(int departmentId)
    {
        string query = """
            SELECT Department.DepartmentId
                , Department.Name
                , Department.ManagerId
                , Department.IsDeleted
                , Department.CreatedAt
                , Department.UpdatedAt
                , Department.DeletedAt
            FROM Department
            WHERE Department.DepartmentId = @DepartmentId
                AND Department.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("DepartmentId", SqlDbType.Int, departmentId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(DepartmentMapper.FromDataRow);
    }

    /// <summary>
    /// Retrieves all departments.
    /// </summary>
    /// <returns>
    /// An enumerable collection of <see cref="Department"/> objects.
    /// </returns>
    public IEnumerable<Department> GetMany()
    {
        string query = """
            SELECT Department.DepartmentId
                , Department.Name
                , Department.ManagerId
                , Department.IsDeleted
                , Department.CreatedAt
                , Department.UpdatedAt
                , Department.DeletedAt
            FROM Department
            WHERE Department.IsDeleted = 0
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(DepartmentMapper.FromDataRow);
    }

    /// <summary>
    /// Retrieves departments by a list of IDs.
    /// </summary>
    /// <param name="departmentIds">
    /// A collection of department IDs.
    /// </param>
    /// <returns>
    /// An enumerable collection of <see cref="Department"/> objects.
    /// </returns>
    public IEnumerable<Department> GetManyByIds(IEnumerable<int> departmentIds)
    {
        if (departmentIds == null || !departmentIds.Any())
        {
            return [];
        }

        string query = """
            SELECT Department.DepartmentId
                , Department.Name
                , Department.ManagerId
                , Department.IsDeleted
                , Department.CreatedAt
                , Department.UpdatedAt
                , Department.DeletedAt
            FROM Department
            INNER JOIN @DepartmentIds AS DepartmentIds
                    ON DepartmentIds.Id = Department.DepartmentId
            WHERE Department.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];

        DataTable entityIds = TableParameters.CreateEntityIds(departmentIds);
        parameters.AddTableValued("DepartmentIds", "EntityIds", entityIds);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(DepartmentMapper.FromDataRow);
    }

    /// <summary>
    /// Searches for departments by name.
    /// </summary>
    /// <param name="searchValue">
    /// The search value.
    /// </param>
    /// <returns>
    /// An enumerable collection of <see cref="Department"/> objects matching the search criteria.
    /// </returns>
    public IEnumerable<Department> Search(string searchValue)
    {
        string query = """
            SELECT Department.DepartmentId
                , Department.Name
                , Department.ManagerId
                , Department.IsDeleted
                , Department.CreatedAt
                , Department.UpdatedAt
                , Department.DeletedAt
            FROM Department
            WHERE Department.Name LIKE '%' + @SearchValue + '%'
                AND Department.IsDeleted = 0
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
            .MapMany(DepartmentMapper.FromDataRow);
    }

    /// <summary>
    /// Inserts a new department into the database.
    /// </summary>
    /// <param name="model">
    /// The department data to insert.
    /// </param>
    /// <returns>
    /// The number of rows affected by the insert operation.
    /// </returns>
    public int Insert(InsertDepartmentModel model)
    {
        string query = """
            INSERT INTO Department
            (
                Name
                , ManagerId
            )
            VALUES
            (
                @Name
                , @ManagerId
            )
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("Name", SqlDbType.NVarChar, 50, model.Name);
        parameters.Add("ManagerId", SqlDbType.Int, model.ManagerId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }

    /// <summary>
    /// Updates an existing department in the database.
    /// </summary>
    /// <param name="departmentId">
    /// The ID of the department to update.
    /// </param>
    /// <param name="model">
    /// The updated department data.
    /// </param>
    /// <returns>
    /// The number of rows affected by the update operation.
    /// </returns>
    public int Update(int departmentId, UpdateDepartmentModel model)
    {
        QueryBuildResult result = new SqlUpdateQueryBuilder("Department")
            .Set("Name", SqlDbType.NVarChar, 50, model.Name)
            .Set("ManagerId", SqlDbType.Int, model.ManagerId)
            .SetDirect("UpdatedAt", SqlDbType.DateTime, DateTime.Now)
            .Where("DepartmentId", SqlDbType.Int, departmentId)
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
    /// Deletes a department from the database.
    /// </summary>
    /// <param name="departmentId">
    /// The ID of the department to delete.
    /// </param>
    /// <returns>
    /// The number of rows affected by the delete operation.
    /// </returns>
    public int Delete(int departmentId)
    {
        string query = """
            UPDATE Department
            SET IsDeleted = 1,
                DeletedAt = GetDate()
            WHERE DepartmentId = @DepartmentId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("DepartmentId", SqlDbType.Int, departmentId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }
}
