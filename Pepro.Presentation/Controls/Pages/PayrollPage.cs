using System.Globalization;
using Pepro.Business;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class PayrollPage : MediatedTemplate
{
    public PayrollPage()
    {
        InitializeComponent();
        payrollDataGridView.SetupCellStyle();
        fullNameColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

        exportButton.ApplyFlatStyle();
    }

    private void LoadPayroll()
    {
        payrollDataGridView.DataSource = SalaryBusiness
            .Instance.GetPayroll()
            .ToList();
    }

    private void PayrollPage_Load(object sender, EventArgs e)
    {
        LoadPayroll();
        baseSalaryLabel.Text += SalaryBusiness.BASE_SALARY.ToString(
            "C",
            CultureInfo.CreateSpecificCulture("vi-VN")
        );
    }

    private void ExportButton_Click(object sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog = new()
        {
            FileName = "Payroll-" + DateTime.Now.ToString("ddMMyyyy-HHmmss"),
            Filter = "Excel Files|*.xlsx",
            Title = "Save Excel File",
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            ExcelExporter.Export(saveFileDialog.FileName, payrollDataGridView);
        }
    }
}
