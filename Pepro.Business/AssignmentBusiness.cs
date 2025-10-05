using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class AssignmentBusiness {
    private static AssignmentBusiness? _instance;

    public static AssignmentBusiness Instance {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private AssignmentBusiness() { }

    public IEnumerable<AssignmentView> GetAssignmentViews()
    {
        IEnumerable<Assignment> assignments = AssignmentDataAccess.Instance.GetMany();
        return MapAssignmentsToViews(assignments);
    }

    public IEnumerable<AssignmentView> SearchAssignmentViews(string searchValue)
    {
        IEnumerable<Assignment> assignments = AssignmentDataAccess.Instance.Search(searchValue);
        return MapAssignmentsToViews(assignments);
    }

    public IEnumerable<AssignmentDto> GetAssignmentsByProjectId(int projectId) {
        IEnumerable<Assignment> assignments = AssignmentDataAccess.Instance.GetManyByProjectId(projectId);
        return assignments.ToDtos();
    }

    public IEnumerable<AssignmentProgressView> GetAssignmentProgressViewsByProjectId(int projectId) {
        IEnumerable<Assignment> assignments = AssignmentDataAccess.Instance.GetManyByProjectId(projectId);
        return MapAssignmentsToProgressViews(assignments);
    }

    public IEnumerable<AssignmentProgressView> GetAssignmentProgressViewsByEmployeeId(int employeeId) {
        IEnumerable<Assignment> assignments = AssignmentDataAccess.Instance.GetManyByEmployeeId(employeeId);
        return MapAssignmentsToProgressViews(assignments);
    }

    public EmployeeDto? GetAssignmentManager(int assignmentId) {
        Employee? employee = AssignmentDataAccess.Instance.GetManager(assignmentId);
        return employee?.ToDto();
    }

    public AssignmentDto? GetAssignmentByDocumentId(int documentId) {
        Assignment? assignment = AssignmentDataAccess.Instance.GetByDocumentId(documentId);
        return assignment?.ToDto();
    }

    public int DeleteAssignment(int assignmentId) {
        return AssignmentDataAccess.Instance.Delete(assignmentId);
    }

    public int UpdateAssignment(AssignmentDto dto)
    {
        Assignment? entity = AssignmentDataAccess.Instance.GetById(dto.AssignmentId);
        if (entity == null)
        {
            return 0;
        }

        UpdateAssignmentModel model = new()
        {
            Name = new(dto.Name, entity.Name != dto.Name),
            IsPublicToProject = new(dto.IsPublicToProject, entity.IsPublicToProject != dto.IsPublicToProject),
            IsPublicToDepartment = new(dto.IsPublicToDepartment, entity.IsPublicToDepartment != dto.IsPublicToDepartment),
            StartDate = new(dto.StartDate, entity.StartDate != dto.StartDate),
            EndDate = new(dto.EndDate, entity.EndDate != dto.EndDate),
            RequiredDocumentCount = new(dto.RequiredDocumentCount, entity.RequiredDocumentCount != dto.RequiredDocumentCount),
            ManagerId = new(dto.ManagerId, entity.ManagerId != dto.ManagerId),
            ProjectId = new(dto.ProjectId, entity.ProjectId != dto.ProjectId),
            StatusId = new(dto.StatusId, entity.StatusId != dto.StatusId),
        };
        return AssignmentDataAccess.Instance.Update(dto.AssignmentId, model);
    }

    public int InsertAssignment(AssignmentDto dto)
    {
        InsertAssignmentModel model = dto.ToInsertModel();
        return AssignmentDataAccess.Instance.Insert(model);
    }

    private IEnumerable<AssignmentView> MapAssignmentsToViews(IEnumerable<Assignment> assignments)
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

    private IEnumerable<AssignmentProgressView> MapAssignmentsToProgressViews(IEnumerable<Assignment> assignments)
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
            decimal percent = requiredDocumentCount != 0
                ? Math.Round(documentCount * 100m / requiredDocumentCount, 2)
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
                ProgressPercent = percent
            };
        });
    }
}
