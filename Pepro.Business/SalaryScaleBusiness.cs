using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class SalaryScaleBusiness
{
    private static SalaryScaleBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="SalaryScaleBusiness"/>.
    /// </summary>
    public static SalaryScaleBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryScaleBusiness() { }

    /// <summary>
    /// Retrieves all available salary scales.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="SalaryScaleDto"/> objects
    /// representing all salary scales in the system.
    /// </returns>
    public IEnumerable<SalaryScaleDto> GetSalaryScales()
    {
        IEnumerable<SalaryScale> salaryScales =
            SalaryScaleDataAccess.Instance.GetMany();
        return salaryScales.ToDtos();
    }

    /// <summary>
    /// Retrieves the salary scale associated with a specific salary level ID.
    /// </summary>
    /// <param name="salaryLevelId">
    /// The unique identifier of the salary level.
    /// </param>
    /// <returns>
    /// A <see cref="SalaryScaleDto"/> object representing the salary scale
    /// linked to the specified salary level, or <c>null</c> if not found.
    /// </returns>
    public SalaryScaleDto? GetSalaryScaleBySalaryLevelId(int salaryLevelId)
    {
        SalaryScale? salaryScale =
            SalaryScaleDataAccess.Instance.GetBySalaryLevelId(salaryLevelId);
        return salaryScale?.ToDto();
    }
}
