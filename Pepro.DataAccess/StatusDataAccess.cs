using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class StatusDataAccess
{
    private static StatusDataAccess? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="StatusDataAccess"/>.
    /// </summary>
    public static StatusDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private StatusDataAccess() { }

    /// <summary>
    /// Retrieves all <see cref="Status"/> records from the database.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Status"/> objects.
    /// </returns>
    public IEnumerable<Status> GetMany()
    {
        string query = """
            SELECT Status.StatusId
                , Status.Name
            FROM Status
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(StatusMapper.FromDataRow);
    }
}
