namespace Pepro.Business.Contracts;

public class StatusDto
{
    private int _statusId;
    private string _name = "";

    public int StatusId
    {
        get => _statusId;
        set => _statusId = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }
}
