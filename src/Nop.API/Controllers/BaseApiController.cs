using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Localization;

namespace Nop.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected readonly IWorkContext _workContext;
    protected readonly ILocalizationService _localizationService;

    public BaseApiController(IWorkContext workContext, ILocalizationService localizationService)
    {
        _workContext = workContext;
        _localizationService = localizationService;
    }

    // Common helpers can go here
}

