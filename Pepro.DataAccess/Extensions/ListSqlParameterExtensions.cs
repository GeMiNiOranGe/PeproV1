using System.Data;
using Microsoft.Data.SqlClient;

namespace Pepro.DataAccess.Extensions;

static class ListSqlParameterExtensions
{
    public static void Add(
        this List<SqlParameter> parameters,
        string parameterName,
        SqlDbType dbType,
        int size,
        object? value
    )
    {
        SqlParameter parameter = new(parameterName, dbType, size)
        {
            Value = value ?? DBNull.Value,
        };
        parameters.Add(parameter);
    }

    public static void Add(
        this List<SqlParameter> parameters,
        string parameterName,
        SqlDbType dbType,
        object? value
    )
    {
        SqlParameter parameter = new(parameterName, dbType)
        {
            Value = value ?? DBNull.Value,
        };
        parameters.Add(parameter);
    }

    public static void AddTableValued(
        this List<SqlParameter> parameters,
        string parameterName,
        string typeName,
        object? value
    )
    {
        SqlParameter parameter = new(parameterName, SqlDbType.Structured)
        {
            TypeName = typeName,
            Value = value ?? DBNull.Value,
        };
        parameters.Add(parameter);
    }
}
