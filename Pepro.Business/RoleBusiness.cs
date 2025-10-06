using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class RoleBusiness
{
    private static RoleBusiness? _instance;

    public static RoleBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private RoleBusiness() { }

    public IEnumerable<RoleDto> GetRoles()
    {
        IEnumerable<Role> roles = RoleDataAccess.Instance.GetMany();
        return roles.ToDtos();
    }

    public IEnumerable<RoleDto> SearchRoles(string searchValue)
    {
        IEnumerable<Role> roles = RoleDataAccess.Instance.Search(searchValue);
        return roles.ToDtos();
    }

    public int InsertRole(RoleDto dto)
    {
        InsertRoleModel entity = dto.ToInsertModel();
        return RoleDataAccess.Instance.Insert(entity);
    }

    public int UpdateRole(RoleDto dto)
    {
        Role? entity = RoleDataAccess.Instance.GetById(dto.RoleId);
        if (entity == null)
        {
            return 0;
        }

        UpdateRoleModel model = new()
        {
            Name = new(dto.Name, entity.Name != dto.Name),
        };
        return RoleDataAccess.Instance.Update(dto.RoleId, model);
    }

    public int DeleteRole(int roleId)
    {
        return RoleDataAccess.Instance.Delete(roleId);
    }
}
