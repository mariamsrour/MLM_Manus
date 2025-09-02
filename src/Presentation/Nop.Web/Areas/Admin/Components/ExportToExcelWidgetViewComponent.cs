using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Web.Areas.Admin.Components
{
    [ViewComponent]

    public class ExportToExcelWidget : NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        // Determine if the export button should be displayed
        if (widgetZone != "admin_datatable_buttons")
            return Content(string.Empty);

        ViewBag.viewName = (string)additionalData; // Pass the view name dynamically
        return View();
    }
}
}

