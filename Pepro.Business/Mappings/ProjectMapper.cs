using Pepro.Business.Contracts;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Mappings;

static class ProjectMapper
{
    public static InsertProjectModel ToInsertModel(this ProjectDto dto)
    {
        return new InsertProjectModel()
        {
            Name = dto.Name,
            CustomerName = dto.CustomerName,
            ManagerId = dto.ManagerId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            StatusId = dto.StatusId,
        };
    }

    public static ProjectDto ToDto(this Project entity)
    {
        return new ProjectDto()
        {
            ProjectId = entity.ProjectId,
            Name = entity.Name,
            CustomerName = entity.CustomerName,
            ManagerId = entity.ManagerId,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            StatusId = entity.StatusId,
        };
    }

    public static IEnumerable<ProjectDto> ToDtos(
        this IEnumerable<Project> entities
    )
    {
        return entities.Select(entity => entity.ToDto());
    }
}
