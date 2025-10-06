using Pepro.DataAccess;

namespace Pepro.Business;

public class PermissionBusiness
{
    private static HashSet<string> _keys = [];
    private static PermissionBusiness? _instance;

    public static PermissionBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private PermissionBusiness() { }

    public void Initialize(int accountId)
    {
        IEnumerable<string> keys = PermissionDataAccess
            .Instance.GetManyByAccountId(accountId)
            .Select(permission => permission.Key);

        _keys = [.. keys];
    }

    public bool Has(string key)
    {
        return _keys.Contains(key);
    }
}
