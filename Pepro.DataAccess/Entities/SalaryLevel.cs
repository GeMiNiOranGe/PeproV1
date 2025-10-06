namespace Pepro.DataAccess.Entities;

public class SalaryLevel
{
    private int _salaryLevelId;
    private string _level = null!;
    private decimal _coefficient;
    private int _salaryScaleId;

    public required int SalaryLevelId
    {
        get => _salaryLevelId;
        set => _salaryLevelId = value;
    }

    public required string Level
    {
        get => _level;
        set => _level = value;
    }

    public required decimal Coefficient
    {
        get => _coefficient;
        set => _coefficient = value;
    }

    public required int SalaryScaleId
    {
        get => _salaryScaleId;
        set => _salaryScaleId = value;
    }
}
