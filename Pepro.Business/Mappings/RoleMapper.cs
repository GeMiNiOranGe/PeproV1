using Pepro.Business.Contracts;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Mappings;

static class RoleMapper
{
    public static InsertRoleModel ToInsertModel(this RoleDto dto)
    {
        return new InsertRoleModel()
        {
            Name = dto.Name,
        };
    }

    public static RoleDto ToDto(this Role entity)
    {
        return new RoleDto()
        {
            RoleId = entity.RoleId,
            Name = entity.Name,
        };
    }

    public static IEnumerable<RoleDto> ToDtos(this IEnumerable<Role> entities)
    {
        return entities.Select(entity => entity.ToDto());
    }
}
