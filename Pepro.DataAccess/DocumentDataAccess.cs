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

    /// <summary>
    /// Gets the singleton instance of <see cref="DocumentDataAccess"/>.
    /// </summary>
    public static DocumentDataAccess Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private DocumentDataAccess() { }

    /// <summary>
    /// Retrieves all documents from the database.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Document"/> objects representing all active documents.
    /// </returns>
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

    /// <summary>
    /// Searches for documents based on a given search value.
    /// </summary>
    /// <param name="searchValue">
    /// The text value to match against document ID, title, or assignment ID.
    /// </param>
    /// <returns>
    /// A collection of <see cref="Document"/> objects that match the search criteria.
    /// </returns>
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

    /// <summary>
    /// Deletes a document from the database.
    /// </summary>
    /// <param name="documentId">
    /// The ID of the document to be deleted.
    /// </param>
    /// <returns>
    /// The number of rows affected by the delete operation.
    /// </returns>
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
