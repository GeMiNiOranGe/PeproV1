namespace Pepro.Business.Contracts;

public class DepartmentDto
{
    private int _departmentId;
    private string _name = "";
    private int? _managerId;

    public int DepartmentId
    {
        get => _departmentId;
        set => _departmentId = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public int? ManagerId
    {
        get => _managerId;
        set => _managerId = value;
    }
}
