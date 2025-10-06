using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Extensions;

namespace Pepro.DataAccess.Utilities;

class SqlUpdateQueryBuilder(string tableName)
{
    private readonly string _tableName = tableName;
    private readonly List<string> _setClauses = [];
    private readonly List<string> _setDirectClauses = [];
    private readonly List<string> _whereClauses = [];
    private readonly List<SqlParameter> _parameters = [];

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
            WHERE {string.Join(", ", _whereClauses)}
            """;

        return new(query, _parameters);
    }
}
