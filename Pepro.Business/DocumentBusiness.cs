using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class DocumentBusiness
{
    private static DocumentBusiness? _instance;

    public static DocumentBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private DocumentBusiness() { }

    public IEnumerable<DocumentDto> GetDocuments()
    {
        IEnumerable<Document> documents = DocumentDataAccess.Instance.GetMany();
        return documents.ToDtos();
    }

    public IEnumerable<DocumentDto> SearchDocuments(string searchValue)
    {
        IEnumerable<Document> documents = DocumentDataAccess.Instance.Search(
            searchValue
        );
        return documents.ToDtos();
    }

    public int DeleteDocument(int documentId)
    {
        return DocumentDataAccess.Instance.Delete(documentId);
    }
}
