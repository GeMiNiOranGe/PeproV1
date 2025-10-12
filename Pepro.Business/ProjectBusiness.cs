using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class ProjectBusiness
{
    private static ProjectBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="ProjectBusiness"/>.
    /// </summary>
    public static ProjectBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private ProjectBusiness() { }

    /// <summary>
    /// Retrieves a specific project by its unique project ID.
    /// </summary>
    /// <param name="projectId">
    /// The unique identifier of the project to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="ProjectDto"/> representing the project if found; otherwise, null.
    /// </returns>
    public ProjectDto? GetProjectByProjectId(int projectId)
    {
        Project? project = ProjectDataAccess.Instance.GetById(projectId);
        return project?.ToDto();
    }

    /// <summary>
    /// Retrieves all projects from the data access layer and converts them to DTOs.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="ProjectDto"/> representing all projects.
    /// </returns>
    public IEnumerable<ProjectDto> GetProjects()
    {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.GetMany();
        return projects.ToDtos();
    }

    /// <summary>
    /// Retrieves all projects and maps them to project views with related information.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="ProjectView"/> containing detailed project information.
    /// </returns>
    public IEnumerable<ProjectView> GetProjectViews()
    {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.GetMany();
        return MapProjectsToViews(projects);
    }

    /// <summary>
    /// Searches for projects based on a keyword and maps results to project views.
    /// </summary>
    /// <param name="searchValue">
    /// The search keyword used to filter projects.
    /// </param>
    /// <returns>
    /// A collection of <see cref="ProjectView"/> matching the search criteria.
    /// </returns>
    public IEnumerable<ProjectView> SearchProjectViews(string searchValue)
    {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.Search(
            searchValue
        );
        return MapProjectsToViews(projects);
    }

    /// <summary>
    /// Calculates progress information for each project based on assignment completion.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="ProjectProgressView"/> containing project progress percentages.
    /// </returns>
    public IEnumerable<ProjectProgressView> GetProjectProgressViews()
    {
        IEnumerable<Project> projects = ProjectDataAccess.Instance.GetMany();
        return projects.Select(project =>
        {
            IEnumerable<Assignment> assignments =
                AssignmentDataAccess.Instance.GetManyByProjectId(
                    project.ProjectId
                );
            int total = assignments.Count();
            int completed = assignments.Count(assignment =>
                assignment.StatusId == 4
            );
            decimal percent =
                total != 0 ? Math.Round(completed * 100m / total, 2) : 0;

            return new ProjectProgressView
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                CustomerName = project.CustomerName,
                ManagerId = project.ManagerId,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                StatusId = project.StatusId,
                ProgressPercent = percent,
            };
        });
    }

    /// <summary>
    /// Retrieves all project names associated with a specific employee ID.
    /// </summary>
    /// <param name="employeeId">
    /// The unique identifier of the employee whose projects are retrieved.
    /// </param>
    /// <returns>
    /// An array of project names linked to the given employee.
    /// </returns>
    public string[] GetProjectNamesByEmployeeId(int employeeId)
    {
        IEnumerable<Project> projects =
            ProjectDataAccess.Instance.GetManyByEmployeeId(employeeId);
        return [.. projects.Select(project => project.Name)];
    }

    /// <summary>
    /// Retrieves the project associated with a specific assignment ID.
    /// </summary>
    /// <param name="assignmentId">
    /// The unique identifier of the assignment.
    /// </param>
    /// <returns>
    /// A <see cref="ProjectDto"/> representing the related project if found; otherwise, null.
    /// </returns>
    public ProjectDto? GetProjectByAssignmentId(int assignmentId)
    {
        Project? project = ProjectDataAccess.Instance.GetByAssignmentId(
            assignmentId
        );
        return project?.ToDto();
    }

    /// <summary>
    /// Deletes a project by its unique identifier.
    /// </summary>
    /// <param name="projectId">
    /// The ID of the project to delete.
    /// </param>
    /// <returns>
    /// The number of records affected by the delete operation.
    /// </returns>
    public int DeleteProject(int projectId)
    {
        return ProjectDataAccess.Instance.Delete(projectId);
    }

    /// <summary>
    /// Updates an existing project record based on the provided DTO.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing updated project information.
    /// </param>
    /// <returns>
    /// The number of records affected by the update operation.
    /// </returns>
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
            CustomerName = new(
                dto.CustomerName,
                entity.CustomerName != dto.CustomerName
            ),
            ManagerId = new(dto.ManagerId, entity.ManagerId != dto.ManagerId),
            StartDate = new(dto.StartDate, entity.StartDate != dto.StartDate),
            EndDate = new(dto.EndDate, entity.EndDate != dto.EndDate),
            StatusId = new(dto.StatusId, entity.StatusId != dto.StatusId),
        };
        return ProjectDataAccess.Instance.Update(dto.ProjectId, model);
    }

    /// <summary>
    /// Inserts a new project record based on the provided DTO.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing project information to insert.
    /// </param>
    /// <returns>
    /// The number of records affected by the insert operation.
    /// </returns>
    public int InsertProject(ProjectDto dto)
    {
        InsertProjectModel entity = dto.ToInsertModel();
        return ProjectDataAccess.Instance.Insert(entity);
    }

    /// <summary>
    /// Maps project entities to view models containing additional related information such as manager and status names.
    /// </summary>
    /// <param name="projects">
    /// The collection of project entities to map.
    /// </param>
    /// <returns>
    /// A collection of <see cref="ProjectView"/> with enriched project information.
    /// </returns>
    private IEnumerable<ProjectView> MapProjectsToViews(
        IEnumerable<Project> projects
    )
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
