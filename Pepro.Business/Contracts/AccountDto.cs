namespace Pepro.Business.Contracts;

public class AccountDto
{
    private int accountId;
    private string username = "";
    private bool isActive;
    private int employeeId;

    public int AccountId
    {
        get => accountId;
        set => accountId = value;
    }

    public string Username
    {
        get => username;
        set => username = value;
    }

    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }

    public int EmployeeId
    {
        get => employeeId;
        set => employeeId = value;
    }
}
