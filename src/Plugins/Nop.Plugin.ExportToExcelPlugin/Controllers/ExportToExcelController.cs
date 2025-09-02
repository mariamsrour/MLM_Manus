using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.ExportToExcelPlugin;

public class ExportToExcelController : BaseAdminController
{
    private readonly ExcelExportService _excelExportService;

    public ExportToExcelController(ExcelExportService excelExportService)
    {
        _excelExportService = excelExportService;
    }

    [ValidateAntiForgeryToken]
    [Route("Admin/ExportData")]
    [HttpPost]
    public virtual public FileResult ExportToExcel([FromBody] ExportRequestModel request)
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
       
        var fileData = _excelExportService.ExportDataTableToExcel(data, columns, "Exported Data");
        return File(fileData, MimeTypes.TextXlsx, $"{viewName}.xlsx");
    }
}
