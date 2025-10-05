namespace Pepro.Business.Contracts;

public class SalaryLevelDto
{
    private int _salaryLevelId;
    private string _level = "";
    private decimal _coefficient;
    private int _salaryScaleId;

    public int SalaryLevelId
    {
        get => _salaryLevelId;
        set => _salaryLevelId = value;
    }

    public string Level
    {
        get => _level;
        set => _level = value;
    }

    public decimal Coefficient
    {
        get => _coefficient;
        set => _coefficient = value;
    }

    public int SalaryScaleId
    {
        get => _salaryScaleId;
        set => _salaryScaleId = value;
    }
}
