using System.Data;
using Pepro.DataAccess.Entities;

namespace Pepro.DataAccess.Mappings;

static class EmployeeMapper
{
    public static Employee FromDataRow(DataRow row)
    {
        return new Employee()
        {
            EmployeeId = row.Field<int>("EmployeeId"),
            FirstName = row.Field<string>("FirstName") ?? "",
            MiddleName = row.Field<string?>("MiddleName"),
            LastName = row.Field<string>("LastName") ?? "",
            DateOfBirth = DateOnly.FromDateTime(
                row.Field<DateTime>("DateOfBirth")
            ),
            Gender = row.Field<bool?>("Gender"),
            TaxCode = row.Field<byte[]?>("TaxCode"),
            CitizenId = row.Field<string>("CitizenId") ?? "",
            DepartmentId = row.Field<int>("DepartmentId"),
            PositionId = row.Field<int>("PositionId"),
            SalaryLevelId = row.Field<int>("SalaryLevelId"),
            IsDeleted = row.Field<bool>("IsDeleted"),
            CreatedAt = row.Field<DateTime>("CreatedAt"),
            UpdatedAt = row.Field<DateTime>("UpdatedAt"),
            DeletedAt = row.Field<DateTime?>("DeletedAt"),
        };
    }
}
