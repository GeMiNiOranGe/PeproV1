using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class SalaryScaleBusiness {
    private static SalaryScaleBusiness? _instance;

    public static SalaryScaleBusiness Instance {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryScaleBusiness() { }

    public IEnumerable<SalaryScaleDto> GetSalaryScales() {
        IEnumerable<SalaryScale> salaryScales = SalaryScaleDataAccess.Instance.GetMany();
        return salaryScales.ToDtos();
    }

    public SalaryScaleDto? GetSalaryScaleBySalaryLevelId(int salaryLevelId) {
        SalaryScale? salaryScale = SalaryScaleDataAccess.Instance.GetBySalaryLevelId(salaryLevelId);
        return salaryScale?.ToDto();
    }
}
