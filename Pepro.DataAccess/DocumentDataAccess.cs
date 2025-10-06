using System.Data;
using Microsoft.Data.SqlClient;
using Pepro.DataAccess.Entities;
using Pepro.DataAccess.Extensions;
using Pepro.DataAccess.Mappings;
using Pepro.DataAccess.Utilities;

namespace Pepro.DataAccess;

public class DocumentDataAccess
{
    private static DocumentDataAccess? _instance;

    public static DocumentDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private DocumentDataAccess() { }

    public IEnumerable<Document> GetMany()
    {
        string query = """
            SELECT Document.DocumentId
                , Document.Title
                , Document.CreateAt
                , Document.RevisionNumber
                , Document.RevisionStatus
                , Document.DocumentUrl
                , Document.NativeFileFormat
                , Document.PreparedBy
                , Document.CheckedBy
                , Document.ApprovedBy
                , Document.AssignmentId
                , Document.IsDeleted
            FROM Document
            WHERE Document.IsDeleted = 0
            """;

        return DataProvider
            .Instance.ExecuteQuery(query)
            .MapMany(DocumentMapper.FromDataRow);
    }

    public IEnumerable<Document> Search(string searchValue)
    {
        string query = """
            SELECT Document.DocumentId
                , Document.Title
                , Document.CreateAt
                , Document.RevisionNumber
                , Document.RevisionStatus
                , Document.DocumentUrl
                , Document.NativeFileFormat
                , Document.PreparedBy
                , Document.CheckedBy
                , Document.ApprovedBy
                , Document.AssignmentId
                , Document.IsDeleted
            FROM Document
            WHERE
                (
                    Document.DocumentId LIKE '%' + @SearchValue + '%'
                    OR Document.Title LIKE '%' + @SearchValue + '%'
                    OR Document.AssignmentId LIKE '%' + @SearchValue + '%'
                )
                AND Document.IsDeleted = 0
            """;
        List<SqlParameter> parameters = [];
        parameters.Add(
            "SearchValue",
            SqlDbType.NVarChar,
            DatabaseConstants.SEARCH_SIZE,
            searchValue
        );

        return DataProvider
            .Instance.ExecuteQuery(query, [.. parameters])
            .MapMany(DocumentMapper.FromDataRow);
    }

    public int Delete(int documentId)
    {
        string query = """
            UPDATE Document
            SET IsDeleted = 1
            WHERE DocumentId = @DocumentId
            """;
        List<SqlParameter> parameters = [];
        parameters.Add("DocumentId", SqlDbType.Int, documentId);

        return DataProvider.Instance.ExecuteNonQuery(query, [.. parameters]);
    }
}
