using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace Nop.Plugin.ExportToExcelPlugin;

public class ExcelExportService
{
    public byte[] ExportDataTableToExcel(IEnumerable<dynamic> data, IEnumerable<string> columns, string sheetName = "Data")
    {
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet(sheetName);

        // Create header row
        var headerRow = sheet.CreateRow(0);
        int colIndex = 0;
        foreach (var column in columns)
        {
            headerRow.CreateCell(colIndex++).SetCellValue(column);
        }

        // Add data rows
        int rowIndex = 1;
        foreach (var row in data)
        {
            var dataRow = sheet.CreateRow(rowIndex++);
            colIndex = 0;
            foreach (var column in columns)
            {
                var cellValue = row.GetType().GetProperty(column)?.GetValue(row, null)?.ToString();
                dataRow.CreateCell(colIndex++).SetCellValue(cellValue ?? string.Empty);
            }
        }

        using (var stream = new MemoryStream())
        {
            workbook.Write(stream);
            return stream.ToArray();
        }
    }
}

