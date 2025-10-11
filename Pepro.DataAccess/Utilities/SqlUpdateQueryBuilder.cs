using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Extensions;

namespace Pepro.DataAccess.Utilities;

/// <summary>
/// Provides a fluent API for building SQL <c>UPDATE</c> queries dynamically with parameters.
/// </summary>
/// <param name="tableName">
/// The name of the table to update.
/// </param>
class SqlUpdateQueryBuilder(string tableName)
{
    private readonly string _tableName = tableName;
    private readonly List<string> _setClauses = [];
    private readonly List<string> _setDirectClauses = [];
    private readonly List<string> _whereClauses = [];
    private readonly List<SqlParameter> _parameters = [];

    /// <summary>
    /// Adds a direct column assignment to the update query (always included regardless of modification tracking).
    /// </summary>
    /// <param name="columnName">
    /// The column to update.
    /// </param>
    /// <param name="dbType">
    /// The SQL data type of the parameter.
    /// </param>
    /// <param name="value">
    /// The value to assign to the column.
    /// </param>
    /// <returns>
    /// The current instance for chaining.
    /// </returns>
    public SqlUpdateQueryBuilder SetDirect(
        string columnName,
        SqlDbType dbType,
        object? value
    )
    {
        _setDirectClauses.Add($"{columnName} = @{columnName}");
        _parameters.Add(columnName, dbType, value);
        return this;
    }

    /// <summary>
    /// Adds a column assignment that is included only if the tracked value has been modified.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value being tracked.
    /// </typeparam>
    /// <param name="columnName">
    /// The column to update.
    /// </param>
    /// <param name="dbType">
    /// The SQL data type of the parameter.
    /// </param>
    /// <param name="value">
    /// The tracked value representing the update candidate.
    /// </param>
    /// <returns>
    /// The current instance for chaining.
    /// </returns>
    public SqlUpdateQueryBuilder Set<T>(
        string columnName,
        SqlDbType dbType,
        TrackedValue<T> value
    )
    {
        if (value.IsModified)
        {
            _setClauses.Add($"{columnName} = @{columnName}");
            _parameters.Add(columnName, dbType, value.Value);
        }
        return this;
    }

    /// <summary>
    /// Adds a sized column assignment that is included only if the tracked value has been modified.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value being tracked.
    /// </typeparam>
    /// <param name="columnName">
    /// The column to update.
    /// </param>
    /// <param name="dbType">
    /// The SQL data type of the parameter.
    /// </param>
    /// <param name="size">
    /// The size of the SQL parameter.
    /// </param>
    /// <param name="value">
    /// The tracked value representing the update candidate.
    /// </param>
    /// <returns>
    /// The current instance for chaining.
    /// </returns>
    public SqlUpdateQueryBuilder Set<T>(
        string columnName,
        SqlDbType dbType,
        int size,
        TrackedValue<T> value
    )
    {
        if (value.IsModified)
        {
            _setClauses.Add($"{columnName} = @{columnName}");
            _parameters.Add(columnName, dbType, size, value.Value);
        }
        return this;
    }

    /// <summary>
    /// Adds a <c>WHERE</c> condition to the update query.
    /// </summary>
    /// <param name="columnName">
    /// The column name used in the condition.
    /// </param>
    /// <param name="dbType">
    /// The SQL data type of the parameter.
    /// </param>
    /// <param name="value">
    /// The value to compare.
    /// </param>
    /// <returns>
    /// The current instance for chaining.
    /// </returns>
    public SqlUpdateQueryBuilder Where(
        string columnName,
        SqlDbType dbType,
        object? value
    )
    {
        _whereClauses.Add($"{columnName} = @{columnName}");
        _parameters.Add(columnName, dbType, value);
        return this;
    }

    /// <summary>
    /// Adds a sized <c>WHERE</c> condition to the update query.
    /// </summary>
    /// <param name="columnName">
    /// The column name used in the condition.
    /// </param>
    /// <param name="dbType">
    /// The SQL data type of the parameter.
    /// </param>
    /// <param name="size">
    /// The size of the SQL parameter.
    /// </param>
    /// <param name="value">
    /// The value to compare.
    /// </param>
    /// <returns>
    /// The current instance for chaining.
    /// </returns>
    public SqlUpdateQueryBuilder Where(
        string columnName,
        SqlDbType dbType,
        int size,
        object? value
    )
    {
        _whereClauses.Add($"{columnName} = @{columnName}");
        _parameters.Add(columnName, dbType, size, value);
        return this;
    }

    /// <summary>
    /// Builds the final SQL <c>UPDATE</c> query string and returns it along with all parameters.
    /// </summary>
    /// <returns>
    /// A <see cref="QueryBuildResult"/> containing the query and its associated parameters.
    /// Returns an empty query if no columns were set or no conditions provided.
    /// </returns>
    public QueryBuildResult Build()
    {
        if (_setClauses.Count == 0)
        {
            return new("", []);
        }

        _setClauses.AddRange(_setDirectClauses);
        string query = $"""
            UPDATE {_tableName}
            SET {string.Join(", ", _setClauses)}
            WHERE {string.Join(" AND ", _whereClauses)}
            """;

        return new(query, _parameters);
    }
}
