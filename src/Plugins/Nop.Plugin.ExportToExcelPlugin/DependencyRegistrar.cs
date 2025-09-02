namespace Nop.Plugin.ExportToExcelPlugin;

public class DependencyRegistrar : IDependencyRegistrar
{
    public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
    {
        services.AddScoped<ExcelExportService>();
    }
}
