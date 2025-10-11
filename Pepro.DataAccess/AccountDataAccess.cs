using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class AccountDataAccess
{
    private static AccountDataAccess? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="AccountDataAccess"/>.
    /// </summary>
    public static AccountDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private AccountDataAccess() { }

    /// <summary>
    /// Retrieves an account by its ID.
    /// </summary>
    /// <param name="accountId">
    /// The ID of the account to retrieve.
    /// </param>
    /// <returns>
    /// The <see cref="Account"/> if found, otherwise null.
    /// </returns>
    public Account? GetById(int accountId)
    {
        string query = """
            SELECT Account.AccountId
                , Account.Username
                , Account.Salt
                , Account.Password
                , Account.EmployeeId
                , Account.IsActive
                , Account.IsDeleted
                , Account.CreatedAt
                , Account.UpdatedAt
                , Account.DeletedAt
            FROM Account
            WHERE Account.AccountId = @AccountId
                AND Account.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("AccountId", SqlDbType.Int, accountId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(AccountMapper.FromDataRow);
    }

    /// <summary>
    /// Retrieves all active accounts.
    /// </summary>
    /// <returns>
    /// An enumerable collection of <see cref="Account"/> objects.
    /// </returns>
    public IEnumerable<Account> GetMany()
    {
        string query = """
            SELECT Account.AccountId
                , Account.Username
                , Account.Salt
                , Account.Password
                , Account.IsActive
                , Account.EmployeeId
                , Account.IsDeleted
                , Account.CreatedAt
                , Account.UpdatedAt
                , Account.DeletedAt
            FROM Account
            WHERE Account.IsDeleted = 0
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(AccountMapper.FromDataRow);
    }

    /// <summary>
    /// Finds an account using a flexible search value (e.g., username, or email) - unique columns.
    /// </summary>
    /// <param name="searchValue">
    /// The search value to use.
    /// </param>
    /// <returns>
    /// The <see cref="Account"/> if found, otherwise null.
    /// </returns>
    public Account? Find(string searchValue)
    {
        string query = """
            SELECT Account.AccountId
                , Account.Username
                , Account.Salt
                , Account.Password
                , Account.EmployeeId
                , Account.IsActive
                , Account.IsDeleted
                , Account.CreatedAt
                , Account.UpdatedAt
                , Account.DeletedAt
            FROM Account
            WHERE Account.Username = @SearchValue
                AND Account.IsDeleted = 0
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
            .MapToSingleOrDefault(AccountMapper.FromDataRow);
    }

    /// <summary>
    /// Searches for accounts by username containing the search value.
    /// </summary>
    /// <param name="searchValue">
    /// The search value to use.
    /// </param>
    /// <returns>
    /// An enumerable collection of <see cref="Account"/> objects matching the search criteria.
    /// </returns>
    public IEnumerable<Account> Search(string searchValue)
    {
        string query = """
            SELECT Account.AccountId
                , Account.Username
                , Account.Salt
                , Account.Password
                , Account.IsActive
                , Account.EmployeeId
                , Account.IsDeleted
                , Account.CreatedAt
                , Account.UpdatedAt
                , Account.DeletedAt
            FROM Account
            WHERE Account.Username LIKE '%' + @SearchValue + '%'
                AND Account.IsDeleted = 0
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
            .MapMany(AccountMapper.FromDataRow);
    }

    /// <summary>
    /// Enables/Disables an existing account in the database.
    /// </summary>
    /// <param name="accountId">
    /// The ID of the account to enable/disable.
    /// </param>
    /// <returns>
    /// The number of rows affected by the enable/disable operation.
    /// </returns>
    public int ToggleActive(int accountId)
    {
        string query = """
            UPDATE Account
            SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END,
                UpdatedAt = GetDate()
            WHERE AccountId = @AccountId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("AccountId", SqlDbType.Int, accountId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }

    /// <summary>
    /// Inserts a new account into the database.
    /// </summary>
    /// <param name="model">
    /// The account data to insert.
    /// </param>
    /// <returns>
    /// The number of rows affected by the insert operation.
    /// </returns>
    public int Insert(InsertAccountModel model)
    {
        string query = """
            INSERT INTO [Account]
            (
                [Username]
                , [Password]
                , [Salt]
                , [IsActive]
                , [EmployeeId]
            )
            VALUES
            (
                @Username
                , @Password
                , @Salt
                , @IsActive
                , @EmployeeId
            )
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("Username", SqlDbType.VarChar, 255, model.Username);
        parameters.Add(
            "Password",
            SqlDbType.VarBinary,
            DatabaseConstants.MAX_SIZE,
            model.Password
        );
        parameters.Add(
            "Salt",
            SqlDbType.VarBinary,
            DatabaseConstants.MAX_SIZE,
            model.Salt
        );
        parameters.Add("IsActive", SqlDbType.Bit, model.IsActive);
        parameters.Add("EmployeeId", SqlDbType.VarChar, 10, model.EmployeeId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }

    /// <summary>
    /// Updates an existing account in the database.
    /// </summary>
    /// <param name="accountId">
    /// The ID of the account to update.
    /// </param>
    /// <param name="model">
    /// The updated account data.
    /// </param>
    /// <returns>
    /// The number of rows affected by the update operation.
    /// </returns>
    public int Update(int accountId, UpdateAccountModel model)
    {
        QueryBuildResult result = new SqlUpdateQueryBuilder("Account")
            .Set("Username", SqlDbType.NVarChar, 255, model.Username)
            .Set(
                "Salt",
                SqlDbType.VarBinary,
                DatabaseConstants.MAX_SIZE,
                model.Salt
            )
            .Set(
                "Password",
                SqlDbType.VarBinary,
                DatabaseConstants.MAX_SIZE,
                model.Password
            )
            .Set("IsActive", SqlDbType.Bit, model.IsActive)
            .Set("EmployeeId", SqlDbType.Int, model.EmployeeId)
            .SetDirect("UpdatedAt", SqlDbType.DateTime, DateTime.Now)
            .Where("AccountId", SqlDbType.Int, accountId)
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
    /// Deletes an existing account in the database.
    /// </summary>
    /// <param name="accountId">
    /// The ID of the account to delete.
    /// </param>
    /// <returns>
    /// The number of rows affected by the delete operation.
    /// </returns>
    public int Delete(int accountId)
    {
        string query = """
            UPDATE Account
            SET IsDeleted = 1,
                DeletedAt = GetDate()
            WHERE AccountId = @AccountId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("AccountId", SqlDbType.Int, accountId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }
}
