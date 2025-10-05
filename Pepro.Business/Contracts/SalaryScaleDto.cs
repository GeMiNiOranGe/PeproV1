namespace Pepro.Business.Contracts;

public class SalaryScaleDto
{
    private int _salaryScaleId;
    private string _group = "";
    private string _name = "";

    public int SalaryScaleId
    {
        get => _salaryScaleId;
        set => _salaryScaleId = value;
    }

    public string Group
    {
        get => _group;
        set => _group = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }
}
