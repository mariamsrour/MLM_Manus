using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.ExportToExcelPlugin;
using Nop.Web.Areas.Admin.Controllers;
using System.Collections.Generic;
using Nop.Web.Areas.Admin.Factories;
using Nop.Core;
using NPOI.XSSF.UserModel;


namespace Nop.Web.Areas.Admin.Controllers
{
    public class ExportToExcelController : BaseAdminController
    {

        public ExportToExcelController()
        {
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual IActionResult ExportToExcel([FromBody] ExportRequestModel request)
        {
            if (request == null)
            {
                // Log error
                return BadRequest("Request is null.");
            }

            if (request.Data == null || request.Columns == null)
            {
                // Log error
                return BadRequest("Missing data or columns.");
            }
            var fileData = ExportDataTableToExcel(request.Data,request.Columns, "Exported Data");
            return File(fileData, MimeTypes.TextXlsx, $"{request.ViewName}.xlsx");
        }

        public byte[] ExportDataTableToExcel(IEnumerable<Dictionary<string, object>> data,IEnumerable<string> columns, string sheetName = "Data")
        {
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet(sheetName);

            // Create header row
            var headerRow = sheet.CreateRow(0);
            int colIndex = 0;
            foreach (var column in columns)
            {
                if(column != "CustomProperties")
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
                    if (column != "CustomProperties")
                    {
                        string cellValue = string.Empty;
                        if (row is Dictionary<string, object> dictRow && dictRow.ContainsKey(column))
                        {
                            cellValue = dictRow[column]?.ToString();
                        }
                        else if (row != null)
                        {
                            var prop = row.GetType().GetProperty(column);
                            if (prop != null)
                            {
                                cellValue = prop.GetValue(row)?.ToString();
                            }
                        }
                        //var cellValue = row.GetType().GetProperty(column)?.GetValue(row, null)?.ToString();
                        dataRow.CreateCell(colIndex++).SetCellValue(cellValue ?? string.Empty);
                    }
                       
                }
            }

            using (var stream = new MemoryStream())
            {
                workbook.Write(stream);
                return stream.ToArray();
            }
        }
    }

}

