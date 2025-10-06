namespace Pepro.DataAccess.Contracts;

public class InsertAccountModel
{
    private string _username = null!;
    private byte[] _salt = null!;
    private byte[] _password = null!;
    private bool _isActive;
    private int _employeeId;

    public required string Username
    {
        get => _username;
        set => _username = value;
    }

    public required byte[] Salt
    {
        get => _salt;
        set => _salt = value;
    }

    public required byte[] Password
    {
        get => _password;
        set => _password = value;
    }

    public required bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public required int EmployeeId
    {
        get => _employeeId;
        set => _employeeId = value;
    }
}
