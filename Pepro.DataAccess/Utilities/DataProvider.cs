using System.Data;
using Microsoft.Data.SqlClient;

namespace Pepro.DataAccess.Utilities;

internal class DataProvider
{
    private const string CONNECTION_STRING =
        @"Data Source=.;Initial Catalog=Pepro;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

    private static DataProvider? _instance;

    private DataProvider() { }

    /// <summary>
    /// Gets the singleton instance of <see cref="DataProvider"/>.
    /// </summary>
    public static DataProvider Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    /// <summary>
    /// Creates and returns a new <see cref="SqlConnection"/> object.
    /// </summary>
    private SqlConnection CreateConnection()
    {
        return new SqlConnection(CONNECTION_STRING);
    }

    /// <summary>
    /// Creates a <see cref="SqlCommand"/> with the specified query and a new connection.
    /// </summary>
    private SqlCommand CreateCommand(string query)
    {
        SqlConnection connection = CreateConnection();
        return new SqlCommand(query, connection);
    }

    /// <summary>
    /// Opens the specified SQL connection if it is closed or broken.
    /// </summary>
    private void OpenConnection(SqlConnection connection)
    {
        ConnectionState state = connection.State;
        if (state == ConnectionState.Closed || state == ConnectionState.Broken)
        {
            connection.Open();
        }
    }

    /// <summary>
    /// Closes the specified SQL connection if it is not null.
    /// </summary>
    private void CloseConnection(SqlConnection connection)
    {
        if (connection == null)
        {
            return;
        }
        connection.Close();
    }

    /// <summary>
    /// Executes a SQL query and returns the result as a <see cref="DataTable"/>.
    /// </summary>
    public DataTable ExecuteQuery(
        string query,
        SqlParameter[]? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using SqlCommand command = CreateCommand(query);
        command.CommandType = commandType;

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }

        using SqlDataAdapter dataAdapter = new(command);
        DataTable dataTable = new();
        dataAdapter.Fill(dataTable);
        return dataTable;
    }

    /// <summary>
    /// Executes a non-query SQL command (INSERT, UPDATE, DELETE) and returns the number of affected rows.
    /// </summary>
    public int ExecuteNonQuery(
        string query,
        SqlParameter[]? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using SqlCommand command = CreateCommand(query);
        command.CommandType = commandType;

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }

        OpenConnection(command.Connection);
        int numberOfRowsAffected = command.ExecuteNonQuery();
        CloseConnection(command.Connection);

        return numberOfRowsAffected;
    }

    /// <summary>
    /// Executes a query and returns the first column of the first row from the result set.
    /// </summary>
    public object ExecuteScalar(
        string query,
        SqlParameter[]? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using SqlCommand command = CreateCommand(query);
        command.CommandType = commandType;

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }

        OpenConnection(command.Connection);
        object obj = command.ExecuteScalar();
        CloseConnection(command.Connection);

        return obj;
    }

    /// <summary>
    /// Executes a reader-based query. Currently not implemented.
    /// </summary>
    public void ExecuteReader(string query, out SqlDataReader dataReader)
    {
        throw new NotImplementedException();
        /*
        using (var connection = CreateConnection()) {
            OpenConnection(connection);
            var sqlCommand = new SqlCommand(query, connection);
            try {
                dataReader = sqlCommand.ExecuteReader();
            }
            catch (SqlException ex) {
                throw ex;
            }
            CloseConnection(connection);
        }
        */
    }
}
