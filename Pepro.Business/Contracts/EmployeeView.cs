namespace Pepro.Business.Contracts;

public class EmployeeView : EmployeeDto
{
    private string _departmentName = "";
    private string _positionTitle = "";

    public string? GenderDisplay
    {
        get
        {
            if (Gender.HasValue)
            {
                return Gender.Value ? "Nam" : "Nữ";
            }
            return "";
        }
    }

    public string DepartmentName
    {
        get => _departmentName;
        set => _departmentName = value;
    }

    public string PositionTitle
    {
        get => _positionTitle;
        set => _positionTitle = value;
    }
}