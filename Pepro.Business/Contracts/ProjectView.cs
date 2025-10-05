namespace Pepro.Business.Contracts;

public class ProjectView : ProjectDto
{
    private string _statusName = "";
    private string _managerFullName = "";

    public string ManagerFullName
    {
        get => _managerFullName;
        set => _managerFullName = value;
    }

    public string StatusName
    {
        get => _statusName;
        set => _statusName = value;
    }
}
