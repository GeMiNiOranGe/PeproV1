namespace Pepro.Business.Contracts;

public class Salary
{
    private string _employeeFullName = "";
    private string _salaryScaleName = "";
    private string _salaryScaleGroup = "";
    private string _salaryLevel = "";
    private string _salaryLevelcoefficient = "";
    private string _basicSalary = "";
    private string _positionAllowancePercent = "";
    private string _positionAllowance = "";
    private string _grossSalary = "";

    public string EmployeeFullName
    {
        get => _employeeFullName;
        set => _employeeFullName = value;
    }

    public string SalaryScaleName
    {
        get => _salaryScaleName;
        set => _salaryScaleName = value;
    }

    public string SalaryScaleGroup
    {
        get => _salaryScaleGroup;
        set => _salaryScaleGroup = value;
    }

    public string SalaryLevel
    {
        get => _salaryLevel;
        set => _salaryLevel = value;
    }

    public string SalaryLevelCoefficient
    {
        get => _salaryLevelcoefficient;
        set => _salaryLevelcoefficient = value;
    }

    public string BasicSalary
    {
        get => _basicSalary;
        set => _basicSalary = value;
    }

    public string PositionAllowancePercent
    {
        get => _positionAllowancePercent;
        set => _positionAllowancePercent = value;
    }

    public string PositionAllowance
    {
        get => _positionAllowance;
        set => _positionAllowance = value;
    }

    public string GrossSalary
    {
        get => _grossSalary;
        set => _grossSalary = value;
    }
}
