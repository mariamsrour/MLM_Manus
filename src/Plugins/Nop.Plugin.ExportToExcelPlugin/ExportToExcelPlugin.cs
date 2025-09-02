using Nop.Core.Plugins;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.ExportToExcelPlugin;

public class ExportToExcelPlugin : BasePlugin, IWidgetPlugin
{
    public override string GetConfigurationPageUrl()
    {
        return "/Admin/ExportToExcel/Configure";
    }

    public override async Task InstallAsync()
    {
        // Perform installation logic if needed
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        // Perform uninstallation logic if needed
        await base.UninstallAsync();
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        // Specify where the widget will be rendered
        return Task.FromResult<IList<string>>(new List<string> { "admin_datatable_buttons" });
    }

    public string GetWidgetViewComponentName(string widgetZone)
    {
        return "ExportToExcelWidget"; // Matches the ViewComponent class name
    }
}
