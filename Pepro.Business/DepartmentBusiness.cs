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

    public static DepartmentBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private DepartmentBusiness() { }

    public DepartmentDto? GetDepartmentByDepartmentId(int departmentID)
    {
        Department? department = DepartmentDataAccess.Instance.GetById(
            departmentID
        );
        return department?.ToDto();
    }

    public IEnumerable<DepartmentDto> GetDepartments()
    {
        IEnumerable<Department> departments =
            DepartmentDataAccess.Instance.GetMany();
        return departments.ToDtos();
    }

    public IEnumerable<DepartmentView> GetDepartmentViews()
    {
        IEnumerable<Department> departments =
            DepartmentDataAccess.Instance.GetMany();
        return MapDepartmentsToViews(departments);
    }

    public IEnumerable<DepartmentView> SearchDepartmentViews(string searchValue)
    {
        IEnumerable<Department> departments =
            DepartmentDataAccess.Instance.Search(searchValue);
        return MapDepartmentsToViews(departments);
    }

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

    public int DeleteDepartment(int departmentId)
    {
        return DepartmentDataAccess.Instance.Delete(departmentId);
    }

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

    public int InsertDepartment(DepartmentDto dto)
    {
        InsertDepartmentModel model = dto.ToInsertModel();
        return DepartmentDataAccess.Instance.Insert(model);
    }
}
