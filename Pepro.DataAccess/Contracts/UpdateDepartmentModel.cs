namespace Pepro.DataAccess.Contracts;

public class UpdateDepartmentModel
{
    private TrackedValue<string> _name = new();
    private TrackedValue<int?> _managerId = new();

    public TrackedValue<string> Name
    {
        get => _name;
        set => _name = value;
    }

    public TrackedValue<int?> ManagerId
    {
        get => _managerId;
        set => _managerId = value;
    }
}
