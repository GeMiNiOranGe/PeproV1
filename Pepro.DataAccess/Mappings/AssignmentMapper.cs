using System.Data;
using Pepro.DataAccess.Entities;

namespace Pepro.DataAccess.Mappings;

static class AssignmentMapper
{
    public static Assignment FromDataRow(DataRow row)
    {
        return new Assignment()
        {
            AssignmentId = row.Field<int>("AssignmentId"),
            Name = row.Field<string>("Name") ?? "",
            IsPublicToProject = row.Field<bool>("IsPublicToProject"),
            IsPublicToDepartment = row.Field<bool>("IsPublicToDepartment"),
            StartDate = row.Field<DateTime>("StartDate"),
            EndDate = row.Field<DateTime>("EndDate"),
            RequiredDocumentCount = row.Field<int>("RequiredDocumentCount"),
            ManagerId = row.Field<int?>("ManagerId"),
            ProjectId = row.Field<int>("ProjectId"),
            StatusId = row.Field<int>("StatusId"),
            IsDeleted = row.Field<bool>("IsDeleted"),
            CreatedAt = row.Field<DateTime>("CreatedAt"),
            UpdatedAt = row.Field<DateTime>("UpdatedAt"),
            DeletedAt = row.Field<DateTime?>("DeletedAt"),
        };
    }
}
