using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class SalaryScaleDataAccess
{
    private static SalaryScaleDataAccess? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="SalaryScaleDataAccess"/>.
    /// </summary>
    public static SalaryScaleDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryScaleDataAccess() { }

    /// <summary>
    /// Retrieves a salary scale associated with a specific salary level ID.
    /// </summary>
    /// <param name="salaryLevelId">
    /// The unique ID of the salary level.
    /// </param>
    /// <returns>
    /// A <see cref="SalaryScale"/> object associated with the given salary level,
    /// or <c>null</c> if not found.
    /// </returns>
    public SalaryScale? GetBySalaryLevelId(int salaryLevelId)
    {
        string query = """
            SELECT SalaryScale.SalaryScaleId
                , SalaryScale.[Group]
                , SalaryScale.Name
            FROM SalaryScale
            INNER JOIN SalaryLevel
                    ON SalaryLevel.SalaryScaleId = SalaryScale.SalaryScaleId
            WHERE SalaryLevel.SalaryLevelId = @SalaryLevelId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("SalaryLevelId", SqlDbType.Int, salaryLevelId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapToSingleOrDefault(SalaryScaleMapper.FromDataRow);
    }

    /// <summary>
    /// Retrieves all salary scales from the database.
    /// </summary>
    /// <returns>
    /// A collection of all <see cref="SalaryScale"/> entities.
    /// </returns>
    public IEnumerable<SalaryScale> GetMany()
    {
        string query = """
            SELECT SalaryScale.SalaryScaleId
                , SalaryScale.[Group]
                , SalaryScale.Name
            FROM SalaryScale
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(SalaryScaleMapper.FromDataRow);
    }

    /// <summary>
    /// Retrieves multiple salary scales by their IDs.
    /// </summary>
    /// <param name="salaryScaleIds">
    /// A collection of salary scale IDs to retrieve.
    /// </param>
    /// <returns>
    /// An enumerable collection of <see cref="SalaryScale"/> entities that match the specified IDs.
    /// Returns an empty collection if no IDs are provided.
    /// </returns>
    public IEnumerable<SalaryScale> GetManyByIds(
        IEnumerable<int> salaryScaleIds
    )
    {
        if (salaryScaleIds == null || !salaryScaleIds.Any())
        {
            return [];
        }

        string query = """
            SELECT SalaryScale.SalaryScaleId
                , SalaryScale.[Group]
                , SalaryScale.Name
            FROM SalaryScale
            INNER JOIN @SalaryScaleIds AS SalaryScaleIds
                    ON SalaryScaleIds.Id = SalaryScale.SalaryScaleId
            """;
        List<SqlParameter> parameters = [];

        DataTable entityIds = TableParameters.CreateEntityIds(salaryScaleIds);
        parameters.AddTableValued("SalaryScaleIds", "EntityIds", entityIds);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(SalaryScaleMapper.FromDataRow);
    }
}
