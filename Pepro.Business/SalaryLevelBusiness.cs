using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class SalaryLevelBusiness {
    private static SalaryLevelBusiness? _instance;

    public static SalaryLevelBusiness Instance {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private SalaryLevelBusiness() { }

    public IEnumerable<SalaryLevelDto> GetSalaryLevelsBySalaryScaleId(int salaryScaleId) {
        IEnumerable<SalaryLevel> salaryLevels = SalaryLevelDataAccess.Instance.GetManyBySalaryScaleId(salaryScaleId);
        return salaryLevels.ToDtos();
    }
}
