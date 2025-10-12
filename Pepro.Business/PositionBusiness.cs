using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class PositionBusiness
{
    private static PositionBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="PositionBusiness"/>.
    /// </summary>
    public static PositionBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private PositionBusiness() { }

    /// <summary>
    /// Retrieves all positions from the data access layer
    /// and converts them to DTOs.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="PositionDto"/> representing all positions.
    /// </returns>
    public IEnumerable<PositionDto> GetPositions()
    {
        IEnumerable<Position> position = PositionDataAccess.Instance.GetMany();
        return position.ToDtos();
    }

    /// <summary>
    /// Retrieves the position title associated with a specific employee ID.
    /// </summary>
    /// <param name="employeeId">
    /// The unique identifier of the employee whose position title is to be retrieved.
    /// </param>
    /// <returns>
    /// The position title if found; otherwise, an empty string.
    /// </returns>
    public string GetPositionTitleByEmployeeId(int employeeId)
    {
        Position? position = PositionDataAccess.Instance.GetByEmployeeId(
            employeeId
        );
        return position?.Title ?? "";
    }
}
