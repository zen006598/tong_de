using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tongDe.Models;
using tongDe.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;


    public HomeController(
        ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(int? shopId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return NotFound();
        IQueryable<Shop> shopsQuery = _dbContext.Shops.Where(shop => shop.UserId == user.Id);

        var homeIndexVM = new HomeIndexVM
        {
            Shops = shopsQuery.ToList()
        };

        if (shopId.HasValue && shopId.Value != 0)
        {
            homeIndexVM.SelectedShop = await _dbContext.Shops.FindAsync(shopId.Value);
        }
        else
        {
            homeIndexVM.SelectedShop = homeIndexVM.Shops.FirstOrDefault();
        }

        return View(homeIndexVM);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var logMessage = $@"
            Request ID: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}
            Error Message: {exceptionHandlerPathFeature?.Error.Message}
            Source: {exceptionHandlerPathFeature?.Error.Source}
            ErrorPath: {exceptionHandlerPathFeature?.Path}
            StackTrace: {exceptionHandlerPathFeature?.Error.StackTrace}
            InnerException: {exceptionHandlerPathFeature?.Error.InnerException}";

        _logger.LogError(logMessage);

        return View(
            new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = exceptionHandlerPathFeature?.Error.Message,
            });
    }
}
