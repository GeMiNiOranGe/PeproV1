namespace Pepro.Business.Contracts;

public class DocumentDto
{
    private int _documentId;
    private string _title = "";
    private DateTime _createAt;
    private int _revisionNumber;
    private string _revisionStatus = "";
    private string _documentUrl = "";
    private string _nativeFileFormat = "";
    private string? _preparedBy;
    private string? _checkedBy;
    private string? _approvedBy;
    private int _assignmentId;

    public int DocumentId
    {
        get => _documentId;
        set => _documentId = value;
    }

    public string Title
    {
        get => _title;
        set => _title = value;
    }

    public DateTime CreateAt
    {
        get => _createAt;
        set => _createAt = value;
    }

    public int RevisionNumber
    {
        get => _revisionNumber;
        set => _revisionNumber = value;
    }

    public string RevisionStatus
    {
        get => _revisionStatus;
        set => _revisionStatus = value;
    }

    public string DocumentUrl
    {
        get => _documentUrl;
        set => _documentUrl = value;
    }

    public string NativeFileFormat
    {
        get => _nativeFileFormat;
        set => _nativeFileFormat = value;
    }

    public string? PreparedBy
    {
        get => _preparedBy;
        set => _preparedBy = value;
    }

    public string? CheckedBy
    {
        get => _checkedBy;
        set => _checkedBy = value;
    }

    public string? ApprovedBy
    {
        get => _approvedBy;
        set => _approvedBy = value;
    }

    public int AssignmentId
    {
        get => _assignmentId;
        set => _assignmentId = value;
    }
}
