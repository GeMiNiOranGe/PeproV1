using Pepro.Business.Contracts;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;
using System.Globalization;

namespace Pepro.Business;

public class SalaryBusiness
{
    private static SalaryBusiness? _instance;

    public static SalaryBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryBusiness() { }

    public const decimal BASE_SALARY = 2_340_000m;

    public IEnumerable<Salary> GetPayroll()
    {
        IEnumerable<EmployeeDto> employees = EmployeeBusiness.Instance.GetEmployees();

        IEnumerable<int> salaryLevelIds = employees
            .Select(employee => employee.SalaryLevelId)
            .Distinct();

        Dictionary<int, SalaryLevel> salaryLevels = SalaryLevelDataAccess
            .Instance.GetManyByIds(salaryLevelIds)
            .ToDictionary(
                salaryLevel => salaryLevel.SalaryLevelId,
                salaryLevel => salaryLevel
            );

        IEnumerable<int> salaryScaleIds = salaryLevels
            .Values.Select(salaryLevel => salaryLevel.SalaryScaleId)
            .Distinct();

        Dictionary<int, SalaryScale> salaryScales = SalaryScaleDataAccess
            .Instance.GetManyByIds(salaryScaleIds)
            .ToDictionary(
                salaryScale => salaryScale.SalaryScaleId,
                salaryScale => salaryScale
            );

        IEnumerable<int> positionIds = employees
            .Select(employee => employee.PositionId)
            .Distinct();

        Dictionary<int, Position> positions = PositionDataAccess
            .Instance.GetManyByIds(positionIds)
            .ToDictionary(
                position => position.PositionId,
                position => position
            );

        return employees.Select(employee =>
        {
            salaryLevels.TryGetValue(
                employee.SalaryLevelId,
                out SalaryLevel? salaryLevel
            );

            SalaryScale? salaryScale = null;
            if (salaryLevel != null)
            {
                salaryScales.TryGetValue(
                    salaryLevel.SalaryScaleId,
                    out salaryScale
                );
            }

            positions.TryGetValue(employee.PositionId, out Position? position);

            decimal basicSalary = salaryLevel?.Coefficient * BASE_SALARY ?? 0;
            decimal positionAllowance = position?.AllowancePercent * basicSalary ?? 0;
            decimal grossSalary = basicSalary + positionAllowance;

            IFormatProvider format = CultureInfo.CreateSpecificCulture("vi-VN");
            return new Salary()
            {
                EmployeeFullName = employee.FullName,
                SalaryScaleName = salaryScale?.Name ?? "",
                SalaryScaleGroup = salaryScale?.Group ?? "",
                SalaryLevel = salaryLevel?.Level ?? "",
                SalaryLevelCoefficient = salaryLevel?.Coefficient.ToString("0.00") ?? "",
                BasicSalary = basicSalary.ToString("C", format),
                PositionAllowancePercent = position?.AllowancePercent.ToString("00.00%") ?? "",
                PositionAllowance = positionAllowance.ToString("C", format),
                GrossSalary = grossSalary.ToString("C", format),
            };
        });
    }
}
