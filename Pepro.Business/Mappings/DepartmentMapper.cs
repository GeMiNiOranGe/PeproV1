using Pepro.Business.Contracts;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Mappings;

static class DepartmentMapper
{
    public static InsertDepartmentModel ToInsertModel(this DepartmentDto dto)
    {
        return new InsertDepartmentModel()
        {
            Name = dto.Name,
            ManagerId = dto.ManagerId,
        };
    }

    public static DepartmentDto ToDto(this Department entity)
    {
        return new DepartmentDto()
        {
            DepartmentId = entity.DepartmentId,
            Name = entity.Name,
            ManagerId = entity.ManagerId,
        };
    }

    public static IEnumerable<DepartmentDto> ToDtos(this IEnumerable<Department> entities)
    {
        return entities.Select(entity => entity.ToDto());
    }
}
