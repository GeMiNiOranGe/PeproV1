using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class DocumentBusiness
{
    private static DocumentBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of the <see cref="DocumentBusiness"/> class.
    /// </summary>
    public static DocumentBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private DocumentBusiness() { }

    /// <summary>
    /// Retrieves all documents.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="DocumentDto"/> representing all documents.
    /// </returns>
    public IEnumerable<DocumentDto> GetDocuments()
    {
        IEnumerable<Document> documents = DocumentDataAccess.Instance.GetMany();
        return documents.ToDtos();
    }

    /// <summary>
    /// Searches documents by a given keyword.
    /// </summary>
    /// <param name="searchValue">
    /// The search keyword used to filter documents.
    /// </param>
    /// <returns>
    /// A filtered collection of <see cref="DocumentDto"/>.
    /// </returns>
    public IEnumerable<DocumentDto> SearchDocuments(string searchValue)
    {
        IEnumerable<Document> documents = DocumentDataAccess.Instance.Search(
            searchValue
        );
        return documents.ToDtos();
    }

    /// <summary>
    /// Deletes a document by its ID.
    /// </summary>
    /// <param name="documentId">
    /// The ID of the document to delete.
    /// </param>
    /// <returns>
    /// The number of affected rows.
    /// </returns>
    public int DeleteDocument(int documentId)
    {
        return DocumentDataAccess.Instance.Delete(documentId);
    }
}
