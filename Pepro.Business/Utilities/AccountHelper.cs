using System.Text;
using Pepro.Business.Extensions;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Utilities;

static class AccountHelper
{
    public static string GenerateDefaultUsername(Employee employee)
    {
        StringBuilder raw = new();
        raw.Append(employee.LastName[0]);
        raw.Append(employee.MiddleName.GetWordInitials());
        raw.Append(employee.FirstName);
        raw.Append(employee.EmployeeId);
        return raw.ToString().ToLower().ToNonAccentVietnamese();
    }
}
