using System.Data;
using Microsoft.Data.SqlClient;

namespace Pepro.DataAccess.Utilities;

internal class DataProvider
{
    private const string CONNECTION_STRING =
        @"Data Source=.;Initial Catalog=Pepro;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

    private static DataProvider? _instance;

    private DataProvider() { }

    public static DataProvider Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(CONNECTION_STRING);
    }

    public SqlCommand CreateCommand(string query)
    {
        SqlConnection connection = CreateConnection();
        return new SqlCommand(query, connection);
    }

    public void OpenConnection(SqlConnection connection)
    {
        ConnectionState state = connection.State;
        if (state == ConnectionState.Closed || state == ConnectionState.Broken)
        {
            connection.Open();
        }
    }

    public void CloseConnection(SqlConnection connection)
    {
        if (connection == null)
        {
            return;
        }
        connection.Close();
    }

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
    ///     Executes the query, and returns the first column of the first row in the result
    ///     set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <param name="query">query script.</param>
    /// <returns>
    ///     The first column of the first row in the result set, or a null reference (Nothing
    ///     in Visual Basic) if the result set is empty. Returns a maximum of 2033 characters.
    /// </returns>
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
