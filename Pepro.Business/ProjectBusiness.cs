using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class ProjectBusiness {
    private static ProjectBusiness? _instance;

    public static ProjectBusiness Instance {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private ProjectBusiness() { }

    public ProjectDto? GetProjectByProjectId(int projectId) {
        Project? project = ProjectDataAccess.Instance.GetById(projectId);
        return project?.ToDto();
    }

    public IEnumerable<ProjectDto> GetProjects() {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.GetMany();
        return projects.ToDtos();
    }

    public IEnumerable<ProjectView> GetProjectViews()
    {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.GetMany();
        return MapProjectsToViews(projects);
    }

    public IEnumerable<ProjectView> SearchProjectViews(string searchValue)
    {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.Search(searchValue);
        return MapProjectsToViews(projects);
    }

    public IEnumerable<ProjectProgressView> GetProjectProgressViews()
    {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.GetMany();
        return projects.Select(project =>
        {
            IEnumerable<Assignment> assignments = AssignmentDataAccess.Instance.GetManyByProjectId(project.ProjectId);
            int total = assignments.Count();
            int completed = assignments.Count(assignment => assignment.StatusId == 4);
            decimal percent = total != 0
                ? Math.Round(completed * 100m / total, 2)
                : 0;

            return new ProjectProgressView
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                CustomerName = project.CustomerName,
                ManagerId = project.ManagerId,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                StatusId = project.StatusId,
                ProgressPercent = percent
            };
        });
    }

    public string[] GetProjectNamesByEmployeeId(int employeeId) {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.GetManyByEmployeeId(employeeId);
        return [.. projects.Select(project => project.Name)];
    }

    public ProjectDto? GetProjectByAssignmentId(int assignmentId) {
        Project? project = ProjectDataAccess.Instance.GetByAssignmentId(assignmentId);
        return project?.ToDto();
    }

    public int DeleteProject(int projectId)
    {
        return ProjectDataAccess.Instance.Delete(projectId);
    }
    
    public int UpdateProject(ProjectDto dto)
    {
        Project? entity = ProjectDataAccess.Instance.GetById(dto.ProjectId);
        if (entity == null)
        {
            return 0;
        }

        UpdateProjectModel model = new()
        {
            Name = new(dto.Name, entity.Name != dto.Name),
            CustomerName = new(dto.CustomerName, entity.CustomerName != dto.CustomerName),
            ManagerId = new(dto.ManagerId, entity.ManagerId != dto.ManagerId),
            StartDate = new(dto.StartDate, entity.StartDate != dto.StartDate),
            EndDate = new(dto.EndDate, entity.EndDate != dto.EndDate),
            StatusId = new(dto.StatusId, entity.StatusId != dto.StatusId),
        };
        return ProjectDataAccess.Instance.Update(dto.ProjectId, model);
    }

    public int InsertProject(ProjectDto dto)
    {
        InsertProjectModel entity = dto.ToInsertModel();
        return ProjectDataAccess.Instance.Insert(entity);
    }

    private IEnumerable<ProjectView> MapProjectsToViews(IEnumerable<Project> projects)
    {
        IEnumerable<int> managerIds = projects
            .Select(p => p.ManagerId)
            .OfType<int>()
            .Distinct();

        Dictionary<int, string> managers = EmployeeBusiness
            .Instance.GetEmployeesByEmployeeIds(managerIds)
            .ToDictionary(e => e.EmployeeId, e => e.FullName);

        Dictionary<int, string> statuses = StatusDataAccess
            .Instance.GetMany()
            .ToDictionary(s => s.StatusId, s => s.Name);

        return projects.Select(project => new ProjectView()
        {
            ProjectId = project.ProjectId,
            Name = project.Name,
            CustomerName = project.CustomerName,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ManagerId = project.ManagerId,
            StatusId = project.StatusId,
            ManagerFullName =
                project.ManagerId.HasValue
                && managers.TryGetValue(
                    project.ManagerId.Value,
                    out var managerFullName
                )
                    ? managerFullName
                    : "",
            StatusName = statuses.TryGetValue(
                project.StatusId,
                out var statusName
            )
                ? statusName
                : "",
        });
    }
}
