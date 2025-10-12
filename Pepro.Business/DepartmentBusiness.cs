using System.Data;
using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class DepartmentBusiness
{
    private static DepartmentBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of the <see cref="DepartmentBusiness"/> class.
    /// </summary>
    public static DepartmentBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private DepartmentBusiness() { }

    /// <summary>
    /// Retrieves a department by its ID.
    /// </summary>
    /// <param name="departmentID">
    /// The ID of the department to retrieve.
    /// </param>
    /// <returns>
    /// The corresponding <see cref="DepartmentDto"/> if found; otherwise, null.
    /// </returns>
    public DepartmentDto? GetDepartmentByDepartmentId(int departmentID)
    {
        Department? department = DepartmentDataAccess.Instance.GetById(
            departmentID
        );
        return department?.ToDto();
    }

    /// <summary>
    /// Retrieves all departments.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="DepartmentDto"/> representing all departments.
    /// </returns>
    public IEnumerable<DepartmentDto> GetDepartments()
    {
        IEnumerable<Department> departments =
            DepartmentDataAccess.Instance.GetMany();
        return departments.ToDtos();
    }

    /// <summary>
    /// Retrieves department views containing additional details such as manager name.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="DepartmentView"/>.
    /// </returns>
    public IEnumerable<DepartmentView> GetDepartmentViews()
    {
        IEnumerable<Department> departments =
            DepartmentDataAccess.Instance.GetMany();
        return MapDepartmentsToViews(departments);
    }

    /// <summary>
    /// Searches departments based on a search value and returns view models.
    /// </summary>
    /// <param name="searchValue">
    /// The search keyword used to filter departments.
    /// </param>
    /// <returns>
    /// A filtered collection of <see cref="DepartmentView"/>.
    /// </returns>
    public IEnumerable<DepartmentView> SearchDepartmentViews(string searchValue)
    {
        IEnumerable<Department> departments =
            DepartmentDataAccess.Instance.Search(searchValue);
        return MapDepartmentsToViews(departments);
    }

    /// <summary>
    /// Maps department entities to department view models, enriching them
    /// with manager information.
    /// </summary>
    /// <param name="departments">
    /// The collection of department entities to map.
    /// </param>
    /// <returns>
    /// A collection of <see cref="DepartmentView"/> containing manager details.
    /// </returns>
    private IEnumerable<DepartmentView> MapDepartmentsToViews(
        IEnumerable<Department> departments
    )
    {
        IEnumerable<int> managerIds = departments
            .Select(d => d.ManagerId)
            .OfType<int>()
            .Distinct();

        Dictionary<int, string> managers = EmployeeBusiness
            .Instance.GetEmployeesByEmployeeIds(managerIds)
            .ToDictionary(e => e.EmployeeId, e => e.FullName);

        return departments.Select(department => new DepartmentView()
        {
            DepartmentId = department.DepartmentId,
            Name = department.Name,
            ManagerId = department.ManagerId,
            ManagerFullName =
                department.ManagerId.HasValue
                && managers.TryGetValue(
                    department.ManagerId.Value,
                    out string? managerFullName
                )
                    ? managerFullName
                    : "",
        });
    }

    /// <summary>
    /// Deletes a department by its ID.
    /// </summary>
    /// <param name="departmentId">
    /// The ID of the department to delete.
    /// </param>
    /// <returns>
    /// The number of affected rows.
    /// </returns>
    public int DeleteDepartment(int departmentId)
    {
        return DepartmentDataAccess.Instance.Delete(departmentId);
    }

    /// <summary>
    /// Updates department information.
    /// </summary>
    /// <param name="dto">
    /// The department data transfer object containing updated values.
    /// </param>
    /// <returns>
    /// The number of affected rows; returns 0 if the department does not exist.
    /// </returns>
    public int UpdateDepartment(DepartmentDto dto)
    {
        Department? entity = DepartmentDataAccess.Instance.GetById(
            dto.DepartmentId
        );
        if (entity == null)
        {
            return 0;
        }

        UpdateDepartmentModel model = new()
        {
            Name = new(dto.Name, entity.Name != dto.Name),
            ManagerId = new(dto.ManagerId, entity.ManagerId != dto.ManagerId),
        };
        return DepartmentDataAccess.Instance.Update(dto.DepartmentId, model);
    }

    /// <summary>
    /// Inserts a new department record.
    /// </summary>
    /// <param name="dto">
    /// The department data to insert.
    /// </param>
    /// <returns>
    /// The number of affected rows.
    /// </returns>
    public int InsertDepartment(DepartmentDto dto)
    {
        InsertDepartmentModel model = dto.ToInsertModel();
        return DepartmentDataAccess.Instance.Insert(model);
    }
}
