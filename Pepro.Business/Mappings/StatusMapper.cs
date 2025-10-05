using Pepro.Business.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Mappings;

static class StatusMapper
{
    public static StatusDto ToDto(this Status entity)
    {
        return new StatusDto()
        {
            StatusId = entity.StatusId,
            Name = entity.Name,
        };
    }

    public static IEnumerable<StatusDto> ToDtos(this IEnumerable<Status> entities)
    {
        return entities.Select(entity => entity.ToDto());
    }
}
