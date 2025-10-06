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

    public static SalaryScaleDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryScaleDataAccess() { }

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
