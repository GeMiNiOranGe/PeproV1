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

    public static SalaryLevelDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryLevelDataAccess() { }

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
