namespace Pepro.Business.Contracts;

public class PhoneNumberDto
{
    private int _phoneNumberId;
    private int _employeeId;
    private string _number = "";

    public int PhoneNumberId
    {
        get => _phoneNumberId;
        set => _phoneNumberId = value;
    }

    public int EmployeeId
    {
        get => _employeeId;
        set => _employeeId = value;
    }

    public string Number
    {
        get => _number;
        set => _number = value;
    }
}
