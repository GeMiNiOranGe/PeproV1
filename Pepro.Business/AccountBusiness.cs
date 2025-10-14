using System.Security.Authentication;
using Pepro.Business.Contracts;
using Pepro.Business.Security;
using Pepro.Business.Utilities;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class AccountBusiness
{
    private readonly Hasher _hasher = new(HashAlgorithmType.Sha256, 32);
    private static AccountBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="AccountBusiness"/>.
    /// </summary>
    public static AccountBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private AccountBusiness() { }

    /// <summary>
    /// Retrieves all accounts and maps them to account views.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="AccountView"/>.
    /// </returns>
    public IEnumerable<AccountView> GetAccountViews()
    {
        IEnumerable<Account> accounts = AccountDataAccess.Instance.GetMany();
        return MapAccountsToViews(accounts);
    }

    /// <summary>
    /// Searches accounts based on the provided search value
    /// and maps the results to views.
    /// </summary>
    /// <param name="searchValue">
    /// The search keyword for filtering accounts.
    /// </param>
    /// <returns>
    /// A collection of account views matching the search criteria.
    /// </returns>
    public IEnumerable<object> SearchAccountViews(string searchValue)
    {
        IEnumerable<Account> accounts = AccountDataAccess.Instance.Search(
            searchValue
        );
        return MapAccountsToViews(accounts);
    }

    /// <summary>
    /// Creates a default account for the specified employee
    /// with a generated username and hashed password.
    /// </summary>
    /// <param name="employee">
    /// The employee for whom to create the account.
    /// </param>
    /// <returns>
    /// The number of affected rows in the database.
    /// </returns>
    public int InsertDefaultAccountByEmployee(Employee employee)
    {
        string username = AccountHelper.GenerateDefaultUsername(employee);
        HashResult hashResult = _hasher.ComputeHashWithSalt(username);

        InsertAccountModel model = new()
        {
            Username = username,
            Password = hashResult.HashedMessage,
            Salt = hashResult.Salt,
            IsActive = true,
            EmployeeId = employee.EmployeeId,
        };
        return AccountDataAccess.Instance.Insert(model);
    }

    /// <summary>
    /// Attempts to log in with the specified credentials and returns the result.
    /// </summary>
    /// <param name="accountName">
    /// The username of the account.
    /// </param>
    /// <param name="password">
    /// The password to verify.
    /// </param>
    /// <returns>
    /// A <see cref="LoginResult"/> object describing the outcome.
    /// </returns>
    public LoginResult TryLogin(string accountName, string password)
    {
        LoginResult loginResult = new();

        if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(password))
        {
            loginResult.Status = LoginStatus.InvalidInput;
            return loginResult;
        }

        Account? account = AccountDataAccess.Instance.Find(accountName);

        if (
            account == null
            || !_hasher.Verify(password, account.Password, account.Salt)
        )
        {
            loginResult.Status = LoginStatus.InvalidAccount;
            return loginResult;
        }

        if (!account.IsActive)
        {
            loginResult.Status = LoginStatus.LockedAccount;
            return loginResult;
        }

        loginResult.EmployeeId = account.EmployeeId;
        loginResult.Status = LoginStatus.Success;

        // Initialize user permissions after successful authentication.
        PermissionBusiness.Instance.Initialize(account.AccountId);

        return loginResult;
    }

    /// <summary>
    /// Toggles the active status of an account by ID.
    /// </summary>
    /// <param name="accountId">
    /// The ID of the account to toggle.
    /// </param>
    /// <returns>
    /// The number of affected rows in the database.
    /// </returns>
    public int ToggleActiveAccount(int accountId)
    {
        Account? account = AccountDataAccess.Instance.GetById(accountId);
        if (account is null)
        {
            return 0;
        }

        UpdateAccountModel model = new()
        {
            IsActive = new(!account.IsActive, true),
        };

        return AccountDataAccess.Instance.Update(accountId, model);
    }

    /// <summary>
    /// Resets the password of an account to its default value (the username).
    /// </summary>
    /// <param name="accountId">
    /// The ID of the account to reset.
    /// </param>
    /// <returns>
    /// The number of affected rows, or 0 if the operation fails.
    /// </returns>
    public int ResetPasswordAccount(int accountId)
    {
        Account? account = AccountDataAccess.Instance.GetById(accountId);

        // Return 0 if the account does not exist or password is already the default.
        if (
            account == null
            || _hasher.Verify(account.Username, account.Password, account.Salt)
        )
        {
            return 0;
        }

        HashResult hashResult = _hasher.ComputeHashWithSalt(account.Username);
        UpdateAccountModel model = new()
        {
            Password = new(hashResult.HashedMessage, true),
            Salt = new(hashResult.Salt, true),
        };

        return AccountDataAccess.Instance.Update(accountId, model);
    }

    /// <summary>
    /// Deletes an account by its ID.
    /// </summary>
    /// <param name="accountId">
    /// The ID of the account to delete.
    /// </param>
    /// <returns>
    /// The number of affected rows in the database.
    /// </returns>
    public int DeleteAccount(int accountId)
    {
        return AccountDataAccess.Instance.Delete(accountId);
    }

    /// <summary>
    /// Maps a collection of <see cref="Account"/> entities to
    /// their corresponding <see cref="AccountView"/> models.
    /// </summary>
    /// <param name="accounts">
    /// The list of accounts to map.
    /// </param>
    /// <returns>
    /// A collection of <see cref="AccountView"/> objects.
    /// </returns>
    private IEnumerable<AccountView> MapAccountsToViews(
        IEnumerable<Account> accounts
    )
    {
        IEnumerable<int> employeeIds = accounts
            .Select(account => account.EmployeeId)
            .Distinct();

        Dictionary<int, string> employees = EmployeeBusiness
            .Instance.GetEmployeesByEmployeeIds(employeeIds)
            .ToDictionary(
                employee => employee.EmployeeId,
                employee => employee.FullName
            );

        return accounts.Select(account =>
        {
            employees.TryGetValue(account.EmployeeId, out string? fullName);

            return new AccountView
            {
                AccountId = account.AccountId,
                Username = account.Username,
                IsActive = account.IsActive,
                EmployeeId = account.EmployeeId,
                EmployeeFullName = fullName ?? string.Empty,
            };
        });
    }
}
