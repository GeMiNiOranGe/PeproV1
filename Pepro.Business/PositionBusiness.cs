using Pepro.Business.Contracts;
using Pepro.Business.Mappings;
using Pepro.DataAccess;
using Pepro.DataAccess.Entities;

namespace Pepro.Business;

public class PositionBusiness {
    private static PositionBusiness? _instance;

    public static PositionBusiness Instance {
        get => _instance ??= new();
        private set => _instance = value;
    }

    private PositionBusiness() { }

    public IEnumerable<PositionDto> GetPositions() {
        IEnumerable<Position> position = PositionDataAccess.Instance.GetMany();
        return position.ToDtos();
    }

    public string GetPositionTitleByEmployeeId(int employeeId) {
        Position? position = PositionDataAccess.Instance.GetByEmployeeId(employeeId);
        return position != null ? position.Title : "";
    }
}
