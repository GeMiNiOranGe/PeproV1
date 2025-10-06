namespace Pepro.Business.Contracts;

public class AssignmentView : AssignmentDto
{
    private string _managerFullName = "";
    private string _projectName = "";
    private string _statusName = "";

    public string ManagerFullName
    {
        get => _managerFullName;
        set => _managerFullName = value;
    }

    public string ProjectName
    {
        get => _projectName;
        set => _projectName = value;
    }

    public string StatusName
    {
        get => _statusName;
        set => _statusName = value;
    }
}
