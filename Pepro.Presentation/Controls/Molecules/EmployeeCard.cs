using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using System.ComponentModel;

namespace Pepro.Presentation.Controls.Molecules;

public partial class EmployeeCard : CardTemplate
{
    private EmployeeDto _item = null!;

    public EmployeeCard()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EmployeeDto Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            employeeIdLabel.Text = _item.EmployeeId.ToString();
            employeeNameLabel.Text = _item.FullName;
        }
    }
}
