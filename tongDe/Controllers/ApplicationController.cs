using Microsoft.AspNetCore.Mvc;

namespace tongDe.Controllers;

public class ApplicationController : Controller
{
    protected readonly ILogger<ApplicationController> _logger;

    protected ApplicationController(ILogger<ApplicationController> logger)
    {
        _logger = logger;
    }
}