using System.Data;
using Microsoft.Data.SqlClient;

namespace Pepro.DataAccess.Extensions;

static class ListSqlParameterExtensions
{
    /// <summary>
    /// Adds a new <see cref="SqlParameter"/> with a specified size to the parameter list.
    /// </summary>
    /// <param name="parameters">
    /// The target list of SQL parameters.
    /// </param>
    /// <param name="parameterName">
    /// The name of the SQL parameter.
    /// </param>
    /// <param name="dbType">
    /// The SQL data type.
    /// </param>
    /// <param name="size">
    /// The maximum size of the parameter.
    /// </param>
    /// <param name="value">
    /// The value assigned to the parameter.
    /// </param>
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

    /// <summary>
    /// Adds a new <see cref="SqlParameter"/> without specifying a size to the parameter list.
    /// </summary>
    /// <param name="parameters">
    /// The target list of SQL parameters.
    /// </param>
    /// <param name="parameterName">
    /// The name of the SQL parameter.
    /// </param>
    /// <param name="dbType">
    /// The SQL data type.
    /// </param>
    /// <param name="value">
    /// The value assigned to the parameter.
    /// </param>
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

    /// <summary>
    /// Adds a table-valued <see cref="SqlParameter"/> to the parameter list.
    /// </summary>
    /// <param name="parameters">
    /// The target list of SQL parameters.
    /// </param>
    /// <param name="parameterName">
    /// The name of the SQL parameter.
    /// </param>
    /// <param name="typeName">
    /// The name of the table type used in SQL.
    /// </param>
    /// <param name="value">
    /// The value assigned to the parameter, typically a <see cref="DataTable"/>.
    /// </param>
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
