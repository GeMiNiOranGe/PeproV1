namespace Pepro.Business.Contracts;

public class RoleDto
{
    private int _roleId;
    private string _name = "";

    public int RoleId
    {
        get => _roleId;
        set => _roleId = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }
}
