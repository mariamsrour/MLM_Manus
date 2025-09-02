﻿using Nop.Web.Framework.Models;

namespace Nop.API.Models.Common;

public partial record StoreThemeSelectorModel : BaseNopModel
{
    public StoreThemeSelectorModel()
    {
        AvailableStoreThemes = new List<StoreThemeModel>();
    }

    public IList<StoreThemeModel> AvailableStoreThemes { get; set; }

    public StoreThemeModel CurrentStoreTheme { get; set; }
}