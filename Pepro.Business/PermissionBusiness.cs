using Pepro.DataAccess;

namespace Pepro.Business;

public class PermissionBusiness
{
    // Holds all permission keys for the currently active account.
    private static HashSet<string> _keys = [];
    private static PermissionBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of the <see cref="PermissionBusiness"/> class.
    /// </summary>
    public static PermissionBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private PermissionBusiness() { }

    /// <summary>
    /// Initializes permission keys for the specified account.
    /// </summary>
    /// <param name="accountId">
    /// The unique ID of the account whose permissions will be loaded.
    /// </param>
    public void Initialize(int accountId)
    {
        IEnumerable<string> keys = PermissionDataAccess
            .Instance.GetManyByAccountId(accountId)
            .Select(permission => permission.Key);

        _keys = [.. keys];
    }

    /// <summary>
    /// Checks whether the current account has the specified permission.
    /// </summary>
    /// <param name="key">
    /// The permission key to check.
    /// </param>
    /// <returns>
    /// <c>true</c> if the permission exists; otherwise, <c>false</c>.
    /// </returns>
    public bool Has(string key)
    {
        return _keys.Contains(key);
    }
}
