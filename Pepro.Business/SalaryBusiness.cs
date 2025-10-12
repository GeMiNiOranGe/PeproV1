using System.Globalization;
using Pepro.Business.Contracts;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class SalaryBusiness
{
    private static SalaryBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="SalaryBusiness"/>.
    /// </summary>
    public static SalaryBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryBusiness() { }

    /// <summary>
    /// Represents the base salary used for payroll calculations.
    /// </summary>
    public const decimal BASE_SALARY = 2_340_000m;

    /// <summary>
    /// Generates the complete payroll list by aggregating employee, salary level, scale, and position data.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Salary"/> objects representing the calculated payroll for all employees.
    /// </returns>
    public IEnumerable<Salary> GetPayroll()
    {
        IEnumerable<EmployeeDto> employees =
            EmployeeBusiness.Instance.GetEmployees();

        // Collects unique salary level IDs used by employees.
        IEnumerable<int> salaryLevelIds = employees
            .Select(employee => employee.SalaryLevelId)
            .Distinct();

        // Builds a dictionary of salary levels for fast lookup by ID.
        Dictionary<int, SalaryLevel> salaryLevels = SalaryLevelDataAccess
            .Instance.GetManyByIds(salaryLevelIds)
            .ToDictionary(
                salaryLevel => salaryLevel.SalaryLevelId,
                salaryLevel => salaryLevel
            );

        // Collects distinct salary scale IDs linked to the retrieved salary levels.
        IEnumerable<int> salaryScaleIds = salaryLevels
            .Values.Select(salaryLevel => salaryLevel.SalaryScaleId)
            .Distinct();

        // Builds a dictionary of salary scales for reference during computation.
        Dictionary<int, SalaryScale> salaryScales = SalaryScaleDataAccess
            .Instance.GetManyByIds(salaryScaleIds)
            .ToDictionary(
                salaryScale => salaryScale.SalaryScaleId,
                salaryScale => salaryScale
            );

        // Extracts all unique position IDs from employee records.
        IEnumerable<int> positionIds = employees
            .Select(employee => employee.PositionId)
            .Distinct();

        // Builds a dictionary of positions to retrieve allowance information.
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

            // Calculates the employee's base salary based on coefficient.
            decimal basicSalary = salaryLevel?.Coefficient * BASE_SALARY ?? 0;

            // Calculates allowance based on position percentage.
            decimal positionAllowance =
                position?.AllowancePercent * basicSalary ?? 0;

            // Sums base and allowance to determine total gross salary.
            decimal grossSalary = basicSalary + positionAllowance;

            // Sets currency format to Vietnamese locale.
            IFormatProvider format = CultureInfo.CreateSpecificCulture("vi-VN");
            return new Salary()
            {
                EmployeeFullName = employee.FullName,
                SalaryScaleName = salaryScale?.Name ?? "",
                SalaryScaleGroup = salaryScale?.Group ?? "",
                SalaryLevel = salaryLevel?.Level ?? "",
                SalaryLevelCoefficient =
                    salaryLevel?.Coefficient.ToString("0.00") ?? "",
                BasicSalary = basicSalary.ToString("C", format),
                PositionAllowancePercent =
                    position?.AllowancePercent.ToString("00.00%") ?? "",
                PositionAllowance = positionAllowance.ToString("C", format),
                GrossSalary = grossSalary.ToString("C", format),
            };
        });
    }
}
