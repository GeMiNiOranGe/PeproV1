using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class SalaryLevelBusiness
{
    private static SalaryLevelBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="SalaryLevelBusiness"/>.
    /// </summary>
    public static SalaryLevelBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryLevelBusiness() { }

    /// <summary>
    /// Retrieves all salary levels associated with a specific salary scale ID.
    /// </summary>
    /// <param name="salaryScaleId">
    /// The unique identifier of the salary scale.
    /// </param>
    /// <returns>
    /// A collection of <see cref="SalaryLevelDto"/> objects that represent
    /// the salary levels linked to the specified salary scale.
    /// </returns>
    public IEnumerable<SalaryLevelDto> GetSalaryLevelsBySalaryScaleId(
        int salaryScaleId
    )
    {
        IEnumerable<SalaryLevel> salaryLevels =
            SalaryLevelDataAccess.Instance.GetManyBySalaryScaleId(
                salaryScaleId
            );
        return salaryLevels.ToDtos();
    }
}
