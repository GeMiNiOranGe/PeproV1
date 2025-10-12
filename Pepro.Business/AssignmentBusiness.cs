using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class AssignmentBusiness
{
    private static AssignmentBusiness? _instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="AssignmentBusiness"/>.
    /// </summary>
    public static AssignmentBusiness Instance
    {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private AssignmentBusiness() { }

    /// <summary>
    /// Retrieves all assignments and maps them to view models.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="AssignmentView"/>.
    /// </returns>
    public IEnumerable<AssignmentView> GetAssignmentViews()
    {
        IEnumerable<Assignment> assignments =
            AssignmentDataAccess.Instance.GetMany();
        return MapAssignmentsToViews(assignments);
    }

    /// <summary>
    /// Searches assignments based on a search keyword and maps them to views.
    /// </summary>
    /// <param name="searchValue">
    /// The search keyword.
    /// </param>
    /// <returns>
    /// A collection of <see cref="AssignmentView"/> matching the search criteria.
    /// </returns>
    public IEnumerable<AssignmentView> SearchAssignmentViews(string searchValue)
    {
        IEnumerable<Assignment> assignments =
            AssignmentDataAccess.Instance.Search(searchValue);
        return MapAssignmentsToViews(assignments);
    }

    /// <summary>
    /// Retrieves assignments belonging to a specific project.
    /// </summary>
    /// <param name="projectId">
    /// The ID of the project.
    /// </param>
    /// <returns>
    /// A collection of <see cref="AssignmentDto"/>.
    /// </returns>
    public IEnumerable<AssignmentDto> GetAssignmentsByProjectId(int projectId)
    {
        IEnumerable<Assignment> assignments =
            AssignmentDataAccess.Instance.GetManyByProjectId(projectId);
        return assignments.ToDtos();
    }

    /// <summary>
    /// Retrieves progress information for assignments of a given project.
    /// </summary>
    /// <param name="projectId">
    /// The ID of the project.
    /// </param>
    /// <returns>
    /// A collection of <see cref="AssignmentProgressView"/>.
    /// </returns>
    public IEnumerable<AssignmentProgressView> GetAssignmentProgressViewsByProjectId(
        int projectId
    )
    {
        IEnumerable<Assignment> assignments =
            AssignmentDataAccess.Instance.GetManyByProjectId(projectId);
        return MapAssignmentsToProgressViews(assignments);
    }

    /// <summary>
    /// Retrieves progress information for assignments associated with a specific employee.
    /// </summary>
    /// <param name="employeeId">
    /// The ID of the employee.
    /// </param>
    /// <returns>
    /// A collection of <see cref="AssignmentProgressView"/>.
    /// </returns>
    public IEnumerable<AssignmentProgressView> GetAssignmentProgressViewsByEmployeeId(
        int employeeId
    )
    {
        IEnumerable<Assignment> assignments =
            AssignmentDataAccess.Instance.GetManyByEmployeeId(employeeId);
        return MapAssignmentsToProgressViews(assignments);
    }

    /// <summary>
    /// Retrieves the manager of a specific assignment.
    /// </summary>
    /// <param name="assignmentId">
    /// The ID of the assignment.
    /// </param>
    /// <returns>
    /// The <see cref="EmployeeDto"/> representing the manager, or <c>null</c> if not found.
    /// </returns>
    public EmployeeDto? GetAssignmentManager(int assignmentId)
    {
        Employee? employee = AssignmentDataAccess.Instance.GetManager(
            assignmentId
        );
        return employee?.ToDto();
    }

    /// <summary>
    /// Retrieves an assignment by the associated document ID.
    /// </summary>
    /// <param name="documentId">
    /// The document ID.
    /// </param>
    /// <returns>
    /// The <see cref="AssignmentDto"/> if found, otherwise <c>null</c>.
    /// </returns>
    public AssignmentDto? GetAssignmentByDocumentId(int documentId)
    {
        Assignment? assignment = AssignmentDataAccess.Instance.GetByDocumentId(
            documentId
        );
        return assignment?.ToDto();
    }

    /// <summary>
    /// Deletes an assignment by its ID.
    /// </summary>
    /// <param name="assignmentId">
    /// The ID of the assignment to delete.
    /// </param>
    /// <returns>
    /// The number of affected rows.
    /// </returns>
    public int DeleteAssignment(int assignmentId)
    {
        return AssignmentDataAccess.Instance.Delete(assignmentId);
    }

    /// <summary>
    /// Updates an existing assignment with new data.
    /// </summary>
    /// <param name="dto">
    /// The updated assignment data.
    /// </param>
    /// <returns>
    /// The number of affected rows, or 0 if not found.
    /// </returns>
    public int UpdateAssignment(AssignmentDto dto)
    {
        Assignment? entity = AssignmentDataAccess.Instance.GetById(
            dto.AssignmentId
        );
        if (entity == null)
        {
            return 0;
        }

        UpdateAssignmentModel model = new()
        {
            Name = new(dto.Name, entity.Name != dto.Name),
            IsPublicToProject = new(
                dto.IsPublicToProject,
                entity.IsPublicToProject != dto.IsPublicToProject
            ),
            IsPublicToDepartment = new(
                dto.IsPublicToDepartment,
                entity.IsPublicToDepartment != dto.IsPublicToDepartment
            ),
            StartDate = new(dto.StartDate, entity.StartDate != dto.StartDate),
            EndDate = new(dto.EndDate, entity.EndDate != dto.EndDate),
            RequiredDocumentCount = new(
                dto.RequiredDocumentCount,
                entity.RequiredDocumentCount != dto.RequiredDocumentCount
            ),
            ManagerId = new(dto.ManagerId, entity.ManagerId != dto.ManagerId),
            ProjectId = new(dto.ProjectId, entity.ProjectId != dto.ProjectId),
            StatusId = new(dto.StatusId, entity.StatusId != dto.StatusId),
        };
        return AssignmentDataAccess.Instance.Update(dto.AssignmentId, model);
    }

    /// <summary>
    /// Inserts a new assignment into the database.
    /// </summary>
    /// <param name="dto">
    /// The assignment data to insert.
    /// </param>
    /// <returns>
    /// The number of affected rows.
    /// </returns>
    public int InsertAssignment(AssignmentDto dto)
    {
        InsertAssignmentModel model = dto.ToInsertModel();
        return AssignmentDataAccess.Instance.Insert(model);
    }

    /// <summary>
    /// Maps a collection of <see cref="Assignment"/> entities to <see cref="AssignmentView"/> models.
    /// </summary>
    /// <param name="assignments">
    /// The assignments to map.
    /// </param>
    /// <returns>
    /// A collection of <see cref="AssignmentView"/>.
    /// </returns>
    private IEnumerable<AssignmentView> MapAssignmentsToViews(
        IEnumerable<Assignment> assignments
    )
    {
        IEnumerable<int> managerIds = assignments
            .Select(a => a.ManagerId)
            .OfType<int>()
            .Distinct();

        Dictionary<int, string> managers = EmployeeBusiness
            .Instance.GetEmployeesByEmployeeIds(managerIds)
            .ToDictionary(e => e.EmployeeId, e => e.FullName);

        IEnumerable<int> projectIds = assignments
            .Select(a => a.ProjectId)
            .Distinct();

        Dictionary<int, string> projects = ProjectDataAccess
            .Instance.GetManyByIds(projectIds)
            .ToDictionary(e => e.ProjectId, e => e.Name);

        Dictionary<int, string> statuses = StatusDataAccess
            .Instance.GetMany()
            .ToDictionary(s => s.StatusId, s => s.Name);

        return assignments.Select(assignment =>
        {
            string? managerFullName = null;
            if (assignment.ManagerId.HasValue)
            {
                managers.TryGetValue(
                    assignment.ManagerId.Value,
                    out managerFullName
                );
            }

            projects.TryGetValue(assignment.ProjectId, out string? projectName);
            statuses.TryGetValue(assignment.StatusId, out string? statusName);

            return new AssignmentView()
            {
                AssignmentId = assignment.AssignmentId,
                Name = assignment.Name,
                IsPublicToProject = assignment.IsPublicToProject,
                IsPublicToDepartment = assignment.IsPublicToDepartment,
                StartDate = assignment.StartDate,
                EndDate = assignment.EndDate,
                RequiredDocumentCount = assignment.RequiredDocumentCount,
                ManagerId = assignment.ManagerId,
                ProjectId = assignment.ProjectId,
                StatusId = assignment.StatusId,
                ManagerFullName = managerFullName ?? "",
                ProjectName = projectName ?? "",
                StatusName = statusName ?? "",
            };
        });
    }

    /// <summary>
    /// Maps <see cref="Assignment"/> entities to <see cref="AssignmentProgressView"/> models,
    /// calculating document completion percentage for each assignment.
    /// </summary>
    /// <param name="assignments">
    /// The list of assignments to map.
    /// </param>
    /// <returns>
    /// A collection of <see cref="AssignmentProgressView"/>.
    /// </returns>
    private IEnumerable<AssignmentProgressView> MapAssignmentsToProgressViews(
        IEnumerable<Assignment> assignments
    )
    {
        IEnumerable<int> assignmentIds = assignments
            .Select(assignment => assignment.AssignmentId)
            .Distinct();

        Dictionary<int, int> documentCounts = AssignmentDataAccess
            .Instance.CountDocumentsByAssignmentIds(assignmentIds)
            .ToDictionary(
                item => item.AssignmentId,
                item => item.DocumentCount
            );

        return assignments.Select(assignment =>
        {
            documentCounts.TryGetValue(
                assignment.AssignmentId,
                out int documentCount
            );
            int requiredDocumentCount = assignment.RequiredDocumentCount;
            decimal percent =
                requiredDocumentCount != 0
                    ? Math.Round(
                        documentCount * 100m / requiredDocumentCount,
                        2
                    )
                    : 0;

            return new AssignmentProgressView
            {
                AssignmentId = assignment.AssignmentId,
                Name = assignment.Name,
                IsPublicToProject = assignment.IsPublicToProject,
                IsPublicToDepartment = assignment.IsPublicToDepartment,
                StartDate = assignment.StartDate,
                EndDate = assignment.EndDate,
                RequiredDocumentCount = assignment.RequiredDocumentCount,
                ManagerId = assignment.ManagerId,
                ProjectId = assignment.ProjectId,
                StatusId = assignment.StatusId,
                ProgressPercent = percent,
            };
        });
    }
}
