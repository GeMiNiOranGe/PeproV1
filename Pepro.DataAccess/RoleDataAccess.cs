using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class RoleDataAccess
{
    private static RoleDataAccess? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="RoleDataAccess"/>.
    /// </summary>
    public static RoleDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private RoleDataAccess() { }

    /// <summary>
    /// Retrieves a role by its unique identifier.
    /// </summary>
    /// <param name="roleId">
    /// The unique ID of the role.
    /// </param>
    /// <returns>
    /// The matching <see cref="Role"/> entity, or <c>null</c> if not found.
    /// </returns>
    public Role? GetById(int roleId)
    {
        string query = """
            SELECT Role.RoleId
                , Role.Name
                , Role.IsDeleted
                , Role.CreatedAt
                , Role.UpdatedAt
                , Role.DeletedAt
            FROM Role
            WHERE Role.RoleId = @RoleId
                AND Role.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("RoleId", SqlDbType.Int, roleId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(RoleMapper.FromDataRow);
    }

    /// <summary>
    /// Retrieves all active roles from the database.
    /// </summary>
    /// <returns>
    /// An enumerable collection of <see cref="Role"/> entities.
    /// </returns>
    public IEnumerable<Role> GetMany()
    {
        string query = """
            SELECT Role.RoleId
                , Role.Name
                , Role.IsDeleted
                , Role.CreatedAt
                , Role.UpdatedAt
                , Role.DeletedAt
            FROM Role
            WHERE Role.IsDeleted = 0
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(RoleMapper.FromDataRow);
    }

    /// <summary>
    /// Searches for roles that match the given search term.
    /// </summary>
    /// <param name="searchValue">
    /// The value to search for within role names.
    /// </param>
    /// <returns>
    /// A collection of <see cref="Role"/> entities that match the search criteria.
    /// </returns>
    public IEnumerable<Role> Search(string searchValue)
    {
        string query = """
            SELECT Role.RoleId
                , Role.Name
                , Role.IsDeleted
                , Role.CreatedAt
                , Role.UpdatedAt
                , Role.DeletedAt
            FROM Role
            WHERE Role.Name LIKE '%' + @SearchValue + '%'
                AND Role.IsDeleted = 0
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
            .MapMany(RoleMapper.FromDataRow);
    }

    /// <summary>
    /// Inserts a new role record into the database.
    /// </summary>
    /// <param name="model">
    /// The model containing role creation data.
    /// </param>
    /// <returns>
    /// The number of rows affected.
    /// </returns>
    public int Insert(InsertRoleModel model)
    {
        string query = """
            INSERT INTO Role
            (
                Name
            )
            VALUES
            (
                @Name
            )
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("Name", SqlDbType.NVarChar, 50, model.Name);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }

    /// <summary>
    /// Updates an existing role in the database.
    /// </summary>
    /// <param name="projectId">
    /// The unique ID of the role to update.
    /// </param>
    /// <param name="model">
    /// The model containing updated role data.
    /// </param>
    /// <returns>
    /// The number of rows affected.
    /// </returns>
    public int Update(int projectId, UpdateRoleModel model)
    {
        QueryBuildResult result = new SqlUpdateQueryBuilder("Role")
            .Set("Name", SqlDbType.NVarChar, 50, model.Name)
            .SetDirect("UpdatedAt", SqlDbType.DateTime, DateTime.Now)
            .Where("RoleId", SqlDbType.Int, projectId)
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
    /// Deletes a role from the database.
    /// </summary>
    /// <param name="roleId">
    /// The unique ID of the role to delete.
    /// </param>
    /// <returns>
    /// The number of rows affected.
    /// </returns>
    public int Delete(int roleId)
    {
        string query = """
            UPDATE Role
            SET IsDeleted = 1,
                DeletedAt = GetDate()
            WHERE RoleId = @RoleId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("RoleId", SqlDbType.Int, roleId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }
}
