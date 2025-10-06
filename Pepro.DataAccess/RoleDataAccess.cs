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

    public static RoleDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private RoleDataAccess() { }

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
