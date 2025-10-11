using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class SalaryLevelDataAccess
{
    private static SalaryLevelDataAccess? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="SalaryLevelDataAccess"/>.
    /// </summary>
    public static SalaryLevelDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryLevelDataAccess() { }

    /// <summary>
    /// Retrieves multiple salary levels by their IDs.
    /// </summary>
    /// <param name="salaryLevelIds">
    /// A collection of salary level IDs to retrieve.
    /// </param>
    /// <returns>
    /// An enumerable collection of <see cref="SalaryLevel"/> entities that match the specified IDs.
    /// Returns an empty collection if no IDs are provided.
    /// </returns>
    public IEnumerable<SalaryLevel> GetManyByIds(
        IEnumerable<int> salaryLevelIds
    )
    {
        if (salaryLevelIds == null || !salaryLevelIds.Any())
        {
            return [];
        }

        string query = """
            SELECT SalaryLevel.SalaryLevelId
                , SalaryLevel.Level
                , SalaryLevel.Coefficient
                , SalaryLevel.SalaryScaleId
            FROM SalaryLevel
            INNER JOIN @SalaryLevelIds AS SalaryLevelIds
                    ON SalaryLevelIds.Id = SalaryLevel.SalaryLevelId
            """;
        List<SqlParameter> parameters = [];

        DataTable entityIds = TableParameters.CreateEntityIds(salaryLevelIds);
        parameters.AddTableValued("SalaryLevelIds", "EntityIds", entityIds);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(SalaryLevelMapper.FromDataRow);
    }

    /// <summary>
    /// Retrieves all salary levels associated with a specific salary scale.
    /// </summary>
    /// <param name="salaryScaleId">
    /// The unique ID of the salary scale.
    /// </param>
    /// <returns>
    /// A collection of <see cref="SalaryLevel"/> entities belonging to the specified salary scale.
    /// </returns>
    public IEnumerable<SalaryLevel> GetManyBySalaryScaleId(int salaryScaleId)
    {
        string query = """
            SELECT SalaryLevel.SalaryLevelId
                , SalaryLevel.Level
                , SalaryLevel.Coefficient
                , SalaryLevel.SalaryScaleId
            FROM SalaryLevel
            WHERE SalaryLevel.SalaryScaleId = @SalaryScaleId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("SalaryScaleId", SqlDbType.Int, salaryScaleId);

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(SalaryLevelMapper.FromDataRow);
    }
}
