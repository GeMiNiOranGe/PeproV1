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

    public static DepartmentDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private DepartmentDataAccess() { }

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
