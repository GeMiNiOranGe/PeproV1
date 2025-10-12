using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class RoleBusiness
{
    private static RoleBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="RoleBusiness"/>.
    /// </summary>
    public static RoleBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private RoleBusiness() { }

    /// <summary>
    /// Retrieves all roles from the data access layer and converts them to DTOs.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="RoleDto"/> representing all roles.
    /// </returns>
    public IEnumerable<RoleDto> GetRoles()
    {
        IEnumerable<Role> roles = RoleDataAccess.Instance.GetMany();
        return roles.ToDtos();
    }

    /// <summary>
    /// Searches for roles based on a provided keyword and converts results to DTOs.
    /// </summary>
    /// <param name="searchValue">
    /// The search keyword used to filter roles.
    /// </param>
    /// <returns>
    /// A collection of <see cref="RoleDto"/> matching the search criteria.
    /// </returns>
    public IEnumerable<RoleDto> SearchRoles(string searchValue)
    {
        IEnumerable<Role> roles = RoleDataAccess.Instance.Search(searchValue);
        return roles.ToDtos();
    }

    /// <summary>
    /// Inserts a new role record based on the provided DTO.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing role information to insert.
    /// </param>
    /// <returns>
    /// The number of records affected by the insert operation.
    /// </returns>
    public int InsertRole(RoleDto dto)
    {
        InsertRoleModel entity = dto.ToInsertModel();
        return RoleDataAccess.Instance.Insert(entity);
    }

    /// <summary>
    /// Updates an existing role based on the provided DTO.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing updated role information.
    /// </param>
    /// <returns>
    /// The number of records affected by the update operation.
    /// </returns>
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

    /// <summary>
    /// Deletes a role by its unique identifier.
    /// </summary>
    /// <param name="roleId">
    /// The unique identifier of the role to delete.
    /// </param>
    /// <returns>
    /// The number of records affected by the delete operation.
    /// </returns>
    public int DeleteRole(int roleId)
    {
        return RoleDataAccess.Instance.Delete(roleId);
    }
}
