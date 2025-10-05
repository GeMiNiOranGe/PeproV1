using Pepro.Business.Contracts;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Mappings;

static class SalaryLevelMapper
{
    public static SalaryLevelDto ToDto(this SalaryLevel entity)
    {
        return new SalaryLevelDto()
        {
            SalaryLevelId = entity.SalaryLevelId,
            Level = entity.Level,
            Coefficient = entity.Coefficient,
            SalaryScaleId = entity.SalaryScaleId,
        };
    }

    public static IEnumerable<SalaryLevelDto> ToDtos(this IEnumerable<SalaryLevel> entities)
    {
        return entities.Select(entity => entity.ToDto());
    }
}
