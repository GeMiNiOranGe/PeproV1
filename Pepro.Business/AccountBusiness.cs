using System.Security.Authentication;
using Pepro.Business.Utilities;
using Pepro.Business.Security;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Contracts;
using Pepro.Business.Contracts;

namespace Pepro.Business;

public class AccountBusiness
{
    private readonly Hasher _hasher = new(HashAlgorithmType.Sha256, 32);
    private static AccountBusiness? _instance;

    public static AccountBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private AccountBusiness() { }

    public IEnumerable<AccountView> GetAccountViews()
    {
        IEnumerable<Account> accounts = AccountDataAccess.Instance.GetMany();
        return MapAccountsToViews(accounts);
    }

    public IEnumerable<object> SearchAccountViews(string searchValue)
    {
        IEnumerable<Account> accounts = AccountDataAccess.Instance.Search(searchValue);
        return MapAccountsToViews(accounts);
    }

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
            EmployeeId = employee.EmployeeId
        };
        return AccountDataAccess.Instance.Insert(model);
    }

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
        PermissionBusiness.Instance.Initialize(account.AccountId);

        return loginResult;
    }

    public int ToggleActiveAccount(int accountId)
    {
        return AccountDataAccess.Instance.ToggleActive(accountId);
    }

    public int ResetPasswordAccount(int accountId)
    {
        Account? account = AccountDataAccess.Instance.GetById(accountId);
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

    public int DeleteAccount(int accountId)
    {
        return AccountDataAccess.Instance.Delete(accountId);
    }

    private IEnumerable<AccountView> MapAccountsToViews(IEnumerable<Account> accounts)
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
