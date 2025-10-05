namespace Pepro.Business.Contracts;

public class AccountView : AccountDto
{
    private string _employeeFullName = "";

    public string EmployeeFullName
    {
        get => _employeeFullName;
        set => _employeeFullName = value;
    }

    public string ActiveStatusText => IsActive ? "Active" : "Inactive";
}
