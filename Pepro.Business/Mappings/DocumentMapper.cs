using Pepro.Business.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Mappings;

static class DocumentMapper
{
    public static DocumentDto ToDto(this Document entity)
    {
        return new DocumentDto()
        {
            DocumentId = entity.DocumentId,
            Title = entity.Title,
            CreateAt = entity.CreateAt,
            RevisionNumber = entity.RevisionNumber,
            RevisionStatus = entity.RevisionStatus,
            DocumentUrl = entity.DocumentUrl,
            NativeFileFormat = entity.NativeFileFormat,
            PreparedBy = entity.PreparedBy,
            CheckedBy = entity.CheckedBy,
            ApprovedBy = entity.ApprovedBy,
            AssignmentId = entity.AssignmentId,
        };
    }

    public static IEnumerable<DocumentDto> ToDtos(
        this IEnumerable<Document> entities
    )
    {
        return entities.Select(entity => entity.ToDto());
    }
}
