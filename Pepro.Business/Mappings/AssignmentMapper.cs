using Pepro.Business.Contracts;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Mappings;

static class AssignmentMapper
{
    public static InsertAssignmentModel ToInsertModel(this AssignmentDto dto)
    {
        return new InsertAssignmentModel()
        {
            Name = dto.Name,
            IsPublicToProject = dto.IsPublicToProject,
            IsPublicToDepartment = dto.IsPublicToDepartment,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            RequiredDocumentCount = dto.RequiredDocumentCount,
            ManagerId = dto.ManagerId,
            ProjectId = dto.ProjectId,
            StatusId = dto.StatusId,
        };
    }

    public static AssignmentDto ToDto(this Assignment entity)
    {
        return new AssignmentDto()
        {
            AssignmentId = entity.AssignmentId,
            Name = entity.Name,
            IsPublicToProject = entity.IsPublicToProject,
            IsPublicToDepartment = entity.IsPublicToDepartment,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            RequiredDocumentCount = entity.RequiredDocumentCount,
            ManagerId = entity.ManagerId,
            ProjectId = entity.ProjectId,
            StatusId = entity.StatusId,
        };
    }

    public static IEnumerable<AssignmentDto> ToDtos(this IEnumerable<Assignment> entities)
    {
        return entities.Select(entity => entity.ToDto());
    }
}
