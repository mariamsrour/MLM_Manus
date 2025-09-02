using Microsoft.AspNetCore.Mvc;

namespace Nop.Plugin.ExportToExcelPlugin;


public class ExportToExcelButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string viewName)
    {
        return View("~/Plugins/ExportToExcelPlugin/Views/ExportButton.cshtml", viewName);
    }
}
