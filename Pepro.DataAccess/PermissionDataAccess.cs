using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class PermissionDataAccess
{
    private static PermissionDataAccess? _instance;

    public static PermissionDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private PermissionDataAccess() { }

    public IEnumerable<Permission> GetManyByAccountId(int accountId)
    {
        string query = @"
            SELECT DISTINCT Permission.PermissionId
                , Permission.[Key]
            FROM Permission
            INNER JOIN RolePermission
                    ON RolePermission.PermissionId = Permission.PermissionId
            INNER JOIN [Role]
                    ON [Role].RoleId = RolePermission.RoleId
            INNER JOIN AccountRole
                    ON AccountRole.RoleId = [Role].RoleId
            INNER JOIN Account
                    ON Account.AccountId = AccountRole.AccountId
            WHERE Account.AccountId = @AccountId
        ";
        List<SqlParameter> parameters = [];
        parameters.Add("AccountId", SqlDbType.Int, accountId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(PermissionMapper.FromDataRow);
    }
}
