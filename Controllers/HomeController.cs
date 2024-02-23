using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tongDe.Models;
using tongDe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;

namespace tongDe.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public HomeController(
        ILogger<HomeController> logger,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var shops = _dbContext.Shops.ToList();
        return View(shops);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var shop = new Shop();
        return View(shop);
    }
    [HttpPost("Create")]
    public async Task<IActionResult> Create(Shop shop)
    {
        try
        {
            _dbContext.Shops.Add(shop);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return View(shop);
        }

        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var execeptionHandlerPathFeture = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var logMessage = $@"
            Request ID: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}
            Error Message: {execeptionHandlerPathFeture.Error.Message}
            Source: {execeptionHandlerPathFeture.Error.Source}
            ErrorPath: {execeptionHandlerPathFeture.Path}
            StackTrace: {execeptionHandlerPathFeture.Error.StackTrace}
            InnerException: {execeptionHandlerPathFeture.Error.InnerException}";

        _logger.LogError(logMessage);

        return View(
            new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = execeptionHandlerPathFeture.Error.Message,
            });
    }
}
