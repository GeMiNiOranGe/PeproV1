using ClosedXML.Excel;

namespace Pepro.Presentation.Utilities;

public static class ExcelExporter
{
    /// <summary>
    /// Exports the data from the specified <see cref="DataGridView"/> into an Excel file.
    /// </summary>
    /// <param name="outputFilePath">
    /// The full file path where the Excel document will be saved.
    /// </param>
    /// <param name="dataGridView">
    /// The <see cref="DataGridView"/> instance whose data will be exported.
    /// </param>
    /// <remarks>
    /// This method creates a new Excel workbook, writes the column headers and cell values
    /// from the provided <see cref="DataGridView"/>, automatically adjusts the column widths,
    /// and then saves the workbook to the specified file path.
    /// </remarks>
    public static void Export(string outputFilePath, DataGridView dataGridView)
    {
        using XLWorkbook workbook = new();

        // Add a new worksheet named "Sheet1" to the workbook.
        IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");

        int rowIndex = 1;
        int columnIndex = 1;

        // Write the column headers from the DataGridView to the first row of the worksheet.
        foreach (DataGridViewColumn header in dataGridView.Columns)
        {
            worksheet.Cell(rowIndex, columnIndex++).Value = header.HeaderText;
        }

        // Write each DataGridView row’s cell values into subsequent worksheet rows.
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            rowIndex++;
            columnIndex = 1;

            foreach (DataGridViewCell cell in row.Cells)
            {
                // Convert the cell value to string to ensure compatibility with Excel cell values.
                worksheet.Cell(rowIndex, columnIndex++).Value =
                    cell.Value?.ToString();
            }
        }

        // Automatically resize all columns to fit their content for better readability.
        worksheet.Columns().AdjustToContents();

        // Save the workbook to the specified output file path.
        workbook.SaveAs(outputFilePath);
    }
}
