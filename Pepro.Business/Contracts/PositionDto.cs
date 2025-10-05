namespace Pepro.Business.Contracts;

public class PositionDto
{
    private int _positionId;
    private string _title = "";
    private decimal _allowancePercent;

    public int PositionId
    {
        get => _positionId;
        set => _positionId = value;
    }

    public string Title
    {
        get => _title;
        set => _title = value;
    }

    public decimal AllowancePercent
    {
        get => _allowancePercent;
        set => _allowancePercent = value;
    }
}
