using Pepro.Business.Contracts;
using Pepro.Business.Utilities;
using Pepro.DataAccess.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Mappings;

static class EmployeeMapper
{
    public static InsertEmployeeModel ToInsertModel(this EmployeeDto dto)
    {
        return new InsertEmployeeModel()
        {
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            TaxCode = EncryptionConverter.EncryptFromString(dto.TaxCode),
            CitizenId = dto.CitizenId,
            DepartmentId = dto.DepartmentId,
            PositionId = dto.PositionId,
            SalaryLevelId = dto.SalaryLevelId,
        };
    }

    public static EmployeeDto ToDto(this Employee entity)
    {
        return new EmployeeDto()
        {
            EmployeeId = entity.EmployeeId,
            FirstName = entity.FirstName,
            MiddleName = entity.MiddleName,
            LastName = entity.LastName,
            DateOfBirth = entity.DateOfBirth,
            Gender = entity.Gender,
            TaxCode = EncryptionConverter.DecryptToString(entity.TaxCode),
            CitizenId = entity.CitizenId,
            DepartmentId = entity.DepartmentId,
            PositionId = entity.PositionId,
            SalaryLevelId = entity.SalaryLevelId,
        };
    }

    public static IEnumerable<EmployeeDto> ToDtos(this IEnumerable<Employee> entities)
    {
        return entities.Select(entity => entity.ToDto());
    }
}
