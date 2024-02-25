using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;
using tongDe.Models;

namespace tongDe.Controllers;

[Route("[controller]"), Authorize]
public class ShopController : Controller
{
    private readonly ILogger<ShopController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public ShopController(
        ILogger<ShopController> logger,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            _logger.LogError($"Can not found use by id (ID: {user.Id}.)");
            throw new Exception($"Can not found use by id (ID: {user.Id}.)");
        }

        var Shops = await _dbContext.Shops.Where(s => s.UserId == user.Id).ToListAsync();
        return View(Shops);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError($"Can not found use by id (ID: {userId}.)");
            throw new Exception($"Can not found use by id (ID: {userId}.)");
        }

        Shop newShop = new Shop
        {
            UserId = userId
        };

        return View(newShop);
    }

    [HttpPost("Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Shop shop)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null) return RedirectToAction("Login", "Account");

        try
        {
            user.Shops.Add(shop);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the database.");
            return View("Home/Shared/Error", new ErrorViewModel { ErrorMessage = "Unable to save changes. Try again, and if the problem persists, see your system administrator." });
        }

        return RedirectToAction("Index", "Shop");
    }
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var shop = await _dbContext.Shops
                                       .Include(s => s.Clients)
                                       .FirstOrDefaultAsync(s => s.Id == id);

        if (shop is null)
        {
            _logger.LogError($"Shop:({id}) is not found");
            return NotFound();
        }

        return View(shop);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}
