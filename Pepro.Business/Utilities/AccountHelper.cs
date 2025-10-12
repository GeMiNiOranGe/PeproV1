using System.Text;
using Pepro.Business.Extensions;
using Pepro.DataAccess.Entities;

namespace Pepro.Business.Utilities;

static class AccountHelper
{
    /// <summary>
    /// Generates a default username for the specified employee.
    /// </summary>
    /// <param name="employee">
    /// The <see cref="Employee"/> object used to generate the username.
    /// </param>
    /// <returns>
    /// A lowercase, non-accent Vietnamese string representing
    /// the default username based on the employee's name and ID.
    /// </returns>
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
