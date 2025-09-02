using Microsoft.AspNetCore.Mvc;
using Nop.Core.Caching;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Infrastructure.Cache;

namespace Nop.Web.Components;

public partial class TopMenuViewComponent : NopViewComponent
{
    protected readonly ICatalogModelFactory _catalogModelFactory;
    private readonly IStaticCacheManager _staticCacheManager;


    public TopMenuViewComponent(ICatalogModelFactory catalogModelFactory,
            IStaticCacheManager staticCacheManager)
    {
        _catalogModelFactory = catalogModelFactory;
        _staticCacheManager = staticCacheManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? productThumbPictureSize)
    {
        var model = await _staticCacheManager.GetAsync(NopModelCacheDefaults.CategoryAllModelKey, async () =>
        {
            return await _catalogModelFactory.PrepareTopMenuModelAsync();
        });

        return View(model);
    }
}