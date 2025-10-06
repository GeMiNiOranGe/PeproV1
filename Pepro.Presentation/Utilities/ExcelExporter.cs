using ClosedXML.Excel;

namespace Pepro.Presentation.Utilities;

public static class ExcelExporter
{
    public static void Export(string outputFilePath, DataGridView dataGridView)
    {
        using XLWorkbook workbook = new();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");

        int rowIndex = 1;
        int columnIndex = 1;

        foreach (DataGridViewColumn header in dataGridView.Columns)
        {
            worksheet.Cell(rowIndex, columnIndex++).Value = header.HeaderText;
        }

        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            rowIndex++;
            columnIndex = 1;
            foreach (DataGridViewCell cell in row.Cells)
            {
                worksheet.Cell(rowIndex, columnIndex++).Value =
                    cell.Value?.ToString();
            }
        }

        worksheet.Columns().AdjustToContents();
        workbook.SaveAs(outputFilePath);
    }
}
