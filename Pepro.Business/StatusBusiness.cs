using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class StatusBusiness
{
    private static StatusBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="StatusBusiness"/>.
    /// </summary>
    public static StatusBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private StatusBusiness() { }

    /// <summary>
    /// Retrieves all available statuses.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="StatusDto"/> objects
    /// representing all statuses in the system.
    /// </returns>
    public IEnumerable<StatusDto> GetStatuses()
    {
        IEnumerable<Status> statuses = StatusDataAccess.Instance.GetMany();
        return statuses.ToDtos();
    }
}
